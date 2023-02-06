using System;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChristinaCreatesGames.Typography.LinkInteractions
{
    public class LinkEventInvokerStatic : MonoBehaviour
    {
        private TMP_Text _textbox;
        
        public static event Action<string> LinkFound;

        private void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(CheckForLinkEvent);
        }

        private void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(CheckForLinkEvent);
        }
        
        private void Awake()
        {
            _textbox = GetComponent<TMP_Text>();
        }

        private void CheckForLinkEvent(Object obj)
        {
            var amountOfLinksInCurrentText = _textbox.textInfo.linkCount;

            if (amountOfLinksInCurrentText == 0)
                return;

            for (int linkIndex = 0; linkIndex < amountOfLinksInCurrentText; linkIndex++)
            {
                var linkInfo = _textbox.textInfo.linkInfo[linkIndex];
                
                LinkFound?.Invoke(linkInfo.GetLinkID());
            }
        }
    }
}