using TMPro;
using UnityEngine;

namespace ChristinaCreatesGames.Typography.ShowButtonPrompt
{
    [RequireComponent(typeof(TMP_Text))]
    public class SetTextToTextBox : MonoBehaviour
    {
        [TextArea(2,3)]
        [SerializeField] private string message = "Press BUTTONPROMPT to interact.";
        
        [Header("Setup for sprites")]
        [SerializeField] private ListOfTmpSpriteAssets listOfTmpSpriteAssets;
        [SerializeField] private DeviceType deviceType;
        
        private PlayerInput _playerInput;
        private TMP_Text _textBox;

        private void Awake()
        {
            _playerInput = new PlayerInput();
            _textBox = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            SetText();
        }

        [ContextMenu("Set Text")]
        private void SetText()
        {
            if ((int)deviceType > listOfTmpSpriteAssets.SpriteAssets.Count-1)
            {
                Debug.Log($"Missing Sprite Asset for {deviceType}");
                return;
            }
            
            _textBox.text = CompleteTextWithButtonPromptSprite.ReadAndReplaceBinding(
                message, 
                _playerInput.Player.Interact.bindings[(int)deviceType], 
                listOfTmpSpriteAssets.SpriteAssets[(int)deviceType]
                );
        }

        private enum DeviceType
        {
            Keyboard = 0,
            Gamepad = 1
        }
    }
}
