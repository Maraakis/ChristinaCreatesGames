using UnityEngine;

namespace ChristinaCreatesGames.UI
{
    public class DoubleClickTester : MonoBehaviour
    {
        // Attach this script to any Gameobject in your scene
        // and hook up these methods to the respective UnityEvents
        // of the DoubleClickButton component.
        
        public void OnSingleClick()
        {
            Debug.Log("Clicked one time!");
        }

        public void OnDoubleClick()
        {
            Debug.Log("Double clicked!");
        }
        
        public void OnHoverEnter()
        {
            Debug.Log("Hovered over!");
        }

        public void OnHoverExit()
        {
            Debug.Log("Hovered out!");
        }
    }
}