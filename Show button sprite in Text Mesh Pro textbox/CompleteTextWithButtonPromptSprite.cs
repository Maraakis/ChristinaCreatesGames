using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Christina.Typography.ShowButtonPrompt
{
    public static class CompleteTextWithButtonPromptSprite
    {
        public static string ReadAndReplaceBinding(string textToDisplay, InputBinding actionNeeded, TMP_SpriteAsset spriteAsset)
        {
            string stringButtonName = actionNeeded.ToString();
            Debug.Log("Original name of InputBinding: " + stringButtonName);
            stringButtonName = RenameInput(stringButtonName);
            Debug.Log("Name we can actually work with: " + stringButtonName);

            textToDisplay = textToDisplay.Replace(
                "BUTTONPROMPT", 
                $"<sprite=\"{spriteAsset.name}\" name=\"{stringButtonName}\">");
        
            return textToDisplay;
        }

        private static string RenameInput(string stringButtonName)
        {
            stringButtonName = stringButtonName.Replace(
                "Interact:", String.Empty);

            stringButtonName = stringButtonName.Replace(
                "<Keyboard>/", "Keyboard_");
            stringButtonName = stringButtonName.Replace(
                "<Gamepad>/", "Gamepad_");

            return stringButtonName;
        }
    }
}
