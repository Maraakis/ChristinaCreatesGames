using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChristinaCreatesGames.Typography.LinkInteractions
{
    [RequireComponent(typeof(Image))]
    public class SpriteSwitcherNPC : MonoBehaviour
    {
        [SerializeField] private Sprite neutral;
        [SerializeField] private Sprite talking;
        [SerializeField] private Sprite snickering;
        [SerializeField] private Sprite winking;
        [SerializeField] private Sprite thinking;
        [SerializeField] private List<Sprite> emotions = new List<Sprite>();
        private Image _imageToSwitch;

        private string _currentKeyword;

        private void Awake()
        {
            _imageToSwitch = GetComponent<Image>();
        }

        private void OnEnable()
        {
            //LinkEventInvokerStatic.LinkFound += SwitchSprite;
            LinkEventInvokerForTypewriter.LinkFound += SwitchSprite;
        }

        private void OnDisable()
        {
            //LinkEventInvokerStatic.LinkFound -= SwitchSprite;
            LinkEventInvokerForTypewriter.LinkFound -= SwitchSprite;
        }
        
        private void SwitchSprite(string keyword)
        {
            if (keyword == _currentKeyword) return;
            _currentKeyword = keyword;
            
            StartCoroutine(SpritePicker(keyword));
        }

        private IEnumerator SpritePicker(string keyword)
        {
            yield return new WaitForEndOfFrame();
            switch (keyword)
            {
                case "SarahTalk":
                    _imageToSwitch.sprite = talking;
                    break;
                case "SarahWink":
                    _imageToSwitch.sprite = winking;
                    break;
                case "SarahSnicker":
                    _imageToSwitch.sprite = snickering;
                    break;
                case "SconeBelow":
                    _imageToSwitch.sprite = neutral;
                    break;
                case "Jam":
                    _imageToSwitch.sprite = snickering;
                    break;
                case "Cream":
                    _imageToSwitch.sprite = winking;
                    break;
                case "StrawberryLeft":
                    _imageToSwitch.sprite = thinking;
                    break;
            }
        }
    }
}