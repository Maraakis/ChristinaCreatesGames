using TMPro;
using UnityEngine;
using UnityEngine.UI;

//* THIS IS SUPER PROTOTYPE-Y CODE, USE AT YOUR OWN RISK
namespace ChristinaCreatesGames.Prototype
{
    public class ResourceSlot : MonoBehaviour
    {
        private int _amount;
        public int Amount
        {
            get => _amount;
            set
            {
                _amount = Mathf.Clamp(value, 0, MaxAmount);
                AmountText.text = _amount.ToString();
                
                if (_amount > 0)
                {
                    labelContainer.SetActive(true);
                    Icon.gameObject.SetActive(true);
                }
                else
                {
                    labelContainer.SetActive(false);
                    Icon.gameObject.SetActive(false);
                }
            }
        }
        public int MaxAmount = 100;
        
        public Image Icon;
        public GameObject labelContainer;
        
        public TMP_Text AmountText;
        
        public bool isFilled => Amount > 0;
        public bool isEmpty => Amount == 0;
        public bool CanAdd => Amount < MaxAmount;

        public int StartingAmount = 0;

        private void Start()
        {
            Amount = StartingAmount;
        }

        private void OnValidate()
        {
            Amount = StartingAmount;
        }
    }
}
