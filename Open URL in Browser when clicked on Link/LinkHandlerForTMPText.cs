using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ChristinaCreatesGames.Typography.InteractingWithText
{
    [RequireComponent(typeof(TMP_Text))]
    public class LinkHandlerForTMPTextWithURLClickability : MonoBehaviour, IPointerClickHandler
    {
        private TMP_Text _tmpTextBox;
        private Canvas _canvasToCheck;
        private Camera _cameraToUse;
        
        public static event Action<string> ClickedOnLink;

        private void Awake()
        {
            _tmpTextBox = GetComponent<TMP_Text>();
            _canvasToCheck = GetComponentInParent<Canvas>();

            if (_canvasToCheck.renderMode == RenderMode.ScreenSpaceOverlay)
                _cameraToUse = null;
            else
                _cameraToUse = _canvasToCheck.worldCamera;
        }
        

        public void OnPointerClick(PointerEventData eventData)
        {
            Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, 0);

            var linkTaggedText = TMP_TextUtilities.FindIntersectingLink(_tmpTextBox, mousePosition, _cameraToUse);

            if (linkTaggedText == -1) return;
            
            TMP_LinkInfo linkInfo = _tmpTextBox.textInfo.linkInfo[linkTaggedText];
            
            string linkID = linkInfo.GetLinkID();
            if (linkID.StartsWith("http://") || linkID.StartsWith("https://"))
            {
                Application.OpenURL(linkID);
                return;
            }

            OnClickedOnLinkEvent?.Invoke(linkInfo.GetLinkText());
        }
    }
}
