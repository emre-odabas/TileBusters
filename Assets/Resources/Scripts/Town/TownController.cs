using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using GameCore.Managers;
using System;
using System.Collections.Generic;
using GameCore.Gameplay;
using System.Linq;
using UnityEngine.Events;
using GameCore.Core;

namespace GameCore.Controllers
{
    public class TownController : SingletonComponent<TownController>
    {
        #region UTILITIES

        public UnityAction onBuildingUpgraded;

        #endregion
        
        #region FIELDS

        //Parameters
        //[FoldoutGroup("Parameters"), SerializeField] private

        //Components
        [FoldoutGroup("Components")] 
        [FoldoutGroup("Components/Utilities"), SerializeField] private Image m_BackgroundImage;
        //[FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks 
        
        //Indicator
        [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private List<TownBuilding> m_Buildings = new List<TownBuilding>();
        
        //Privates
        

        #endregion


        #region MONOBEHAVIOUR

        private void OnEnable()
        {
            GameManager.Instance.onLevelSetup += OnLevelSetup;
            onBuildingUpgraded += OnBuildingUpgraded;
        }
        
        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.onLevelSetup -= OnLevelSetup;
            }
            onBuildingUpgraded -= OnBuildingUpgraded;
        }

        private void Start()
        {
            m_Buildings = GetComponentsInChildren<TownBuilding>().ToList();
        }

        #endregion

        #region CALLBACKS

        private void OnLevelSetup()
        {
            Setup();
        }

        private void OnBuildingUpgraded()
        {
            if (IsAllBuildingsMaxLevel())
            {
                GameManager.Instance.CompleteLevel();
            }
        }

        #endregion

        #region RETURN FUNCTIONS

        private bool IsAllBuildingsMaxLevel()
        {
            for(int i = 0; i < m_Buildings.Count; i++)
            {
                if (!m_Buildings[i].IsMaxLevel())
                    return false;
            }
            return true;
        }

        #endregion

        #region FUNCTIONS

        private void Setup()
        {
            m_BackgroundImage.sprite = GameManager.Instance.m_CurrentTownData.m_Background;

            for(int i = 0; i < m_Buildings.Count; i++)
            {
                m_Buildings[i].Setup();
            }
        }

        #endregion

    }
}

