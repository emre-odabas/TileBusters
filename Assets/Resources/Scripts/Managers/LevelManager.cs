using UnityEngine;
using Sirenix.OdinInspector;

namespace GameCore.Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region UTILITIES

        #endregion
        
        #region FIELDS

        //Parameters
        //[FoldoutGroup("Parameters"), SerializeField] private

        //Components
        //[FoldoutGroup("Components")] 
        //[FoldoutGroup("Components/Utilities"), SerializeField] private
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
         
        #endregion

        #region EDITOR

#if UNITY_EDITOR

        [FoldoutGroup("Editor")]
        [Button]
        public void SetLevelInEditor(int level = 1)
        {
            
        }
#endif

        #endregion
    }
}

