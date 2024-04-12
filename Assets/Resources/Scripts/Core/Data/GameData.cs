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
        public SettingsLocalData m_SettingsLocalData = new SettingsLocalData();
        public TownLocalData m_TownLocalData= new TownLocalData();

        //public int m_PlayerTownLevel = 0;
    }

    [System.Serializable]
    public class SettingsLocalData
    {
        public bool m_IsSoundON = true;
        public bool m_IsHapticON = true;
    }

    [System.Serializable]
    public class TownLocalData
    {
        public int m_TownLevel = 0;
        [ShowInInspector] public Dictionary<string, int> m_TownBuildingLevels = new Dictionary<string, int>();
    }
}
