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
        [FoldoutGroup("Components/Utilities"), SerializeField] private Transform m_TownsPlaceholder;
        [FoldoutGroup("Components/Utilities"), SerializeField] private GameObject m_CurrentTownPlatform;
        
        #region MONOBEHAVIOUR

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            InitTown();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.Instance.onGameSetup += InitTown;
            //GameManager.Instance.onInitialize += Show;
            //GameManager.Instance.onNextLevel += InitTown;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if(GameManager.Instance != null)
            {
                GameManager.Instance.onGameSetup -= InitTown;
                //GameManager.Instance.onInitialize -= Show;
                //GameManager.Instance.onNextLevel -= InitTown;
            }
        }

        #endregion

        #region RECALL FUNCTIONS

        #endregion

        #region OVERRIDES

        public override void Show()
        {
            base.Show();
            
        }

        #endregion

        #region FUNCTIONS

        private void InitTown()
        {
            if (m_CurrentTownPlatform != null)
                Destroy(m_CurrentTownPlatform);

            m_CurrentTownPlatform = Instantiate(TownDataList.Instance.GetCurrentTownData().m_Platform, m_TownsPlaceholder);
        }

        #endregion
    }
}
