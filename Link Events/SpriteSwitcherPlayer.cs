using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ChristinaCreatesGames.Typography.LinkInteractions
{
    [RequireComponent(typeof(Image))]
    public class SpriteSwitcherPlayer : MonoBehaviour
    {
        [SerializeField] private Sprite happy;
        [SerializeField] private Sprite surprised;
        [SerializeField] private Sprite content;
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
                case "PlayerHappy":
                    _imageToSwitch.sprite = happy;
                    break;
                case "PlayerSurprise":
                    _imageToSwitch.sprite = surprised;
                    break;
                case "PlayerContent":
                    _imageToSwitch.sprite = content;
                    break;
                case "Jam":
                    _imageToSwitch.sprite = surprised;
                    break;
                case "Cream":
                    _imageToSwitch.sprite = content;
                    break;
                default:
                    _imageToSwitch.sprite = happy;
                    break;
            }
        }
    }
}