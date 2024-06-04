using UnityEngine;
using UnityEngine.EventSystems;

public class IsOverUiCheck : MonoBehaviour
{
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            Debug.Log("Mouse is over a UI element");
    }
}
