using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace ChristinaCreatesGames.UI
{
    public class InputActionHandlerWithDeviceChange : MonoBehaviour
    {
        [Header("UI Input Conditionals")] 
        [SerializeField] private InputActionReference One;
        [SerializeField] private InputActionReference Two;
        [SerializeField] private InputActionReference Three;
        [SerializeField] private InputActionReference Four;
        
        private InputDevice _lastDevice;

        public static UnityEvent<InputDevice> OnUpdatedInputDevice = new UnityEvent<InputDevice>();

        
        private void Awake()
        {
            One?.action.Enable();
            Two?.action.Enable();
            Three?.action.Enable();
            Four?.action.Enable();
        }

        private void OnDestroy()
        {
            One?.action.Disable();
            Two?.action.Disable();
            Three?.action.Disable();
            Four?.action.Disable();
            
            InputSystem.onEvent -= OnInputEvent;
        }
        
        private void Start()
        {
            InputSystem.onEvent += OnInputEvent;
        }

        private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
        {
            if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
                return;
    
            if (_lastDevice == device) 
                return;
            
            _lastDevice = device;
            
            OnUpdatedInputDevice?.Invoke(_lastDevice);
        }
    }
}