using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ShowNextPageButton : MonoBehaviour
{
    [SerializeField] private Image _buttonImage;
    
    [SerializeField] private TMP_Text _textBox;
    
    private void Update()
    {
        if (_textBox.overflowMode == TextOverflowModes.Page && 
            _textBox.textInfo.pageCount > 1)
        {
            if (_buttonImage.enabled == false)
                _buttonImage.enabled = true;
        }
        else
        {
            if (_buttonImage.enabled)
                _buttonImage.enabled = false;
        }
    }
}
