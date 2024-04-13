using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using GameCore.Managers;
using System;
using GameCore.Core;

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
        [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private bool m_isBusy = false;
        [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private bool m_isMaxLevel = false;
        [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private int currentLevel = 0;
        [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private TownBuildingProperty townBuildingProperty;
        
        //Privates

        #endregion

        #region MONOBEHAVIOUR

        private void OnEnable()
        {
            GameManager.Instance.onStartPlay += OnStartPlay;
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.onStartPlay -= OnStartPlay;
            }
        }

        #endregion

        #region CALLBACKS

        private void OnStartPlay()
        {
            ButtonToggle(true);
        }

        #endregion

        #region RETURN FUNCTIONS

        #endregion

        #region FUNCTIONS

        public void Setup()
        {
            currentLevel = -1;
            townBuildingProperty = GameManager.Instance.m_CurrentTownData.GetBuildingData(m_Id);
            Customize();
            ButtonToggle(false);
        }

        private void Customize()
        {
            if (townBuildingProperty == null) return;

            //Image
            m_BuildingImage.enabled = currentLevel != -1;
            if (m_BuildingImage.enabled)
                m_BuildingImage.sprite = townBuildingProperty.m_UpgradeList[currentLevel].m_Sprite;

            //Button
            
        }

        private void ButtonToggle(bool isShow)
        {
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
            if (m_isBusy) return;
            if (m_isMaxLevel) return;

            m_isBusy = true;
            currentLevel = Mathf.Clamp(currentLevel + 1, 0, townBuildingProperty.m_UpgradeList.Count - 1);
            Customize();
            
            m_isMaxLevel = currentLevel == townBuildingProperty.m_UpgradeList.Count - 1;
            ButtonToggle(!m_isMaxLevel);

            MMFeedbacks upgradeFeedbacks = m_isMaxLevel ? m_MaxUpgradeFeedbacks : m_UpgradeFeedbacks;
            upgradeFeedbacks.PlayFeedbacks();

            /*Utilities.DelayedCall(upgradeFeedbacks.Feedbacks[0], () =>
            {
                m_isBusy = false;
            });*/
            m_isBusy = false;
        }
         
        #endregion
    }
}

