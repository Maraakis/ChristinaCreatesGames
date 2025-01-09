using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Christina.UI
{
    public class CarouselButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float duration = 0.25f;
        [SerializeField] private Color hoverColor;
        [SerializeField] private Image buttonBackground;
        [SerializeField] private Button button;
        
        private Coroutine _alphaChangeCoroutine;

        private void OnValidate()
        {
            button = GetComponent<Button>();
            buttonBackground = GetComponent<Image>();
            
            if (button != null)
            {
                if (button.transition != Selectable.Transition.ColorTint)
                    return;

                var colorBlock = button.colors;
                colorBlock.normalColor = hoverColor;
                colorBlock.highlightedColor = hoverColor;
                colorBlock.pressedColor = hoverColor;
                colorBlock.selectedColor = hoverColor;
                colorBlock.disabledColor = Color.clear;
                button.colors = colorBlock;
            }
            
            if (buttonBackground != null)
                buttonBackground.color = hoverColor;
        }

        private void Start()
        {
            Color alphaHoverColor = hoverColor;
            alphaHoverColor.a = 0f;
            buttonBackground.color = alphaHoverColor;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_alphaChangeCoroutine != null)
                StopCoroutine(_alphaChangeCoroutine);
            
            _alphaChangeCoroutine = StartCoroutine(ChangeAlpha(1, duration));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_alphaChangeCoroutine != null)
                StopCoroutine(_alphaChangeCoroutine);
            
            _alphaChangeCoroutine = StartCoroutine(ChangeAlpha(0, duration));
        }
        
        private IEnumerator ChangeAlpha(float targetAlpha, float duration)
        {
            float startAlpha = buttonBackground.color.a;
            float time = 0;
            while (time < duration)
            {
                time += Time.deltaTime;
                float lerpValue = time / duration;
                Color newColor = buttonBackground.color;
                newColor.a = Mathf.Lerp(startAlpha, targetAlpha, lerpValue);
                buttonBackground.color = newColor;
                yield return null;
            }
        }
    }
}
