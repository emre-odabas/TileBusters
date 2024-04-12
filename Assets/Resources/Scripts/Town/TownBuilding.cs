using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

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
        [FoldoutGroup("Components/Upgrade"), SerializeField] private Image m_BuildingSprite;
        [FoldoutGroup("Components/Upgrade"), SerializeField] private Transform m_BtnTiersContainer;
        [FoldoutGroup("Components/Upgrade"), SerializeField] private GameObject m_RefButtonUpgradeTier;
        
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_ShowButtonFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_HideButtonFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_UpgradeFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_MaxUpgradeFeedbacks;
        
        //Indicator
        //[FoldoutGroup("Indicator"), SerializeField, ReadOnly] private
        
        //Privates

        #endregion


        #region MONOBEHAVIOUR

        private void Awake()
        {
            
        }

        private void Start()
        {
            
        }
        
        private void Update()
        {
            
        }
        
        private void OnEnable()
        {
            
        }
        
        private void OnDisable()
        {
            
        }

        private void OnValidate()
        {
            
        }

        #endregion

        #region CALLBACKS

        #endregion

        #region RETURN FUNCTIONS

        #endregion

        #region FUNCTIONS
        
        [Button]
        private void Upgrade()
        {

        }
         
        #endregion
    }
}

