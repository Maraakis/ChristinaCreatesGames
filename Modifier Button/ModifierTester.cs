using TMPro;
using UnityEngine;

namespace ChristinaCreatesGames.UI
{
    public class ModifierTester : MonoBehaviour
    {
        public TMP_Text textLabel; // Just create any kind of TextMesh Pro Text object in your scene and assign it here.

        public void OnClick()
        {
            Debug.Log("Taking one Iron.");
        }
        
        public void OnModifierAHeldDown()
        {
            textLabel.SetText("Split in half");
        }
        
        public void OnModifierBHeldDown()
        {
            textLabel.SetText("Take ten");
        }
        
        public void OnNoModifierHeldDown()
        {
            textLabel.SetText("Take one");
        }
        
        public void OnModifierAClick()
        {
            Debug.Log("Split Iron stack in half.");
        }
        
        public void OnModifierBClick()
        {
            Debug.Log("Taking ten Iron.");
        }
        
        public void OnBothHeldDown()
        {
            textLabel.SetText("Take all");
        }
        
        public void OnBothClick()
        {
            Debug.Log("Taking all Iron.");
        }
    }
}