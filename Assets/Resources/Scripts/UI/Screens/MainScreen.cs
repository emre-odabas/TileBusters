using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Core.UI;
using GameCore.Managers;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.GameFoundation;


namespace GameCore.UI
{
    public class MainScreen : CoreScreen<MainScreen>
    {
        [FoldoutGroup("Bars")] public CoinBar m_CoinBar;
        [FoldoutGroup("Texts", expanded: true)] public TextMeshProUGUI m_CoinText;
        [FoldoutGroup("Texts")] public TextMeshProUGUI m_LevelText;

        #region MonoBehavour

        protected override void Awake()
        {
            if (m_State == State.Hidden)
            {
                Hide();
            }
        }

        protected override void Start()
        {
            base.Start();
            GameManager.Instance.onWaitPlayerAct += Show;
            GameManager.Instance.onStartPlay += Hide;
            GameManager.Instance.onLevelSetup += OnLevelSetup;
            //New system
            WalletManager.balanceChanged += OnCurrencyBalanceChanged;
        }

        #endregion

        #region Controls

        public override void Show()
        {
            base.Show();
            
            m_CoinBar.UpdateCoin((int)WalletManager.GetBalance(GameManager.Instance.m_CoinCurrency), false);
        }
        
        #endregion

        #region Events

        private void OnLevelSetup()
        {
            m_LevelText.text = "Level " + (GameManager.Instance.m_CurrentLevelIndex + 1).ToString();
        }

        private void OnCurrencyBalanceChanged(BalanceChangedEventArgs _args)
        {
            Debug.Log("Balance Changed :" + _args.currency.key);
            Debug.Log("Balance Changed :" + _args.newBalance);
            if (_args.currency.key == GameManager.Instance.m_CoinCurrency.key)
                m_CoinBar.UpdateCoin((int)_args.newBalance, false);
        }

        #endregion
    }
}