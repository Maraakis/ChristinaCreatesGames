using UnityEngine;
using UnityEngine.EventSystems;

namespace ChristinaCreatesGames.UI
{
    public class UiSoundTrigger : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [Header("Types of Interaction")]
        [SerializeField] private InteractionSoundType soundType = InteractionSoundType.Unspecified;
        [SerializeField] private InteractionSoundOn playType = InteractionSoundOn.PointerDown;

        [Header("Manager")]
        [SerializeField] private UiInteractionSoundsManager soundManager;

        private void Reset()
        {
            soundManager = FindFirstObjectByType<UiInteractionSoundsManager>();
        }

        private void Start()
        {
            if (soundManager == null)
                Debug.LogError("UiInteractionSoundsManager has not been set. " +
                               "Either search manually or click Reset while not in play mode.", this);
        }

        public void PlaySound()
        {
            soundManager.PlaySound(soundType, this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (playType == InteractionSoundOn.PointerDown && soundManager != null)
                soundManager.PlaySound(soundType, this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (playType == InteractionSoundOn.PointerUp && soundManager != null)
                soundManager.PlaySound(soundType, this);
        }
    }
}