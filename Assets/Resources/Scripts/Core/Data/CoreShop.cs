using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.GameFoundation;

namespace GameCore.Core
{
    [System.Serializable]
    public class CoreShop
    {
        [ReadOnly]
        public string m_StoreKey;
        [ReadOnly]
        public readonly List<CoreShopItem> m_Items = new List<CoreShopItem>();

        public List<CoreShopItem> GetUnlockedItems()
        {
            return m_Items.Where(x => x.GetAmount() > 0).ToList();
        }

        public List<CoreShopItem> GetLockedItems()
        {
            return m_Items.Where(x => x.GetAmount() == 0).ToList();
        }

        public CoreShopItem GetItem(BaseTransaction _transaction)
        {
            return m_Items.FirstOrDefault(x => x.m_Transaction == _transaction);
        }
        public CoreShopItem GetItem(InventoryItemDefinition _definition)
        {
            return m_Items.FirstOrDefault(x => x.m_InventoryDefinition == _definition);
        }
    } 
}
