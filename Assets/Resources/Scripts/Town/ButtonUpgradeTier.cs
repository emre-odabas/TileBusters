using UnityEngine;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;

namespace GameCore.Gameplay
{
    public class ButtonUpgradeTier : MonoBehaviour
    {
        #region UTILITIES

        #endregion
        
        #region FIELDS

        //Parameters
        //[FoldoutGroup("Parameters"), SerializeField] private

        //Components
        [FoldoutGroup("Components")] 
        [FoldoutGroup("Components/Utilities"), SerializeField] private GameObject m_FillObject;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_FillFeedbacks;
        
        //Indicator
        //[FoldoutGroup("Indicator"), SerializeField, ReadOnly] private
        
        //Privates

        #endregion

        #region MONOBEHAVIOUR

        #endregion

        #region CALLBACKS

        #endregion

        #region RETURN FUNCTIONS

        #endregion

        #region FUNCTIONS

        public void Fill()
        {
            m_FillObject.SetActive(true);
            m_FillFeedbacks.PlayFeedbacks();
        }

        public void EmptyIt()
        {
            m_FillObject.SetActive(false);
        }
         
        #endregion
    }
}

