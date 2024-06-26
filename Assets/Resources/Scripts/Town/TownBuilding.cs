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
using AssetKits.ParticleImage;
using GameCore.UI;

namespace GameCore.Gameplay
{
    public class TownBuilding : MonoBehaviour
    {
        public enum State
        {
            Show,
            Hide
        }
        
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
        [FoldoutGroup("Components/FX"), SerializeField] private ParticleImage m_PfxComet;

        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_ShowFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_HideFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_FirstUpgradeFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_UpgradeFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_MaxUpgradeFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_ShowButtonFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_HideButtonFeedbacks;

        //Indicator
        [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private State m_State = State.Show;
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

        private void OnEnable()
        {
            GameManager.Instance.onStateChange += OnStateChange;
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.onStateChange -= OnStateChange;
            }
        }

        private void Update()
        {
            CheckButtonToggle();
        }

        #endregion

        #region CALLBACKS

        private void OnStateChange()
        {
            switch (GameManager.Instance.m_State)
            {
                case GameManager.State.Home:
                    Show();
                    break;
                case GameManager.State.PlayingTown:
                    Show();
                    break;
                case GameManager.State.PlayingPuzzle:
                    Hide();
                    break;
                default:
                    Show();
                    break;
            }
        }

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

        private void Show()
        {
            if (m_State == State.Show) return;
            m_State = State.Show;
            m_ShowFeedbacks.PlayFeedbacks();
        }

        private void Hide()
        {
            if (m_State == State.Hide) return;
            m_State = State.Hide;
            m_HideFeedbacks.PlayFeedbacks();
        }

        public void Setup(TownBuildingLocalData localData)
        {
            townBuildinglocalData = localData;

            if (townBuildinglocalData == null) return;

            currentLevel = townBuildinglocalData.m_Level;
            townBuildingProperty = TownDataList.Instance.GetCurrentTownData().GetBuildingData(m_Id);

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
                GetHammerPopup.Instance.Show();
                return;
            }

            TownBuildingUpgradeProperty upgradeProperty = NextLevelProperty();
            PayCost(upgradeProperty);

            //PayAndGiveReward();
            isBusy = true;
            //currentLevel = Mathf.Clamp(currentLevel + 1, 0, townBuildingProperty.m_UpgradeList.Count - 1);
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
                GiveReward(upgradeProperty);
                currentLevel = Mathf.Clamp(currentLevel + 1, 0, townBuildingProperty.m_UpgradeList.Count - 1);
                Customize();
                SaveLevel();
                isBusy = false;
                TownController.Instance.onBuildingUpgraded?.Invoke();
            });
        }

        private void PayCost(TownBuildingUpgradeProperty upgradeProperty)
        {
            if (upgradeProperty == null) return;

            //Pay Cost
            GameManager.Instance.DecreaseCurrency(upgradeProperty.m_CostType, upgradeProperty.m_CostAmount);
        }

        private void GiveReward(TownBuildingUpgradeProperty upgradeProperty)
        {
            if (upgradeProperty == null) return;

            //Give Reward
            if (upgradeProperty.m_GiveReward && upgradeProperty.m_RewardType != CurrencyType.None)
            {
                Image rewardImagee = MainScreen.Instance.GetBarImage(upgradeProperty.m_RewardType);
                m_PfxComet.texture = rewardImagee.sprite.texture;
                m_PfxComet.attractorTarget = rewardImagee.rectTransform;     
                m_PfxComet.onLastParticleFinish.RemoveAllListeners();
                m_PfxComet.onLastParticleFinish.AddListener(() => GameManager.Instance.IncreaseCurrency(upgradeProperty.m_RewardType, upgradeProperty.m_RewardAmount));
                m_PfxComet.Play();
            }
        }

        /*private void PayAndGiveReward()
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

                    //Particle
                    Image rewardImagee = TownScreen.Instance.GetBarImage(nextUpgrade.m_RewardType);
                    m_PfxComet.texture = rewardImagee.sprite.texture;
                    m_PfxComet.attractorTarget = rewardImagee.rectTransform;
                    m_PfxComet.Play();
                }
            }
        }*/

        public void SaveLevel()
        {
            if (townBuildinglocalData == null) return;

            townBuildinglocalData.m_Level = currentLevel;
            DataManager.Instance.SaveGameData();
        }

        #endregion
    }
}

