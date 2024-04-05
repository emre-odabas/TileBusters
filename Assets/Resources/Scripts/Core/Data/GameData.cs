using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameCore.Core
{
    [System.Serializable]
    public class GameData
    {
        public SettingsData m_SettingsData = new SettingsData();
        //public EconomyData m_EconomyData = new EconomyData();
        //public InventoryData m_InventoryData = new InventoryData();

        public int m_PlayerLevel = 0;
        public string m_SeletedItem;
        public int m_RandomUnlockedItemCount = 0;
    }
    [System.Serializable]
    public class SettingsData
    {
        public bool m_IsSoundON = true;
        public bool m_IsHapticON = true;
    }
    //[System.Serializable]
    //public class EconomyData
    //{        
    //    public int m_Coin;
    //    public int m_Key;
    //    public EconomyData()
    //    {
    //        m_Coin = 0;
    //        m_Key = 0;
    //    }
    //}
    //[System.Serializable]
    //public class InventoryData
    //{
    //    public List<CoreItem> m_UnlockedItems;
    //    public CoreItem m_SelectedSkin;
    //    public InventoryData()
    //    {
    //        m_UnlockedItems = new List<CoreItem>();
    //        //m_UnlockedItems.Add(ItemDB.Instance.m_List[0]);
    //        //m_SelectedSkin = ItemDB.Instance.m_List[0];
    //    }
    //    public void UnlockItem(CoreItem _skin)
    //    {
    //        if (!m_UnlockedItems.Exists(x=> x.m_Name == _skin.name && x.m_Category == _skin.m_Category))
    //        {
    //            m_UnlockedItems.Add(_skin);
    //        }   
    //    }
    //}
}
