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
using UnityEngine.Analytics;

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
            //GameManager.Instance.onGameSetup += OnLevelSetup;
            GameManager.Instance.onTownComplete += OnTownComplete;
            onBuildingUpgraded += OnBuildingUpgraded;
        }
        
        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                //GameManager.Instance.onGameSetup -= OnLevelSetup;
                GameManager.Instance.onTownComplete -= OnTownComplete;
            }
            onBuildingUpgraded -= OnBuildingUpgraded;
        }

        private void Start()
        {
            m_Buildings = GetComponentsInChildren<TownBuilding>().ToList();

            Setup();
        }

        #endregion

        #region CALLBACKS

        /*private void OnLevelSetup()
        {
            Setup();
        }*/

        private void OnTownComplete()
        {
            DataManager.Instance.m_GameData.m_TownLocalData.m_TownBuildingLevels.Clear();
        }

        private void OnBuildingUpgraded()
        {
            if (IsAllBuildingsMaxLevel())
            {
                GameManager.Instance.TownComplete();
            }
        }

        #endregion

        #region RETURN FUNCTIONS

        public TownBuildingLocalData GetBuildingLocalData(string id)
        {
            TownBuildingLocalData townBuildingLocalData = DataManager.Instance.m_GameData.m_TownLocalData.m_TownBuildingLevels.FirstOrDefault(c => c.m_Id == id);
            if (townBuildingLocalData == null)
            {
                Debug.LogError("Could not find building with ID: " + id);
                return null;
            }

            return townBuildingLocalData;
        }

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
            if (DataManager.Instance.m_GameData.m_TownLocalData.m_TownBuildingLevels.Count == 0)
            {
                for (int i = 0; i < m_Buildings.Count; i++)
                {
                    string id = m_Buildings[i].m_Id;
                    int level = -1;
                    DataManager.Instance.m_GameData.m_TownLocalData.m_TownBuildingLevels.Add(new TownBuildingLocalData() { m_Id = id, m_Level = level });
                }
            }

            m_BackgroundImage.sprite = TownDataList.Instance.GetCurrentTownData().m_Background;

            for(int i = 0; i < m_Buildings.Count; i++)
            {
                m_Buildings[i].Setup(GetBuildingLocalData(m_Buildings[i].m_Id));
            }
        }

        #endregion

    }
}

