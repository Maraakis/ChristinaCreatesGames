using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace ChristinaCreatesGames.UI
{
    public class InputActionHandler : MonoBehaviour
    {
        [Header("UI Input Conditionals")] // These come from the Modifier Button Tutorial
        [SerializeField] private InputActionReference conditionalKeyA;
        [SerializeField] private InputActionReference conditionalKeyB;
        
        [Header("UI Action bar hotkeys")] //These come from the Hotkey Tutorial
        [SerializeField] private InputActionReference One;
        [SerializeField] private InputActionReference Two;
        [SerializeField] private InputActionReference Three;
        [SerializeField] private InputActionReference Four;

        private InputDevice _lastDevice;
        public static UnityEvent<InputDevice> OnUpdatedInputDevice = new UnityEvent<InputDevice>();
        
        private void Awake()
        {
            conditionalKeyA?.action.Enable();
            conditionalKeyB?.action.Enable();
            
            One?.action.Enable();
            Two?.action.Enable();
            Three?.action.Enable();
            Four?.action.Enable();
        }

        private void Start()
        {
            InputSystem.onEvent += OnInputEvent;
        }

        private void OnDestroy()
        {
            conditionalKeyA?.action.Disable();
            conditionalKeyB?.action.Disable();
            
            One?.action.Disable();
            Two?.action.Disable();
            Three?.action.Disable();
            Four?.action.Disable();
            
            InputSystem.onEvent -= OnInputEvent;
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