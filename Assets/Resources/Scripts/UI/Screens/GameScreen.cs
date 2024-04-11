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
        [FoldoutGroup("Bars", expanded: true)]public CoinBar m_CoinBar;
        [FoldoutGroup("Texts", expanded: true)] public TextMeshProUGUI m_LevelText;
        //[FoldoutGroup("Components")] public GameObject m_TouchPanel;

        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            GameManager.Instance.onAppStart += Hide;
            GameManager.Instance.onStartPlay += Show;
            GameManager.Instance.onLevelFinish += Hide;
            //Cache Currencies
            GameManager.Instance.onInGameCoinChange += OnCoinChange;
        }
        protected override void OnEnable()
        {
            
        }
        protected override void OnDisable()
        {
            
        }
        
        #endregion
        #region Controls
        
        public override void Show()
        {
            base.Show();
            
            m_CoinBar.UpdateCoin(GameManager.Instance.m_InGameCoin,false);
            m_LevelText.text = "Level " + (GameManager.Instance.m_CurrentLevelIndex + 1).ToString();
            //m_TouchPanel.gameObject.SetActive(true);
        }

        #endregion

        #region Events

        void OnCoinChange(int _coin)
        {
            m_CoinBar.UpdateCoin(_coin, true);
        }

        #endregion
    }
}
