using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

[System.Serializable]
public class TownBuildingUpgradeProperty
{
    public UpgradeCurrency m_UpgradeCurrency = UpgradeCurrency.Hammer;
    public int m_RequiredAmount = 2;
    [PreviewField] public Sprite m_Sprite;
}

