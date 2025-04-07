using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ChristinaCreatesGames.UI
{
    public class ModifierButton : Selectable, IPointerClickHandler, ISubmitHandler
    {
        [Header("Input Conditionals")] 
        [SerializeField] private InputActionReference conditionalKey_A;
        [SerializeField] private InputActionReference conditionalKey_B;
        
        [Header("Click Events")] 
        public UnityEvent OnClick;
        public UnityEvent OnConditionalHeldDown_None;
        [Space]
        [Header("Modifier A Events")] 
        public UnityEvent OnConditionalClick_A;
        public UnityEvent OnConditionalHeldDown_A;
        [Space]
        [Header("Modifier B Events")] 
        public UnityEvent OnConditionalClick_B;
        public UnityEvent OnConditionalHeldDown_B;
        [Space]
        [Header("Modifier Both Events")] 
        public UnityEvent OnConditionalClick_Both;
        public UnityEvent OnConditionalHeldDown_Both;
        
        private Coroutine _resetRoutine;
        private WaitForSeconds _waitTimeFadeDuration;
        
        private void Reset()
        {
            var imageComponent = GetComponent<Image>();
            if (imageComponent is null)
                imageComponent = gameObject.AddComponent<Image>();

            targetGraphic = imageComponent;

            if (FindObjectOfType<InputActionHandler>() is null)
                Debug.Log("Make sure to add the ConditionalInputActionEnabler in your scene!");
        }

        protected override void Start()
        {
            base.Start();
            
            _waitTimeFadeDuration = new WaitForSeconds(colors.fadeDuration);

            if (conditionalKey_A?.action is not null)
            {
                conditionalKey_A.action.performed += NotifyAboutConditionalAHeldDown;
                conditionalKey_A.action.canceled += NotifyAboutConditionalAReleased;
            }

            if (conditionalKey_B?.action is not null)
            {
                conditionalKey_B.action.performed += NotifyAboutConditionalBHeldDown;
                conditionalKey_B.action.canceled += NotifyAboutConditionalBReleased;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (conditionalKey_A?.action is not null)
            {
                conditionalKey_A.action.performed -= NotifyAboutConditionalAHeldDown;
                conditionalKey_A.action.canceled -= NotifyAboutConditionalAReleased;
            }

            if (conditionalKey_B?.action is not null)
            {
                conditionalKey_B.action.performed -= NotifyAboutConditionalBHeldDown;
                conditionalKey_B.action.canceled -= NotifyAboutConditionalBReleased;
            }
        }


        private void NotifyAboutConditionalAHeldDown(InputAction.CallbackContext context)
        {
            if (conditionalKey_B?.action?.IsPressed() is true)
                OnConditionalHeldDown_Both?.Invoke();
            else
                OnConditionalHeldDown_A?.Invoke();
        }

        private void NotifyAboutConditionalBHeldDown(InputAction.CallbackContext context)
        {
            if (conditionalKey_A?.action?.IsPressed() is true)
                OnConditionalHeldDown_Both?.Invoke();
            else
                OnConditionalHeldDown_B?.Invoke();
        }

        private void NotifyAboutConditionalAReleased(InputAction.CallbackContext context)
        {
            if (conditionalKey_B?.action?.IsPressed() is true)
                OnConditionalHeldDown_B?.Invoke();
            else
                OnConditionalHeldDown_None?.Invoke();
        }

        private void NotifyAboutConditionalBReleased(InputAction.CallbackContext context)
        {
            if (conditionalKey_A?.action?.IsPressed() is true)
                OnConditionalHeldDown_A?.Invoke();
            else
                OnConditionalHeldDown_None?.Invoke();
        }
        

        public void OnPointerClick(PointerEventData eventData)
        {
            InputHandling();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            DoStateTransition(SelectionState.Pressed, true);
            
            InputHandling();
            
            if (_resetRoutine != null)
                StopCoroutine(OnFinishSubmit());
            _resetRoutine = StartCoroutine(OnFinishSubmit());
        }
        
        private void InputHandling()
        {
            bool isA = conditionalKey_A?.action?.IsPressed() is true;
            bool isB = conditionalKey_B?.action?.IsPressed() is true;

            if (isA && isB)
                OnConditionalClick_Both?.Invoke();
            else if (isA)
                OnConditionalClick_A?.Invoke();
            else if (isB)
                OnConditionalClick_B?.Invoke();
            else
                OnClick?.Invoke();
        }
        
        private IEnumerator OnFinishSubmit()
        {
            yield return _waitTimeFadeDuration;
            DoStateTransition(currentSelectionState, false);
        }
    }
}