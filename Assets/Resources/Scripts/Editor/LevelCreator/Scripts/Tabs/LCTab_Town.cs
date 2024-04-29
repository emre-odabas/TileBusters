using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Hyperlab.Gameplay.Editor
{
    public partial class LCTab_Town
    {
        private string errorNull = "Boş bırakılamaz!";
        
        //CONSRUCTOR
        public LCTab_Town()
        {
            //common tab
            m_TownName = "New Town";
            m_basePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(basePrefabPath + "/" + "_BaseTown.prefab");

            //building upgrades tab
            for (int i = 0; i < 5; i++)
            {
                TownBuildingProperty property = new TownBuildingProperty();
                property.m_Id = "building_" + (i + 1);

                for (int j = 0; j < 3; j++)
                {
                    TownBuildingUpgradeProperty upgradeProperty = new TownBuildingUpgradeProperty();
                    upgradeProperty.m_CostType = CurrencyType.Hammer;
                    upgradeProperty.m_CostAmount = 2;
                    upgradeProperty.m_Sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/_GameAssets/Sprites/Towns/Town_{(i + 1)}/Buildings/Building_{(i + 1)}/building_{(i + 1)}_level_{(j + 1)}.png");
                    upgradeProperty.m_GiveReward = true;
                    upgradeProperty.m_RewardType = CurrencyType.Star;
                    upgradeProperty.m_RewardAmount = 1;

                    property.m_UpgradeList.Add(upgradeProperty);
                }

                m_TownBuildingProperties.Add(property);
            }
        }
        
        [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
        private void CeateTown()
        {
            SetDatas();
            
            void SetDatas()
            {
                
                Debug.Log("Asset oluşturma başarılı.");
            }
        }
    }

    #region COMMON TAB

    public partial class LCTab_Town
    {
        private string commonTabName = "New Town Common Settings";
        private string basePrefabPath = "Assets/_GameAssets/Prefabs/Towns/Levels";

        [TabGroup("tab_common", "$commonTabName"/*, SdfIconType.GearFill, TextColor = "green"*/)]
        [Required("$errorNull")]
        [TabGroup("tab_common", "$commonTabName")] public string m_TownName;
        [Required("$errorNull")]
        [TabGroup("tab_common", "$commonTabName"), PreviewField(ObjectFieldAlignment.Left, Height = 100)] public Sprite m_BackgroundImage;
        [Required("$errorNull")]
        

        [Space(15)]
        [Header("References")]
        [TabGroup("tab_common", "$commonTabName")] public GameObject m_basePrefab;
    }

    #endregion

    #region BUILDING UPGRADES TAB

    public partial class LCTab_Town
    {
        private string buildingUpgradesTabName = "Town Building Upgrade Settings";

        [TabGroup("tab_buildingUpgrades", "$buildingUpgradesTabName"/*, SdfIconType.GearFill, TextColor = "green"*/)]
        [TableList(AlwaysExpanded = true, NumberOfItemsPerPage = 25, ShowPaging = true, ShowIndexLabels = true)]
        //[Required("$errorNull")]
        public List<TownBuildingProperty> m_TownBuildingProperties = new List<TownBuildingProperty>();

        public void CreateWeaponCollectible()
        {

        }

    }

    #endregion
}

