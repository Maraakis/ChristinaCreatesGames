using System;
using UnityEngine;

namespace ChristinaCreatesGames.Typography.TooltipForTMP
{
    [Serializable]
    public struct TooltipInfos
    {
        public string Keyword;
        [TextArea]
        public string Description;
        public Sprite Image;
    }
}