using System;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace ChristinaCreatesGames.Typography.FontSizes
{
public class FontSizeCustomizer : MonoBehaviour
{
    private TMP_StyleSheet _styleSheet => TMP_Settings.defaultStyleSheet;

    [SerializeField] private string styleName;

    public static Action<string> UpdatedTheTextStyle;
    
    
    public void ChangeFontSize(float fontSize)
    {
        TMP_Style style = _styleSheet.GetStyle(styleName);
        
        if (style == null)
        {
            Debug.LogError($"No style with name {styleName} found in the default style sheet. Check for spelling?");
            return;
        }

        Regex regex = new Regex(@"<size=\d+>");

        string modifiedOpeningDefinition = regex.Replace(style.styleOpeningDefinition, $"<size={fontSize}>");  
        
        FieldInfo openingDefinitionField = typeof(TMP_Style).GetField("m_OpeningDefinition", BindingFlags.NonPublic | BindingFlags.Instance);
        
        if (openingDefinitionField != null) 
            openingDefinitionField.SetValue(style, modifiedOpeningDefinition);

        style.RefreshStyle();
        
        UpdatedTheTextStyle?.Invoke(styleName);
    }
}
}
