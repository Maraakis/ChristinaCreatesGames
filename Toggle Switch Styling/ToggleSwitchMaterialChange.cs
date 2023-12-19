using UnityEngine;
using UnityEngine.UI;

namespace Christina.UI
{
    public class ToggleSwitchMaterialChange : ToggleSwitch
    {
        [Header("Background")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Material backgroundMaterial;
        private Material _localCopyOfBackgroundMaterial;
        
        [Space]
        [SerializeField] private Image handleImage;
        [SerializeField] private Material handleMaterial;
        private Material _localCopyOfHandleMaterial;
        
        private bool _isBackgroundImageNotNull;
        private bool _isHandleImageNotNull;
        
        private bool _isBackgroundMaterialNotNull;
        private bool _isHandleMaterialNotNull;

        private new void OnValidate()
        {
            base.OnValidate();
            
            SetupBackgroundMaterial();
            SetupHandleMaterial();
            
            TransitionImages();
        }
        
        protected override void Awake() 
        {
            base.Awake();
            
            _isHandleMaterialNotNull = handleImage.material != null;
            _isBackgroundMaterialNotNull = backgroundImage.material != null;
            _isHandleImageNotNull = handleImage != null;
            _isBackgroundImageNotNull = backgroundImage != null;
            
            SetupBackgroundMaterial();
            SetupHandleMaterial();
            TransitionImages();
        }
        
        private void SetupHandleMaterial()
        {
            _localCopyOfHandleMaterial = new Material(handleMaterial);
            
            if (_isHandleImageNotNull)
                handleImage.material = _localCopyOfHandleMaterial;
        }

        private void SetupBackgroundMaterial()
        {
            _localCopyOfBackgroundMaterial = new Material(backgroundMaterial);
            
            if (_isBackgroundImageNotNull)
                backgroundImage.material = _localCopyOfBackgroundMaterial;
        }

        
        private void OnEnable()
        {
            transitionEffect += TransitionImages;
        }
        
        private void OnDisable()
        {
            transitionEffect -= TransitionImages;
        }

        private void TransitionImages()
        {
            if (_isBackgroundImageNotNull && _isBackgroundMaterialNotNull)
                backgroundImage.material.SetFloat("_MixValue", sliderValue); 

            if (_isHandleImageNotNull && _isHandleMaterialNotNull)
                handleImage.material.SetFloat("_MixValue", sliderValue);
        }
    }
}





