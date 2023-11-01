using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChristinaCreatesGames.EyeMovement
{
    public class Eye : MonoBehaviour
    {
        [Header("Eye Setup")]
        [SerializeField] private Transform eyeball;
        [SerializeField] private Transform pupil;
        [SerializeField] private float pupilMovementRadius;
        [SerializeField, Range(0,1.5f)] private float durationOfEyeResetMovement = 1f;
        
        [Header("Blinking")]
        [SerializeField] private bool doesBlink;
        [SerializeField] private float blinkInterval = 3f;

        private Vector3 _initialPosition = Vector3.zero;
        
        public Transform Eyeball => eyeball;
        public Transform Pupil => pupil;
        public Vector3 InitialPosition => _initialPosition;
        public float PupilMovementRadius => pupilMovementRadius;
        public float DurationOfEyeResetMovement => durationOfEyeResetMovement;
        
        
        public Coroutine IdleCoroutine { get; set; }
        
        private void Start()
        {
            if (doesBlink)
                StartCoroutine(Blink());
        }

        
        private IEnumerator Blink()
        {
            yield return new WaitForSeconds(blinkInterval + Random.Range(-blinkInterval * 0.25f, 1f));
            
            while (true)
            {
                float duration = 0.125f;

                Vector3 initialScale = transform.localScale;
                Vector3 blinkScale = new Vector3(initialScale.x, 0, initialScale.z);

                for (float time = 0; time < duration; time += Time.deltaTime)
                {
                    float factor = time / duration;
                    transform.localScale = Vector3.Lerp(initialScale, blinkScale, factor);
                    yield return null;
                }

                transform.localScale = blinkScale;
                
                for (float time = 0; time < duration; time += Time.deltaTime)
                {
                    float factor = time / duration;
                    transform.localScale = Vector3.Lerp(blinkScale, initialScale, factor);
                    yield return null;
                }

                transform.localScale = initialScale;

                yield return new WaitForSeconds(blinkInterval + Random.Range(-blinkInterval * 0.25f, 1f));
            }
        }
    }
}




