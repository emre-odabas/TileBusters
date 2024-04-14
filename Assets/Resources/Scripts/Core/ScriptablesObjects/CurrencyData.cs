using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace GameCore.Core
{
    [System.Serializable]
    [HideLabel]
    [CreateAssetMenu(fileName = "CurrencyData", menuName = "GameCore/Create/CurrencyData", order = 1)]
   
    public class CurrencyData : ScriptableObject
    {
        public List<CurrencyProperty> m_CurrencyPropertyList = new List<CurrencyProperty>();

        #region RECALL FUNCTIONS

        public CurrencyProperty GetCurrencyProperty(CurrencyType currencyType)
        {
            CurrencyProperty data = m_CurrencyPropertyList.FirstOrDefault(c => c.m_CurrencyType == currencyType);
            if (data == null)
                Logger.LogError("CurrencyData", "Could not find currency with ID: " + currencyType);
                
            return data;
        }

        #endregion
    }
}