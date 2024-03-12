using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ChristinaCreatesGames.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class HighlightCopyClick : MonoBehaviour, IPointerClickHandler
    {
        [Header("Highlight Settings")]
        [SerializeField] private Color highlightColor = Color.white;
        private string _hexValueOfColor;
        private string StartMarkTag => $"<mark=#" + _hexValueOfColor + ">";
        private string _endMarkTag = "</mark>";
        
        private TMP_Text _textField;
        private string _originalText;

        private bool _highlighted = false;
        
        private void Awake()
        {
            _textField = GetComponent<TMP_Text>();
            _originalText = _textField.text;
        }
        
        public void SetNewText(string newText)
        {
            _textField.SetText(newText);
            _originalText = newText;
        }

        public void CopyToClipboard()
        {
            _textField.SetText(_originalText);
            MarkSelection();
            
            string textToCopy = _textField.text;
            string copiedTextWithoutTags = Regex.Replace(textToCopy, @"<.*?>", string.Empty);
            GUIUtility.systemCopyBuffer = copiedTextWithoutTags;
            Debug.Log("Copied to clipboard: " + _textField.text);
        }

        public void MarkSelection()
        {
            _hexValueOfColor = ColorUtility.ToHtmlStringRGBA(highlightColor);
            _textField.SetText("<mark=#F4AE1180>" + _textField.text + "</mark>");
            _textField.SetText(StartMarkTag + _textField.text + _endMarkTag);
            _highlighted = true;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_highlighted)
            {
                _textField.SetText(_originalText);
                _highlighted = false;
            }
            else
                CopyToClipboard();
        }
    }
}
