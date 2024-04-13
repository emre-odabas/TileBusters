using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using GameCore.Managers;
using System;

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
        //[FoldoutGroup("Indicator"), SerializeField, ReadOnly] private
        
        //Privates

        #endregion


        #region MONOBEHAVIOUR

        private void OnEnable()
        {
            GameManager.Instance.onLevelSetup += OnLevelSetup;
        }
        
        private void OnDisable()
        {
            GameManager.Instance.onLevelSetup -= OnLevelSetup;
        }

        private void Start()
        {
            
        }

        #endregion

        #region CALLBACKS

        private void OnLevelSetup()
        {
            m_BackgroundImage.sprite = GameManager.Instance.m_CurrentTownData.m_Background;
        }

        #endregion

        #region RETURN FUNCTIONS

        #endregion

        #region FUNCTIONS

        #endregion

    }
}

