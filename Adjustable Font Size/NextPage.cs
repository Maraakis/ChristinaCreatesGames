using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace ChristinaCreatesGames.Typography.FontSizes
{
[RequireComponent(typeof(TMP_Text))]
public class NextPage : MonoBehaviour
{
    [SerializeField] private TMP_Text _textBox;

    [TextArea(5,10)][SerializeField] private string FirstChunkOfText;
    [TextArea(5,10)][SerializeField] private string SecondChunkOfText;
    
    private List<string> _textList => new List<string> {FirstChunkOfText, SecondChunkOfText};
    private int _textIndex = 0;
    private int _currentTextPages => _textBox.textInfo.pageCount;
    
    private int _currentPage => _textBox.pageToDisplay;


    private void Awake()
    {
        _textBox = GetComponent<TMP_Text>();
    }

    [ContextMenu("Display Next Page")]
    [Button]
    public void DisplayNextPage()
    {
        if (_currentPage < _currentTextPages)
        {
            _textBox.pageToDisplay++;
        }
        else
        {
            _textIndex++;
            if (_textIndex >= _textList.Count)
            {
                _textIndex = 0;
            }
            
            _textBox.text = _textList[_textIndex];
            _textBox.pageToDisplay = 1;
        }
    }
}
}
