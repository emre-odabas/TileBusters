using GameCore.Core;
using GameCore.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "TownDataList", menuName = "GameCore/Create/TownDataList")]
[System.Serializable]
public class TownDataList : SingletonScriptableObject<TownDataList>
{
    public List<TownData> m_List = new List<TownData>();

    public TownData GetCurrentTownData()
    {
        int index = DataManager.Instance.m_GameData.m_TownLocalData.m_TownLevel;
        return m_List[index];
    }

    public int GetCurrentTownLevel()
    {
        int index = DataManager.Instance.m_GameData.m_TownLocalData.m_TownLevel;
        return index + 1;
    }
}
