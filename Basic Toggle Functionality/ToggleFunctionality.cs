using AssetKits.ParticleImage;
using Christina.Animations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ChristinaCreatesGames.UI
{
    public class ToggleFunctionality : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private TMP_Text textfield;
        [SerializeField] private string message;
        [SerializeField] private SquashAndStretch squashAndStretch;
        [SerializeField] private ParticleImage particles;
        
        private void OnEnable()
        {
            toggle.onValueChanged.AddListener(SendMessage);
        }

        private void OnDisable()
        {
            toggle.onValueChanged.RemoveListener(SendMessage);
        }

        private void SendMessage(bool toggleValue)
        {
            if (toggleValue)
            {
                textfield.SetText(message);
                //particles.Play();
            }
            else
                textfield.SetText("Nothing set.");
            
            //squashAndStretch.PlaySquashAndStretch();
        }

        public void ToggleValueThroughScript()
        {
            toggle.isOn = !toggle.isOn;
        }
    }
}




