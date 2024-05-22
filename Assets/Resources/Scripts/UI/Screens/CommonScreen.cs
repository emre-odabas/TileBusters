﻿using UnityEngine;
using GameCore.Core.UI;
using GameCore.Managers;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using GameCore.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace GameCore.UI
{
    public class CommonScreen : CoreScreen<CommonScreen>
    {
        [FoldoutGroup("Components")]
        [FoldoutGroup("Components/Utilities"), SerializeField] private Image m_Background;
        [FoldoutGroup("Components/Bars"), SerializeField] private List<CurrencyBar> m_CurrencyBarList = new List<CurrencyBar>();

        #region MONOBEHAVIOUR

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
            GameManager.Instance.onLevelReady += Show;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (GameManager.Instance != null)
            {
                GameManager.Instance.onLevelReady -= Show;
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

        #region OVERRIDES

        public override void Show()
        {
            base.Show();

            m_Background.sprite = GameManager.Instance.GetCurrentTownData().m_Background;

            for(int i = 0; i < m_CurrencyBarList.Count; i++)
            {
                m_CurrencyBarList[i].UpdateCurrency(false);
            }
        }

        #endregion
    }
}