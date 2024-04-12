using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

[System.Serializable]
public class TownBuildingUpgradeProperty
{
    public int m_RequiredHammer = 2;
    [PreviewField] public Sprite m_Sprite;
}

