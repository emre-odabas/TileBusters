using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace GameCore.Utilities
{
    public class TownBuilding : MonoBehaviour
    {
        #region UTILITIES

        #endregion
        
        #region FIELDS

        //Parameters
        [FoldoutGroup("Parameters")] 
        [FoldoutGroup("Parameters"), SerializeField] private string m_Id = "building_";
        //[FoldoutGroup("Parameters/Properties"), SerializeField] private

        //Components
        [FoldoutGroup("Components")] 
        [FoldoutGroup("Components/Utilities"), SerializeField] private Image m_Image;
        //[FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks 
        
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

