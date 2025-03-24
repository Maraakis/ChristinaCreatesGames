using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChristinaCreatesGames.UI
{
    public class DoubleClickButton : Selectable, IPointerClickHandler, ISubmitHandler
    {
        [Header("Click Settings")] 
        [SerializeField, Min(0.2f)] private float doubleClickThreshold = 0.2f;
        [SerializeField, Min(0.25f)] private float singleClickDelay = 0.25f;
        [SerializeField, Min(1f)] private float hoverDelay = 1f;
        [SerializeField] private bool hoverEnabled = true;

        [Header("Click Events")] 
        public UnityEvent OnClick;
        public UnityEvent OnDoubleClick;
        public UnityEvent OnHoverEnter;
        public UnityEvent OnHoverExit;

        private float _lastClickTime = 0f;
        private Coroutine _clickCoroutine;
        private Coroutine _hoverRoutine;

        private WaitForSeconds _waitTimeSingleClickDelay;
        private WaitForSeconds _waitTimeHoverDelay;
        
        private Coroutine _resetRoutine;
        
        protected override void Reset()
        {
            base.Reset();
           
            var imageComponent = GetComponent<Image>();
            if (imageComponent == null)
                imageComponent = gameObject.AddComponent<Image>();

            targetGraphic = imageComponent;
        }
        
        protected override void Start()
        {
            base.Start();
            
            _waitTimeSingleClickDelay = new WaitForSeconds(singleClickDelay);
            _waitTimeHoverDelay = new WaitForSeconds(hoverDelay);
        }
        
        #region Setup
        
        public void SetClickDelays(float singleClickDelay, float doubleClickThreshold)
        {
            this.singleClickDelay = singleClickDelay;
            this.doubleClickThreshold = doubleClickThreshold;
            _waitTimeSingleClickDelay = new WaitForSeconds(singleClickDelay);
        }

        public void SetHoverDelay(float hoverDelay)
        {
            this.hoverDelay = hoverDelay;
            _waitTimeHoverDelay = new WaitForSeconds(hoverDelay);
        }
        
        public void SetHoverEnabled(bool hoverEnabled)
        {
            this.hoverEnabled = hoverEnabled;
        }
        
        #endregion

        
        #region Clicking
        
        public void OnPointerClick(PointerEventData eventData)
        {
            HandlingInput();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            DoStateTransition(SelectionState.Pressed, true);
            
            HandlingInput();
            
            if (_resetRoutine != null)
                StopCoroutine(OnFinishSubmit());
           
            _resetRoutine = StartCoroutine(OnFinishSubmit());
        }

        private void HandlingInput()
        {
            if (!interactable)
                return;

            float timeSinceLastClick = Time.time - _lastClickTime;
            
            _lastClickTime = Time.time;

            if (timeSinceLastClick <= doubleClickThreshold)
                HandleDoubleClick();
            else
                HandleSingleClick();
        }

        private void HandleDoubleClick()
        {
            if (_clickCoroutine != null)
                StopCoroutine(_clickCoroutine);

            OnDoubleClick?.Invoke();
        }

        private void HandleSingleClick()
        {
            if (_clickCoroutine != null)
                StopCoroutine(_clickCoroutine);

            _clickCoroutine = StartCoroutine(SingleClickDelay());
        }

        private IEnumerator SingleClickDelay()
        {
            yield return _waitTimeSingleClickDelay;
            OnClick?.Invoke();
        }
        
        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }
        
        #endregion

        
        #region Hovering
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (!hoverEnabled)
                return;
            
            base.OnPointerEnter(eventData);
            
            if (_hoverRoutine != null)
                StopCoroutine(_hoverRoutine);
            
            _hoverRoutine = StartCoroutine(OnHoverDelay());
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (!hoverEnabled)
                return;
            
            base.OnPointerExit(eventData);
            
            OnHoverExit?.Invoke();
            
            if (_hoverRoutine != null)
                StopCoroutine(_hoverRoutine);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            if (!hoverEnabled)
                return;

            if (_hoverRoutine != null)
                StopCoroutine(_hoverRoutine);

            _hoverRoutine = StartCoroutine(OnHoverDelay());
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            if (!hoverEnabled)
                return;

            if (_hoverRoutine != null)
                StopCoroutine(_hoverRoutine);

            OnHoverExit?.Invoke();
        }
        
        private IEnumerator OnHoverDelay()
        {
            yield return _waitTimeHoverDelay;
            OnHoverEnter?.Invoke();
        }
        
        #endregion
    }
}