using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[System.Serializable]
public class TownBuildingProperty
{
    public string m_Id = "building_";
    public List<TownBuildingUpgradeProperty> m_UpgradeList = new List<TownBuildingUpgradeProperty>();
}

