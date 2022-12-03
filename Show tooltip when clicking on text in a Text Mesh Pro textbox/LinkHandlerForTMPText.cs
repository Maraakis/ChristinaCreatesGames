using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Link System Tutorial 1: Showing a tooltip by clicking on text in a Text Mesh Pro textbox
// Video tutorial: https://youtu.be/N6vYyCahLr8

// Hi! I'm Christina, this is my implementation of how to make text in a Text Mesh Pro textbox clickable. 
// This system uses three scripts:
// TooltipHandler - Controls the tooltip and I place it on my Canvas gameobject which contains the tooltip to control
// LinkHandlerForTMPText - Sits on the same gameobject as your Text Mesh Pro component, but could live anywhere else, too. It checks if you have clicked on a link in your textbox.
// TooltipInfos - a simple struct which contains relevant infos for the link. In this implementation, it uses an image to display, in the implementation on showing a tooltip while hovering, it contains different contents.
// Please watch the video if you have questions about how to set this up :)
// Hope you'll enjoy my system!
// - Christina

namespace ChristinaCreatesGames.Typography.TooltipForTMPbyClick
{
    [RequireComponent(typeof(TMP_Text))]
    public class LinkHandlerForTMPText : MonoBehaviour, IPointerClickHandler
    {
        private TMP_Text _tmpTextBox;
        private Canvas _canvasToCheck;
        [SerializeField] private Camera cameraToUse;
        
        public delegate void ClickOnLinkEvent(string keyword);
        public static event ClickOnLinkEvent OnClickedOnLinkEvent;

        private void Awake()
        {
            _tmpTextBox = GetComponent<TMP_Text>();
            _canvasToCheck = GetComponentInParent<Canvas>();

            if (_canvasToCheck.renderMode == RenderMode.ScreenSpaceOverlay)
                cameraToUse = null;
            else
                cameraToUse = _canvasToCheck.worldCamera;
        }
        

        public void OnPointerClick(PointerEventData eventData)
        {
            Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, 0);

            var linkTaggedText = TMP_TextUtilities.FindIntersectingLink(_tmpTextBox, mousePosition, cameraToUse);
            
            if (linkTaggedText != -1)
            {
                TMP_LinkInfo linkInfo = _tmpTextBox.textInfo.linkInfo[linkTaggedText];
                
                OnClickedOnLinkEvent?.Invoke(linkInfo.GetLinkText());
            }
        }
    }
}
