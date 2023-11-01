using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector; // only if you have Odin Inspector
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChristinaCreatesGames.EyeMovement
{
    public class PupilTracking : MonoBehaviour
    {
        [Header("Eyes and Target")]
        [SerializeField] private List<Eye> eyes;
        [SerializeField, Space] private string tagOfObjectToTrack = "EyeTrackingTarget";
        
        [Header("Setup")]
        [SerializeField] private ViewConeShape viewConeShape = ViewConeShape.Circle;
        enum ViewConeShape
        {
            Circle,
            Polygon,
            Box
        }
        [SerializeField] private Color areaColor = Color.red;
        private Collider2D _visionCollider;
        
        
        [Header("Tracking")]
        [SerializeField] private float eyeMovementSpeed = 50f;
        [SerializeField] private float resetEyeAfterSeconds = 1.25f;
        [SerializeField] private float minimumMovementDistanceUntilTracked = 0.025f;
        private float _longestResetTime;
        private Transform _targetToTrack;
        private bool _targetInVision;
        
        private Vector3 _targetPositionLastFrame;
        private bool _targetMoved;
        private float _currentlyWatchingForSeconds;
        private float _idlingSinceSeconds;
        private float _elapsedTimeSinceResetStart;
        
        private State _state = State.Idle;
        
        enum State
        {
            Idle,
            Tracking,
            Resetting
        }
        

        [ContextMenu("Create Collider")] // use this if you don't have Odin Inspector
        [PropertySpace(25), Button] // use this if you do
        private void CreateCollider()
        {
            if (viewConeShape == ViewConeShape.Circle && _visionCollider is CircleCollider2D)
                return;

            if (viewConeShape == ViewConeShape.Polygon && _visionCollider is PolygonCollider2D)
                return;
            
            if (viewConeShape == ViewConeShape.Box && _visionCollider is BoxCollider2D)
                return;

            Collider2D[] colliders = gameObject.GetComponents<Collider2D>();

            foreach (var entry in colliders)
            {
                DestroyImmediate(entry);
            }


            if (viewConeShape == ViewConeShape.Circle)
                _visionCollider = gameObject.AddComponent<CircleCollider2D>();
            else if (viewConeShape == ViewConeShape.Polygon)
                _visionCollider = gameObject.AddComponent<PolygonCollider2D>();
            else if (viewConeShape == ViewConeShape.Box)
                _visionCollider = gameObject.AddComponent<BoxCollider2D>();

            _visionCollider.isTrigger = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = areaColor;
            if (_visionCollider == null)
                return;
            
            switch (_visionCollider)
            {
                case CircleCollider2D circleCollider:
                    Gizmos.DrawWireSphere(circleCollider.transform.position, circleCollider.radius);
                    break;
                case PolygonCollider2D polyCollider:
                {
                    var points = polyCollider.GetPath(0);
                    for (var i = 0; i < points.Length; i++)
                    {
                        int next;
                    
                        if (i + 1 >= points.Length)
                            next = 0;
                        else
                            next = i + 1;

                        Vector3 initialPoint = polyCollider.transform.TransformPoint(points[i]);
                        Vector3 nextPoint = polyCollider.transform.TransformPoint(points[next]);
                        Gizmos.DrawLine(initialPoint, nextPoint);
                    }
                    break;
                }
                case BoxCollider2D boxCollider:
                {
                    Vector3 boxPosition = boxCollider.transform.position;
                    Vector3 size3D = boxCollider.size;

                    Vector2 boxPosition2D = new Vector2(boxPosition.x + boxCollider.offset.x, 
                        boxPosition.y + boxCollider.offset.y);
                    Vector2 size2D = new Vector2(size3D.x, size3D.y);

                    Gizmos.DrawWireCube(boxPosition2D, size2D);
                    break;
                }
            }
        }
        

        private void Awake()
        { 
            for (int i = 0; i < eyes.Count; i++)
            {
                if (eyes[i].DurationOfEyeResetMovement > _longestResetTime)
                    _longestResetTime = eyes[i].DurationOfEyeResetMovement;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(tagOfObjectToTrack))
                return;
            
            _targetToTrack = other.transform;
            _targetInVision = true;
            _currentlyWatchingForSeconds = 0;
            
            _state = State.Tracking;
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (_targetToTrack == null)
                return;

            _state = State.Resetting;
            _targetInVision = false;
        }
        
        private void Update()
        {
           if (_targetToTrack == null)
               return;
            
           switch (_state)
            {
                default:
                case State.Idle: 
                    foreach (Eye entry in eyes)
                    {
                        if (entry.IdleCoroutine == null)
                            entry.IdleCoroutine = StartCoroutine(IdleMovement(entry));
                    }
                    break;
                
                case State.Tracking:
                    StopRandomlyLookingAround();
                    Tracking();
                    break;
                
                case State.Resetting:
                    StopRandomlyLookingAround();
                    ResetPupil();
                    break;
            }            
        }

        private IEnumerator IdleMovement(Eye eye)
        {
            while (_state == State.Idle)
            {
                Vector3 randomPosition = Random.insideUnitCircle * (eye.PupilMovementRadius * 0.5f);

                float idleMovementDuration = 0f;

                while (idleMovementDuration < eye.DurationOfEyeResetMovement)
                {
                    if (_state != State.Idle)
                        yield break;

                    eye.Pupil.localPosition = Vector3.Lerp(eye.Pupil.localPosition, randomPosition,
                        eyeMovementSpeed * Time.deltaTime);

                    idleMovementDuration += Time.deltaTime;

                    yield return null;
                }

                yield return new WaitForSeconds(eye.DurationOfEyeResetMovement + 
                                                Random.Range(-eye.DurationOfEyeResetMovement * 0.25f, 1f));
            }
        }

        private void StopRandomlyLookingAround()
        {
            foreach (Eye eye in eyes)
            {
                if (eye.IdleCoroutine != null)
                {
                    StopCoroutine(eye.IdleCoroutine);
                    eye.IdleCoroutine = null;
                }
            }
        }

        private void Tracking()
        {
            if (!_targetInVision || (_currentlyWatchingForSeconds >= resetEyeAfterSeconds && !_targetMoved))
            {
                _state = State.Resetting;
                return;
            }

            CheckIfTargetMoved();

            if (!_targetMoved)
                return;

            foreach (Eye eye in eyes)
                PositionPupil(eye);
        }
        
        private void CheckIfTargetMoved()
        {
            if (Vector3.Distance(_targetPositionLastFrame, _targetToTrack.position) > minimumMovementDistanceUntilTracked)
            {
                _targetMoved = true;
                _targetPositionLastFrame = _targetToTrack.position;
                _currentlyWatchingForSeconds = 0;
                _state = State.Tracking;
            }
            else
            {
                _currentlyWatchingForSeconds += Time.deltaTime;
                _targetMoved = false;
            }
        }


        private void PositionPupil(Eye eye)
        {
            Vector3 directionToLookAt = _targetToTrack.position - eye.Eyeball.position;
            directionToLookAt.z = 0;
            
            Vector3 targetPupilPosition = eye.Eyeball.localPosition +
                                          Vector3.ClampMagnitude(directionToLookAt, eye.PupilMovementRadius);
            eye.Pupil.localPosition = Vector3.Lerp(eye.Pupil.localPosition, targetPupilPosition,
                eyeMovementSpeed * Time.deltaTime);
        }
        
        
        private void ResetPupil()
        {
            _elapsedTimeSinceResetStart += Time.deltaTime;

            foreach (Eye eye in eyes)
                ResetEyePupilPosition(eye);
            
            if (_elapsedTimeSinceResetStart > _longestResetTime)
            {
                _currentlyWatchingForSeconds = 0;
                _elapsedTimeSinceResetStart = 0f;

                if (!_targetInVision)
                    _state = State.Idle;
                else
                    _state = State.Tracking;
            }
        }
        
        private void ResetEyePupilPosition(Eye eye)
        {
            Vector3 initialPosition = eye.InitialPosition;

            float distance = Time.deltaTime * (1 / eye.DurationOfEyeResetMovement);

            eye.Pupil.localPosition = Vector3.MoveTowards(eye.Pupil.localPosition, initialPosition, distance);
        }
    }
}