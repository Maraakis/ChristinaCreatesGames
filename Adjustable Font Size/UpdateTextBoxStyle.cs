using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class UpdateTextBoxStyle : MonoBehaviour
{
    private TMP_Text _textBox;

    private void OnEnable()
    {
        FontSizeCustomizer.UpdatedTheTextStyle += UpdateTextStyle;
    }
    
    private void OnDisable()
    {
        FontSizeCustomizer.UpdatedTheTextStyle -= UpdateTextStyle;
    }

    private void Awake()
    {
        _textBox = GetComponent<TMP_Text>();
    }
    
    private void UpdateTextStyle(string styleName)
    {
        if (_textBox.textStyle.name != styleName)
            return;
        
        _textBox.textStyle = TMP_Settings.defaultStyleSheet.GetStyle(styleName);
        
        int lastPage = _textBox.textInfo.pageCount - 1;
        
        if (_textBox.pageToDisplay > lastPage)
            _textBox.pageToDisplay = lastPage;
        
        Debug.Log($"Text needs {_textBox.textInfo.pageCount} pages to display. Current page is {_textBox.pageToDisplay}");
    }
}
