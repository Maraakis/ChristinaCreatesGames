using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ChristinaCreatesGames.UI
{
    public class HighlightCopyFreeform : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
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
        private string _originalTextWithoutTags;
        private string _selectedText;
        private int _startPos;
        private bool _mouseDown;
        
        private void Awake()
        {
            _textField = GetComponent<TMP_Text>();
            _canvas = GetComponentInParent<Canvas>();
            _originalText = _textField.text;
            _originalTextWithoutTags = Regex.Replace(_originalText, @"<.*?>", string.Empty);

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
            _originalTextWithoutTags = Regex.Replace(newText, @"<.*?>", string.Empty);
        }
        

        public void OnPointerDown(PointerEventData eventData)
        {
            _textField.SetText(_originalTextWithoutTags);

            Vector3 mousePos = new Vector3(eventData.position.x, eventData.position.y, 0);

            _startPos = TMP_TextUtilities.FindNearestCharacter(_textField, mousePos, _cameraToUse, true);
            _mouseDown = true;
        }
        
        public void OnPointerMove(PointerEventData eventData)
        {
            if (!_mouseDown)
                return;

            Vector3 mousePos = new Vector3(eventData.position.x, eventData.position.y, 0);
            var currentPos = TMP_TextUtilities.FindNearestCharacter(_textField, mousePos, _cameraToUse, false);

            if (currentPos != -1 && currentPos <= _startPos)
                return;
            
            int length = currentPos - _startPos + 2;

            if (_startPos + length > _originalTextWithoutTags.Length)
                length = _originalTextWithoutTags.Length - _startPos;

            _selectedText = _originalTextWithoutTags.Substring(_startPos, length);

            var markedText = MarkSelection(length);

            _textField.SetText(markedText);
        }

        private string MarkSelection(int length)
        {
            var markedText = new StringBuilder(_originalTextWithoutTags);
            markedText.Insert(_startPos, StartMarkTag);

            if (_startPos + length + StartMarkTag.Length <= markedText.Length)
                markedText.Insert(_startPos + length + StartMarkTag.Length, _endMarkTag);
            else
            {
                int maxLengthOfMarkedText = markedText.Length - _startPos;
                markedText.Insert(maxLengthOfMarkedText, _endMarkTag);
            }

            return markedText.ToString();   
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_mouseDown == false)
                return;
            
            _mouseDown = false;

            CopyToClipboard();
        }
        
        public void CopyToClipboard()
        {
            if (!string.IsNullOrEmpty(_selectedText) && (_selectedText[^1] == ' ' || _selectedText[0] == ' '))
                _selectedText = _selectedText.Trim(' ');
            
            GUIUtility.systemCopyBuffer = _selectedText;
            
            _textField.SetText(_originalText);
        }
    }
}