using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

[System.Serializable]
public class TownBuildingUpgradeProperty
{
    public CurrencyType m_CostType = CurrencyType.Hammer;
    public int m_CostAmount = 2;
    [PreviewField(ObjectFieldAlignment.Left, Height = 100)] public Sprite m_Sprite;
    public bool m_GiveReward = true;
    [ShowIf("m_GiveReward")] public CurrencyType m_RewardType = CurrencyType.Star;
    [ShowIf("m_GiveReward")] public int m_RewardAmount = 1;
}

