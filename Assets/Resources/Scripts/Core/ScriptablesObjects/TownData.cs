using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GameCore.Core
{
    [System.Serializable]
    [HideLabel]
    [CreateAssetMenu(fileName = "New Town", menuName = "GameCore/Create/Town", order = 1)]
   
    public class TownData : ScriptableObject
    {
        public string m_TownName = "New Town";
        public Sprite m_Background;
        public List<TownBuildingProperty> m_TownBuildingProperties = new List<TownBuildingProperty>();
        public GameObject m_Platform;
    }
}