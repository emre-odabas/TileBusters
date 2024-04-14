using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace GameCore.Core
{
    [System.Serializable]
    [HideLabel]
    [CreateAssetMenu(fileName = "CurrencyData", menuName = "GameCore/Create/CurrencyData", order = 1)]
   
    public class DisplayData : ScriptableObject
    {
        public List<DisplayProperty> m_DisplayPropertyList = new List<DisplayProperty>();
    }
}