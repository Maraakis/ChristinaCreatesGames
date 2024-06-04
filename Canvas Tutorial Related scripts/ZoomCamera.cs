using Christina.Core_Systems;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ChristinaCreatesGames.CameraSystem
{
    // This code won't work out of the box for you. 
    // It's just an example to show where to place the IsPointerOverUI check. 
    public class ZoomCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cameraToZoom;
        [SerializeField] private float zoomSpeed = 2.0f;

        private float _targetZoom;

        private void Start()
        {
            // Game Event Manager is my own event system.
            // You can instead call HandleZoom from other scripts to test it.
            GameEventManager.Instance.InputEvents.OnZoom += HandleZoom;
            _targetZoom = cameraToZoom.m_Lens.FieldOfView;
        }

        private void OnDestroy()
        {
            GameEventManager.Instance.InputEvents.OnZoom -= HandleZoom;
        }

        private void Update()
        {
            var fov = cameraToZoom.m_Lens.FieldOfView;
            _targetZoom = Mathf.Lerp(fov, _targetZoom, Time.deltaTime * zoomSpeed);
        }

        private void HandleZoom(float zoomValue)
        {
            if (EventSystem.current.IsPointerOverGameObject()) // Check if the mouse is over a UI element
                return; // If it is, don't change field of view
            
            // Changes the field of view of the camera so it stays between 30 and 100
            cameraToZoom.m_Lens.FieldOfView = Mathf.Clamp(_targetZoom - zoomValue, 30, 100);
        }
    }
}