using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ChristinaCreatesGames.Typography.ShowButtonPrompt
{
    [CreateAssetMenu(fileName = "List of Sprite Assets", 
        menuName = "List of Sprite Assets", order = 0)]
    public class ListOfTmpSpriteAssets : ScriptableObject
    {
        public List<TMP_SpriteAsset> SpriteAssets;
    }
}
