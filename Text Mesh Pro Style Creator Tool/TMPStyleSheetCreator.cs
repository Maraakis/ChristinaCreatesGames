using System;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;

namespace ChristinaCreatesGames.Tools.Typography
{
    public class TMPStyleSheetCreator : MonoBehaviour
    {
        [SerializeField] private TMP_Text Textbox;
        [TextArea(3,5)]
        [SerializeField] private string OpeningTags;
        [TextArea(3,5)]
        [SerializeField] private string ClosingTags;

        public TextWeight TextWeight;
        
        [ContextMenu("Read Values from TMP")]
        public void ReadValuesFromTMP()
        {
            StringBuilder openSB = new StringBuilder();
            StringBuilder closeSB = new StringBuilder();

            if ((Textbox.fontStyle & FontStyles.Bold) != 0)
            {
                openSB.Append("<b>");
                closeSB.Insert(0, "</b>");
            }
            
           if ((Textbox.fontStyle & FontStyles.Italic) != 0)
           {
               openSB.Append("<i>");
               closeSB.Insert(0, "</i>");
           }

           if ((Textbox.fontStyle & FontStyles.UpperCase) != 0)
           {
               openSB.Append("<uppercase>");
               closeSB.Insert(0, "</uppercase>");
           }

           if ((Textbox.fontStyle & FontStyles.LowerCase) != 0)
           {
               openSB.Append("<lowercase>");
               closeSB.Insert(0, "</lowercase>");
           }

           string fontAsset = Textbox.font.ToString();
           fontAsset = fontAsset.Replace(" (TMPro.TMP_FontAsset)", String.Empty);
           
           openSB.Append($"<font=\"{fontAsset}\">");
           closeSB.Insert(0, "</font>");

           float textSize = Textbox.fontSize;
           openSB.Append($"<size={textSize}pt>");
           closeSB.Insert(0, "</size>");

           Color textColor = Textbox.color;
           string textColorRGB = ColorUtility.ToHtmlStringRGB(textColor);
           openSB.Append($"<color=#{textColorRGB}>");
           closeSB.Insert(0, "</color>");

           float characterSpacing = Textbox.characterSpacing;
           characterSpacing /= 100;
           string cSpacing = characterSpacing.ToString("N3", CultureInfo.InvariantCulture);
           
           if (characterSpacing != 0)
           {
               openSB.Append($"<cspace={cSpacing}em>");
               closeSB.Insert(0, "</cspace>");
           }

           float lineSpacing = Textbox.lineSpacing;
           lineSpacing /= 100;
           string lSpacing = lineSpacing.ToString("N3", CultureInfo.InvariantCulture);
           
           if (lineSpacing != 0)
           {
               openSB.Append($"<line-height={lSpacing}em>");
               closeSB.Insert(0, "</line-height>");
           }
           
           if (TextWeight == TextWeight.Black)
           {
               openSB.Append($"<font-weight={"900"}>");
               closeSB.Insert(0, "</font-weight>");
           }
           else if (TextWeight == TextWeight.Thin)
           {
               openSB.Append($"<font-weight={"100"}>");
               closeSB.Insert(0, "</font-weight>");
           }
            
            OpeningTags = openSB.ToString();
            ClosingTags = closeSB.ToString();
        }
    }

    public enum TextWeight
    {
        Regular,
        Thin,
        Black
        // Add to this as you need!
    }
}
