using GameCore.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "TownDataList", menuName = "GameCore/Create/TownDataList")]
[System.Serializable]
public class TownDataList : SingletonScriptableObject<TownDataList>
{
    public List<TownData> m_List = new List<TownData>();
}
