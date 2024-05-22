using UnityEngine;
using GameCore.Core.UI;
using GameCore.Managers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

namespace GameCore.UI
{
    public class TownScreen : CoreScreen<TownScreen>
    {
        [FoldoutGroup("Components")]
        [FoldoutGroup("Components/Utilities")] public Transform m_TownsPlaceholder;
        [FoldoutGroup("Components/Utilities")] public GameObject m_TempLevel;
        
        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.Instance.onStartPlayTown += Show;

            if(m_TempLevel != null)
                Destroy(m_TempLevel);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if(GameManager.Instance != null)
            {
                GameManager.Instance.onStartPlayTown -= Show;
            }
        }

        #endregion

        #region RECALL FUNCTIONS

        #endregion

        #region Controls

        public override void Show()
        {
            base.Show();
        }

        #endregion

        #region Events

        

        #endregion
    }
}
