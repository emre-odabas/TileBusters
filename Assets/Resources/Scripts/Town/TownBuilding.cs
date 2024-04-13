using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using GameCore.Managers;
using System;
using GameCore.Core;
using GameCore.Controllers;
using System.Collections.Generic;

namespace GameCore.Gameplay
{
    public class TownBuilding : MonoBehaviour
    {
        #region UTILITIES

        #endregion
        
        #region FIELDS

        //Parameters
        [FoldoutGroup("Parameters")] 
        [FoldoutGroup("Parameters"), SerializeField] private string m_Id = "building_";

        //Components
        [FoldoutGroup("Components")] 
        [FoldoutGroup("Components/Upgrade"), SerializeField] private Image m_BuildingImage;
        [FoldoutGroup("Components/Upgrade"), SerializeField] private Transform m_BtnTiersContainer;
        [FoldoutGroup("Components/Upgrade"), SerializeField] private GameObject m_RefButtonUpgradeTier;
        
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_ShowButtonFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_HideButtonFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_UpgradeFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_MaxUpgradeFeedbacks;
        
        //Indicator
        [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private bool isMaxLevel = false;
        [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private int currentLevel = 0;
        [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private TownBuildingProperty townBuildingProperty;
        
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

        #endregion

        #region FUNCTIONS

        public void Setup()
        {
            currentLevel = -1;
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

            //Image
            m_BuildingImage.enabled = currentLevel != -1;
            if (m_BuildingImage.enabled)
                m_BuildingImage.sprite = townBuildingProperty.m_UpgradeList[currentLevel].m_Sprite;

            //Button
            for (int i = 0; i < buttonUpgradeTierList.Count; i++)
            {
                if (i <= currentLevel)
                {
                    buttonUpgradeTierList[i].Fill();
                }
                else
                {
                    buttonUpgradeTierList[i].EmptyIt();
                }
            }
        }

        private void CheckButtonToggle()
        {
            bool value = (GameManager.Instance.m_State == GameManager.State.Playing && !isMaxLevel);
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

            isBusy = true;
            currentLevel = Mathf.Clamp(currentLevel + 1, 0, townBuildingProperty.m_UpgradeList.Count - 1);
            Customize();
            
            isMaxLevel = currentLevel == townBuildingProperty.m_UpgradeList.Count - 1;

            MMFeedbacks upgradeFeedbacks = isMaxLevel ? m_MaxUpgradeFeedbacks : m_UpgradeFeedbacks;
            upgradeFeedbacks.PlayFeedbacks();

            /*Utilities.DelayedCall(upgradeFeedbacks.Feedbacks[0], () =>
            {
                m_isBusy = false;
            });*/
            isBusy = false;

            TownController.Instance.onBuildingUpgraded?.Invoke();
        }
         
        #endregion
    }
}

