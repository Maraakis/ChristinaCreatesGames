using UnityEngine;

namespace Christina.CustomCursor
{
    public class CursorControllerComplex : MonoBehaviour
    {
        public static CursorControllerComplex Instance { get; private set; }

        [SerializeField] private Texture2D cursorTextureDefault;
        [SerializeField] private Texture2D cursorTextureQuestion;
        [SerializeField] private Texture2D cursorTextureExclamation;

        [SerializeField] private Vector2 clickPosition = Vector2.zero;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            Cursor.SetCursor(cursorTextureDefault, clickPosition, CursorMode.Auto);
        }

        public void SetToMode(ModeOfCursor modeOfCursor)
        {
            switch (modeOfCursor)
            {
                case ModeOfCursor.Default:
                    Cursor.SetCursor(cursorTextureDefault, clickPosition, CursorMode.Auto);
                    break;
                case ModeOfCursor.Question:
                    Cursor.SetCursor(cursorTextureQuestion, clickPosition, CursorMode.Auto);
                    break;
                case ModeOfCursor.Exclamation:
                    Cursor.SetCursor(cursorTextureExclamation, clickPosition, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(cursorTextureDefault, clickPosition, CursorMode.Auto);
                    break;
            }
        }
    }


    public enum ModeOfCursor
    {
        Default,
        Question,
        Exclamation
    }
}
