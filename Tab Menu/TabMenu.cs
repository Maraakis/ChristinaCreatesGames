using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ChristinaCreatesGames.UI
{
    public class TabMenu : MonoBehaviour
    {
        [Header("Current Index")]
        [SerializeField] private int pageIndex = 0;
        
        [Header("Components")]
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private List<Toggle> tabs = new List<Toggle>();
        [SerializeField] private List<CanvasGroup> pages = new List<CanvasGroup>();
        
        [Header("Event to call")]
        public UnityEvent<int> OnPageIndexChanged;

        private void Initialize()
        {
            toggleGroup = GetComponentInChildren<ToggleGroup>();
            
            tabs.Clear();
            pages.Clear();
            
            tabs.AddRange(GetComponentsInChildren<Toggle>());
            pages.AddRange(GetComponentsInChildren<CanvasGroup>());
        }

        private void Reset()
        {
            Initialize();
        }

        private void OnValidate()
        {
            Initialize();
            OpenPage(pageIndex);
            tabs[pageIndex].SetIsOnWithoutNotify(true);
        }
        
        private void Awake()
        {
            foreach (var toggle in tabs)
            {
                toggle.onValueChanged.AddListener(CheckForTab);
                toggle.group = toggleGroup;
            }
        }

        private void OnDestroy()
        {
            foreach (var toggle in tabs)
            {
                toggle.onValueChanged.RemoveListener(CheckForTab);
            }
        }
        
        private void CheckForTab(bool value)
        {
            for (int i = 0; i < tabs.Count; i++)
            {
                if (!tabs[i].isOn) continue;
                pageIndex = i;
            }
            
            OpenPage(pageIndex);
        }
        
        private void OpenPage(int index)
        {
            EnsureIndexIsInRange(index);
            
            for (int i = 0; i < pages.Count; i++)
            {
                bool isActivePage = (i == pageIndex);
                
                pages[i].alpha = isActivePage ? 1.0f : 0.0f;
                pages[i].interactable = isActivePage;
                pages[i].blocksRaycasts = isActivePage;
            }
            
            if (Application.isPlaying)
                OnPageIndexChanged?.Invoke(pageIndex);
        }

        private void EnsureIndexIsInRange(int index)
        {
            if (tabs.Count == 0 || pages.Count == 0)
            {
                Debug.Log("Forgot to setup Tabs or Pages");
                return;
            }
            
            pageIndex = Mathf.Clamp(index, 0, pages.Count - 1);
        }
        
        public void JumpToPage(int page)
        {
            EnsureIndexIsInRange(page);
            
            tabs[pageIndex].isOn = true;
        }
    }
}
