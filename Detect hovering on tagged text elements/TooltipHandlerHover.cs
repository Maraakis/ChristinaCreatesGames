using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ChristinaCreatesGames.Typography.TooltipForTMP
{
    public class TooltipHandlerHover : MonoBehaviour
    {
        [SerializeField] private List<TooltipInfos> tooltipContentList;

        [SerializeField] private GameObject tooltipContainer;
        private TMP_Text _tooltipDescriptionTMP;
        
        private void Awake()
        {
            _tooltipDescriptionTMP = tooltipContainer.GetComponentInChildren<TMP_Text>();
        }

        private void OnEnable()
        {
            LinkHandlerForTMPTextHover.OnHoverOnLinkEvent += GetTooltipInfo;
            LinkHandlerForTMPTextHover.OnCloseTooltipEvent += CloseTooltip;
        }

        private void OnDisable()
        {
            LinkHandlerForTMPTextHover.OnHoverOnLinkEvent -= GetTooltipInfo;
            LinkHandlerForTMPTextHover.OnCloseTooltipEvent -= CloseTooltip;
        }

        private void GetTooltipInfo(string keyword, Vector3 mousePos)
        {
            foreach (var entry in tooltipContentList)
            {
                if (entry.Keyword == keyword)
                {
                    if (!tooltipContainer.gameObject.activeInHierarchy)
                    {
                        tooltipContainer.transform.position = mousePos +  new Vector3(0, 350, 0);
                        tooltipContainer.gameObject.SetActive(true);
                    }
                    
                    _tooltipDescriptionTMP.text = entry.Description;
                    return;
                }
            }
            
            Debug.Log($"Keyword: {keyword} not found");
        }

        public void CloseTooltip()
        {
            if (tooltipContainer.gameObject.activeInHierarchy)
                tooltipContainer.SetActive(false);
        }
    }
}