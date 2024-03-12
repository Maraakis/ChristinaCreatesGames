using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ChristinaCreatesGames.UI
{
    public class HighlightCopyLinks : MonoBehaviour, IPointerClickHandler
    {
        [Header("Highlight Settings")] [SerializeField]
        private Color highlightColor = Color.white;
        private string _hexValueOfColor;

        private Canvas _canvas;
        private Camera _cameraToUse;
        private string StartMarkTag => $"<mark=#" + _hexValueOfColor + ">";
        private string _endMarkTag = "</mark>";

        private TMP_Text _textField;
        private string _originalText;

        private bool _highlighted;
        private int _lastCheckedLink = -1;

        private void Awake()
        {
            _textField = GetComponent<TMP_Text>();
            _canvas = GetComponentInParent<Canvas>();
            
            _originalText = _textField.text;

            if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                _cameraToUse = null;
            else
                _cameraToUse = _canvas.worldCamera;
            
            _hexValueOfColor = ColorUtility.ToHtmlStringRGBA(highlightColor);
        }

        public void SetNewText(string newText)
        {
            _textField.SetText(newText);
            _originalText = newText;
            _lastCheckedLink = -1;
        }
        public void CopyToClipboard(int currentLink)
        {
            var linkInfo = _textField.textInfo.linkInfo[currentLink];
            
            string textToCopy = linkInfo.GetLinkText();
            string copiedTextWithoutTags = Regex.Replace(textToCopy, @"<.*?>", string.Empty);
            GUIUtility.systemCopyBuffer = copiedTextWithoutTags;
            
            string markedText = MarkSelection(linkInfo);
            
            _textField.SetText(markedText);
        }

        public string MarkSelection(TMP_LinkInfo linkInfo)
        {
            string linkId = linkInfo.GetLinkID();
            
            int startIndex = _textField.text.IndexOf($"<link=\"{linkId}\">");
            int extraTagCharacters = $"<link=\"{linkId}\">".Length;

            int linkTextFirstCharacterIndex = startIndex + extraTagCharacters;
            int linkTextLastCharacterIndex = linkTextFirstCharacterIndex + linkInfo.linkTextLength;
            
            var markedText = new StringBuilder(_textField.text);

            if (markedText.Length >= linkTextFirstCharacterIndex &&
                markedText.Length >= linkTextLastCharacterIndex + StartMarkTag.Length)
            {
                markedText.Insert(linkTextFirstCharacterIndex, StartMarkTag);
                markedText.Insert(linkTextLastCharacterIndex + StartMarkTag.Length, _endMarkTag);
            }
            
            _highlighted = true;
            return markedText.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Vector3 mousePos = new Vector3(eventData.position.x, eventData.position.y, 0);
            
            int currentLink = TMP_TextUtilities.FindNearestLink(_textField, mousePos, _cameraToUse);
            
            if (currentLink == -1) 
                return;
            
            if (_highlighted && _lastCheckedLink != currentLink)
            {
                _textField.SetText(_originalText);
                CopyToClipboard(currentLink);
                _lastCheckedLink = currentLink;
            }
            else if (_highlighted)
            {
                _textField.SetText(_originalText);
                _highlighted = false;
            }
            else
            {
                CopyToClipboard(currentLink);
                _lastCheckedLink = currentLink;
                _highlighted = true;
            }
        }
    }
}