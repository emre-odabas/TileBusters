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
        [FoldoutGroup("Components/Bars"), SerializeField] private List<CurrencyBar> m_CurrencyBarList = new List<CurrencyBar>();

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
            //GameManager.Instance.onAppStart += Show;

            if(m_TempLevel != null)
                Destroy(m_TempLevel);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if(GameManager.Instance != null)
            {
                //GameManager.Instance.onAppStart -= Show;
            }
        }

        #endregion

        #region RECALL FUNCTIONS

        public Image GetBarImage(CurrencyType currencyType)
        {
            CurrencyBar bar = m_CurrencyBarList.FirstOrDefault(x => x.m_CurrencyType == currencyType);
            if (bar == null)
            {
                Debug.LogError("Currency Bar not exist!");
                return null; 
            }

            return bar.m_Image;
        }

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
