using UnityEngine;
using UnityEngine.UI;

namespace Christina.UI
{
    public class ToggleSwitchColorChange : ToggleSwitch
    {
        [Header("Elements to Recolor")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image handleImage;
        
        [Space]
        [SerializeField] private bool recolorBackground;
        [SerializeField] private bool recolorHandle;
        
        [Header("Colors")]
        [SerializeField] private Color backgroundColorOff = Color.white;
        [SerializeField] private Color backgroundColorOn = Color.white;
        [Space]
        [SerializeField] private Color handleColorOff = Color.white;
        [SerializeField] private Color handleColorOn = Color.white;
        
        private bool _isBackgroundImageNotNull;
        private bool _isHandleImageNotNull;
        
        protected override void OnValidate()
        {
            base.OnValidate();
            
            CheckForNull();
            ChangeColors();
        }

        private void OnEnable()
        {
            transitionEffect += ChangeColors;
        }
        
        private void OnDisable()
        {
            transitionEffect -= ChangeColors;
        }

        protected override void Awake() 
        {
            base.Awake();
            
            CheckForNull();
            ChangeColors();
        }

        private void CheckForNull()
        {
            _isBackgroundImageNotNull = backgroundImage != null;
            _isHandleImageNotNull = handleImage != null;
        }


        private void ChangeColors()
        {
            if (recolorBackground && _isBackgroundImageNotNull)
                backgroundImage.color = Color.Lerp(backgroundColorOff, backgroundColorOn, sliderValue); 
            
            if (recolorHandle && _isHandleImageNotNull)
                handleImage.color = Color.Lerp(handleColorOff, handleColorOn, sliderValue); 
        }
    }
}




