using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using GameCore.Managers;
using System;
using GameCore.Core;
using GameCore.Controllers;
using System.Collections.Generic;
using TMPro;
using System.Linq;

namespace GameCore.Gameplay
{
    public class TownBuilding : MonoBehaviour
    {
        #region UTILITIES

        #endregion
        
        #region FIELDS

        //Parameters
        [FoldoutGroup("Parameters")] 
        [FoldoutGroup("Parameters"), SerializeField] public string m_Id = "building_";

        //Components
        [FoldoutGroup("Components")] 
        [FoldoutGroup("Components/Building"), SerializeField] private Image m_BuildingImage;
        [FoldoutGroup("Components/Button"), SerializeField] private Transform m_BtnTiersContainer;
        [FoldoutGroup("Components/Button"), SerializeField] private GameObject m_RefButtonUpgradeTier;
        [FoldoutGroup("Components/Button"), SerializeField] private Image m_ButtonCurrencyImage;
        [FoldoutGroup("Components/Button"), SerializeField] private TextMeshProUGUI m_ButtonCurrencyText;
        
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_ShowButtonFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_HideButtonFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_FirstUpgradeFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_UpgradeFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_MaxUpgradeFeedbacks;
        
        //Indicator
        [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private bool isMaxLevel = false;
        [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private int currentLevel = 0;
        [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private TownBuildingProperty townBuildingProperty;
        [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private TownBuildingLocalData townBuildinglocalData;
        
        //Privates
        private bool isBusy = false;
        private bool lastButtonState = true;
        private List<ButtonUpgradeTier> buttonUpgradeTierList = new List<ButtonUpgradeTier>();

        #endregion

        #region MONOBEHAVIOUR

        private void Update()
        {
            CheckButtonToggle();
        }

        #endregion

        #region CALLBACKS

        #endregion

        #region RETURN FUNCTIONS

        public bool IsMaxLevel()
        {
            return isMaxLevel;
        }

        private TownBuildingUpgradeProperty CurrentLevelProperty()
        {
            TownBuildingUpgradeProperty property = townBuildingProperty.m_UpgradeList[currentLevel];
            return property;
        }

        private TownBuildingUpgradeProperty NextLevelProperty()
        {
            int index = currentLevel + 1;
            if (index > townBuildingProperty.m_UpgradeList.Count - 1)
                return null;

            TownBuildingUpgradeProperty property = townBuildingProperty.m_UpgradeList[index];
            return property;
        }

        private bool IsAvailableCost()
        {
            TownBuildingUpgradeProperty nextUpgrade = NextLevelProperty();
            if (nextUpgrade != null)
            {
                bool availableCost = GameManager.Instance.m_CurrencyData.GetCurrentCurrencyValue(nextUpgrade.m_CostType) >= nextUpgrade.m_CostAmount;
                return availableCost;
            }

            return false;
        }

        #endregion

        #region FUNCTIONS

        public void Setup(TownBuildingLocalData localData)
        {
            townBuildinglocalData = localData;

            if (townBuildinglocalData == null) return;

            currentLevel = townBuildinglocalData.m_Level;
            townBuildingProperty = GameManager.Instance.m_CurrentTownData.GetBuildingData(m_Id);

            for (int i = 0; i < townBuildingProperty.m_UpgradeList.Count; i++)
            {
                GameObject buttonTierObj = Instantiate(m_RefButtonUpgradeTier, m_BtnTiersContainer);
                ButtonUpgradeTier buttonUpgradeTier = buttonTierObj.GetComponent<ButtonUpgradeTier>();
                buttonUpgradeTier.EmptyIt();

                buttonUpgradeTierList.Add(buttonUpgradeTier);
            }

            Customize();
        }

        private void Customize()
        {
            if (townBuildingProperty == null) return;

            isMaxLevel = currentLevel == townBuildingProperty.m_UpgradeList.Count - 1;

            //Image
            /*m_BuildingImage.enabled = currentLevel != -1;
            if (m_BuildingImage.enabled)
                m_BuildingImage.sprite = CurrentLevelProperty().m_Sprite;*/

            //m_BuildingImage.rectTransform.localScale = Vector3.one * Math.Sign(currentLevel);

            if (currentLevel == -1)
            {
                m_BuildingImage.rectTransform.localScale = Vector3.zero;
            }
            else
            {
                m_BuildingImage.sprite = CurrentLevelProperty().m_Sprite;
            }

            //Button
            for (int i = 0; i < buttonUpgradeTierList.Count; i++)
            {
                if (i <= currentLevel)
                    buttonUpgradeTierList[i].Fill();
                else
                    buttonUpgradeTierList[i].EmptyIt();
            }

            if(NextLevelProperty() != null)
            {
                m_ButtonCurrencyImage.sprite = GameManager.Instance.m_CurrencyData.GetCurrencyProperty(NextLevelProperty().m_CostType).m_Sprite;
                m_ButtonCurrencyText.text = NextLevelProperty().m_CostAmount.ToString();
            }
        }

        private void CheckButtonToggle()
        {
            bool value = (GameManager.Instance.m_State == GameManager.State.PlayingTown && !isMaxLevel);
            ButtonToggle(value);
        }

        private void ButtonToggle(bool isShow)
        {
            if (isShow == lastButtonState) return;

            lastButtonState = isShow;
            if (isShow)
            {
                m_ShowButtonFeedbacks.PlayFeedbacks();
            }
            else
            {
                m_HideButtonFeedbacks.PlayFeedbacks();
            }
        }

        public void Upgrade()
        {
            if (isBusy) return;
            if (isMaxLevel) return;
            if (!IsAvailableCost()) 
            {
                Debug.LogError("insufficient amount!!!!!!! Play Puzzle!");
                return;
            }

            PayAndGiveReward();
            isBusy = true;
            currentLevel = Mathf.Clamp(currentLevel + 1, 0, townBuildingProperty.m_UpgradeList.Count - 1);
            //Customize();

            //MMFeedbacks upgradeFeedbacks = isMaxLevel ? m_MaxUpgradeFeedbacks : m_UpgradeFeedbacks;
            MMFeedbacks upgradeFeedbacks;
            if (currentLevel == 0)
            {
                upgradeFeedbacks = m_FirstUpgradeFeedbacks;
            }
            else if (isMaxLevel)
            {
                upgradeFeedbacks = m_MaxUpgradeFeedbacks;
            }
            else
            {
                upgradeFeedbacks = m_UpgradeFeedbacks;
            }

            upgradeFeedbacks.PlayFeedbacks();

            Utilities.DelayedCall(1, () =>
            {
                Customize();
            });

            SaveLevel();

            /*Utilities.DelayedCall(upgradeFeedbacks.Feedbacks[0], () =>
            {
                m_isBusy = false;
            });*/
            isBusy = false;

            TownController.Instance.onBuildingUpgraded?.Invoke();
        }

        private void PayAndGiveReward()
        {
            TownBuildingUpgradeProperty nextUpgrade = NextLevelProperty();
            if (nextUpgrade != null)
            {
                //Pay Cost
                GameManager.Instance.DecreaseCurrency(nextUpgrade.m_CostType, nextUpgrade.m_CostAmount);

                //Give Reward
                if (nextUpgrade.m_GiveReward && nextUpgrade.m_RewardType != CurrencyType.None)
                {
                    GameManager.Instance.IncreaseCurrency(nextUpgrade.m_RewardType, nextUpgrade.m_RewardAmount);
                }
            }
        }

        public void SaveLevel()
        {
            if (townBuildinglocalData == null) return;

            townBuildinglocalData.m_Level = currentLevel;
            DataManager.Instance.SaveGameData();
        }

        #endregion
    }
}

