using UnityEngine;
using UnityEngine.EventSystems;

namespace Christina.CustomCursor
{
    public class ChangeCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private ModeOfCursor modeOfCursor;


        public void OnPointerEnter(PointerEventData eventData)
        {
            CursorControllerComplex.Instance.SetToMode(modeOfCursor);
        }

        public void OnPointerExit(PointerEventData  eventData)
        {
            CursorControllerComplex.Instance.SetToMode(ModeOfCursor.Default);
        }
    }
}