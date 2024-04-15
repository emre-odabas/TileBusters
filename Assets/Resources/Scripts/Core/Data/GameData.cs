using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

namespace GameCore.Core
{
    [System.Serializable]
    public class GameData
    {
        public TownLocalData m_TownLocalData= new TownLocalData();
        public SettingsLocalData m_SettingsLocalData = new SettingsLocalData();
    }

    [System.Serializable]
    public class SettingsLocalData
    {
        public bool m_IsSoundON = true;
        public bool m_IsHapticON = true;
    }

    #region Town

    [System.Serializable]
    public class TownBuildingLocalData
    {
        public string m_Id = "building_";
        public int m_Level = -1;
    }

    [System.Serializable]
    public class TownLocalData
    {
        public int m_TownLevel = 0;
        public List<TownBuildingLocalData> m_TownBuildingLevels = new List<TownBuildingLocalData>();
    }

    #endregion


}
