using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore.Core.UI;
using GameCore.Managers;
using System;
using TMPro;
using DG.Tweening;
using GameCore.Controllers;
using Sirenix.OdinInspector;
using UnityEngine.GameFoundation;
namespace GameCore.UI
{
    public class GameScreen : CoreScreen<GameScreen>
    {
        [FoldoutGroup("Components")]
        [FoldoutGroup("Components/Utilities")] public Transform m_TownsPlaceholder;
        [FoldoutGroup("Components/Utilities")] public GameObject m_TempLevel;
        [FoldoutGroup("Components/Utilities", expanded: true)]public CoinBar m_CoinBar;
        
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
            GameManager.Instance.onAppStart += Show;
            GameManager.Instance.onLevelSetup += OnLevelSetup;
            GameManager.Instance.onInGameCoinChange += OnCoinChange;

            if(m_TempLevel != null)
                Destroy(m_TempLevel);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if(GameManager.Instance != null)
            {
                GameManager.Instance.onAppStart -= Show;
                GameManager.Instance.onLevelSetup -= OnLevelSetup;
                GameManager.Instance.onInGameCoinChange -= OnCoinChange;
            }
        }

        #endregion

        #region Controls

        public override void Show()
        {
            base.Show();
        }

        #endregion

        #region Events

        private void OnLevelSetup()
        {
            m_CoinBar.UpdateCoin(GameManager.Instance.m_InGameCoin,false);
        }

        void OnCoinChange(int _coin)
        {
            m_CoinBar.UpdateCoin(_coin, true);
        }

        #endregion
    }
}
