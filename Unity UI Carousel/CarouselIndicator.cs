using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Christina.UI
{
    public class CarouselIndicator : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        
        private Coroutine _alphaChangeCoroutine;
        
        private UnityAction _onClickAction;

        public void Initialize(UnityAction onClickAction)
        {
            _onClickAction = onClickAction;
            button.onClick.AddListener(_onClickAction);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(_onClickAction);
        }

        private void Reset()
        {
            image = GetComponentInChildren<Image>();
            var color = image.color;
            color.a = 0;
            image.color = color;
        }

        public void Activate(float duration)
        {
            if (_alphaChangeCoroutine != null)
                StopCoroutine(_alphaChangeCoroutine);
            
            _alphaChangeCoroutine = StartCoroutine(ChangeAlpha(1, duration));
        }
        
        public void Deactivate(float duration)
        {
            if (_alphaChangeCoroutine != null)
                StopCoroutine(_alphaChangeCoroutine);
            
            _alphaChangeCoroutine = StartCoroutine(ChangeAlpha(0, duration));
        }
        
        private IEnumerator ChangeAlpha(float targetAlpha, float duration)
        {
            float startAlpha = image.color.a;
            float time = 0;
            while (time < duration)
            {
                time += Time.deltaTime;
                float lerpValue = time / duration;
                Color newColor = image.color;
                newColor.a = Mathf.Lerp(startAlpha, targetAlpha, lerpValue);
                image.color = newColor;
                yield return null;
            }
        }
    }
}