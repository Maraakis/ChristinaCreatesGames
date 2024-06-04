using System.Collections;
using System.IO;
using Sirenix.OdinInspector; // If you have Odin Inspector
using UnityEngine;

namespace ChristinaCreatesGames.Systems.SimpleScreenshotTaker
{
    public class TakeScreenshot : MonoBehaviour
    {
        private Coroutine _photoCoroutine;
        
        [ContextMenu("Take Screenshot")] // If you do not have Odin Inspector
        [Button] // If you have Odin Inspector
        private void TakesScreenshot()
        {
            string directory = Application.persistentDataPath;
            string screenshotName = "Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
            string screenshotFullPath = Path.Combine(directory, screenshotName);
        
            // Saves the screenshot to the specified directory
            ScreenCapture.CaptureScreenshot(screenshotFullPath);
            
            Debug.Log(screenshotName);
        }
    
        [ContextMenu("Take Screenshot Without UI")] // If you do not have Odin Inspector
        [Button] // If you have Odin Inspector
        private void TakeScreenshotWithoutUi()
        {
            if (_photoCoroutine != null)
                StopCoroutine(_photoCoroutine);
            
            _photoCoroutine = StartCoroutine(TakeScreenshotNoUi());
        }
    
        private IEnumerator TakeScreenshotNoUi()
        {
            string directory = Application.persistentDataPath;
            string screenshotName = "Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
            string screenshotFullPath = Path.Combine(directory, screenshotName);
        
            // Save initial list of all rendered layers (culling mask)
            int oldCullingMask = Camera.main.cullingMask;
        
            // Remove UI from culling mask
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("UI"));
            
            ScreenCapture.CaptureScreenshot(screenshotFullPath);
        
            // Wait until the end of the frame, then restore mask
            yield return new WaitForEndOfFrame();
            Camera.main.cullingMask = oldCullingMask;
            
            Debug.Log(screenshotName);
        }
    }
}