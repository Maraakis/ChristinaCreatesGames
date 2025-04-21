using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ChristinaCreatesGames.UI
{
    public class HotkeyButton : Selectable, IPointerClickHandler, ISubmitHandler
    {
        [Header("Input Conditionals")] 
        [SerializeField] private InputActionReference assignedHotkeyButton;

        [Header("Visuals Setup")] 
        [SerializeField] private TMP_Text hotkeyLabel;
        [SerializeField] private float longLabelFontSize = 35f;
        
        [Header("Click Events")] 
        public UnityEvent OnClick;
        
        private Coroutine _resetRoutine;
        private InputDevice _lastDevice;
        private WaitForSeconds _waitTimeFadeDuration;

        private float _initialLabelFontSize = 70f;
        
        private void Reset()
        {
            var imageComponent = GetComponent<Image>();
            if (imageComponent == null)
                imageComponent = gameObject.AddComponent<Image>();

            targetGraphic = imageComponent;
            
            hotkeyLabel = GetComponentInChildren<TMP_Text>();
        }

        public void AssignInputActionReference(InputActionReference reference)
        {
            assignedHotkeyButton = reference;
            SetLabelText(_lastDevice);
        }

        protected override void Start()
        {
            base.Start();
            
            _waitTimeFadeDuration = new WaitForSeconds(colors.fadeDuration);
            
            assignedHotkeyButton.action.performed += HotkeyClicked;
            
            InputActionHandlerWithDeviceChange.OnUpdatedInputDevice.AddListener(SetLabelText);
            SetLabelText(_lastDevice);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            assignedHotkeyButton.action.performed -= HotkeyClicked;
            
            InputActionHandlerWithDeviceChange.OnUpdatedInputDevice.RemoveListener(SetLabelText);
        }

        #region Labeling

        public void SetLabelText(InputDevice inputDevice)
        {
            _lastDevice = inputDevice;
            hotkeyLabel.SetText(GetAssignedButton());
        }
        
        private string GetAssignedButton()
        {
            if (assignedHotkeyButton != null && assignedHotkeyButton.action != null)
            {
                var action = assignedHotkeyButton.action;
                foreach (var binding in action.bindings)
                {
                    if (binding.isPartOfComposite || binding.isComposite) 
                        continue;
                    
                    if (IsCurrentDeviceBinding(binding))
                    {
                        return InputControlPath.ToHumanReadableString(
                            binding.effectivePath,
                            InputControlPath.HumanReadableStringOptions.OmitDevice);
                    }
                }
            }
            else
                Debug.LogWarning("InputActionReference or Action is null.");
            
            return "Not found.";
        }
        
        
        private bool IsCurrentDeviceBinding(InputBinding binding)
        {
            if (_lastDevice == null)
                return string.IsNullOrEmpty(binding.groups);

            if (binding.groups.Contains("Gamepad") && _lastDevice is Gamepad)
            {
                hotkeyLabel.fontSize = longLabelFontSize;
                return true;
            }

            if (binding.groups.Contains("Keyboard") && (_lastDevice is Keyboard || _lastDevice is Mouse))
            {
                hotkeyLabel.fontSize = _initialLabelFontSize;
                return true;
            }

            return string.IsNullOrEmpty(binding.groups);
        }
        #endregion

        #region ClickFunctionality
        private void Clicked()
        {
            OnClick?.Invoke();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked();
        }
        
        public void OnSubmit(BaseEventData eventData)
        {
            DoStateTransition(SelectionState.Pressed, true);
            
            Clicked();
            
            if (_resetRoutine != null)
                StopCoroutine(OnFinishSubmit());
            
            _resetRoutine = StartCoroutine(OnFinishSubmit());
        }

        private void HotkeyClicked(InputAction.CallbackContext obj)
        {
            DoStateTransition(SelectionState.Pressed, true);
            
            Clicked();
            
            if (_resetRoutine != null)
                StopCoroutine(OnFinishSubmit());
           
            _resetRoutine = StartCoroutine(OnFinishSubmit());
        }
        
        private IEnumerator OnFinishSubmit()
        {
            yield return _waitTimeFadeDuration;
            DoStateTransition(currentSelectionState, false);
        }
        #endregion
    }
}