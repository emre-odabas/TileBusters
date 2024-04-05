using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Core.UI;
using GameCore.Core;

using GameCore.Managers;
using TMPro;
using UnityEngine.Events;
using UnityEngine.GameFoundation;

namespace GameCore.UI
{
    public class CoinPrizeBoxItem : CorePrizeBoxItem
    {
        public TextMeshProUGUI m_CoinText;
        public override void Setup(GameObject _unlockFX, Transform _VFXContainer, InventoryItemDefinition _itemDefinition)
        {
            base.Setup(_unlockFX, _VFXContainer, _itemDefinition);
            m_CoinText.text = m_Transaction.rewards.GetCurrencyExchange(0).amount.ToString();
        }

        public override void Unlock(UnityAction _callback = null)
        {
            base.Unlock(()=>OnCoinUnlock());
        }

        void OnCoinUnlock()
        {
            
        }
    } 
}