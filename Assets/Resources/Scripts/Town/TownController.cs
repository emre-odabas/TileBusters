using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using GameCore.Managers;
using System;
using System.Collections.Generic;
using GameCore.Gameplay;
using System.Linq;

namespace GameCore.Controllers
{
    public class TownController : MonoBehaviour
    {
        #region UTILITIES

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
        }
        
        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.onLevelSetup -= OnLevelSetup;
            }
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

        #endregion

        #region RETURN FUNCTIONS

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

