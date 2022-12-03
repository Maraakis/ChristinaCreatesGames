using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Link System Tutorial 1: Showing a tooltip by clicking on text in a Text Mesh Pro textbox
// Video tutorial: https://youtu.be/N6vYyCahLr8

// Hi! I'm Christina, this is my implementation of how to make text in a Text Mesh Pro textbox clickable. 
// This system uses three scripts:
// TooltipHandler - Controls the tooltip and I place it on my Canvas gameobject which contains the tooltip to control
// LinkHandlerForTMPText - Sits on the same gameobject as your Text Mesh Pro component, but could live anywhere else, too. It checks if you have clicked on a link in your textbox.
// TooltipInfos - a simple struct which contains relevant infos for the link. In this implementation, it uses an image to display, in the implementation on showing a tooltip while hovering, it contains different contents.
// Please watch the video if you have questions about how to set this up :)
// Hope you'll enjoy my system!
// - Christina

namespace ChristinaCreatesGames.Typography.TooltipForTMPbyClick
{
    public class TooltipHandler : MonoBehaviour
    {
        [SerializeField] private List<TooltipInfos> tooltipContentList;

        [SerializeField] private GameObject tooltipContainer;
        private TMP_Text _tooltipDescriptionTMP;
        [SerializeField] private Image iconDisplay;
        
        private void Awake()
        {
            _tooltipDescriptionTMP = tooltipContainer.GetComponentInChildren<TMP_Text>();
        }

        private void OnEnable()
        {
            LinkHandlerForTMPText.OnClickedOnLinkEvent += GetTooltipInfo;
        }

        private void OnDisable()
        {
            LinkHandlerForTMPText.OnClickedOnLinkEvent -= GetTooltipInfo;
        }

        private void GetTooltipInfo(string keyword)
        {
            foreach (var entry in tooltipContentList)
            {
                if (entry.Keyword == keyword)
                {
                    if (!tooltipContainer.activeInHierarchy)
                    {
                        tooltipContainer.SetActive(true);
                    }
                    
                    _tooltipDescriptionTMP.text = entry.Keyword;
                    iconDisplay.sprite = entry.Image;
                    return;
                }
            }
            
            Debug.Log($"Keyword: {keyword} not found");
        }

        public void CloseTooltip()
        {
            if (tooltipContainer.gameObject.activeInHierarchy)
            {
                tooltipContainer.gameObject.SetActive(false);
            }
        }
    }
}