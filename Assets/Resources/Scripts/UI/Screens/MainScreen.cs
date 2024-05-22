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
        [FoldoutGroup("Components", expanded: true)]
        [FoldoutGroup("Components/Utilities")] public TextMeshProUGUI m_TxtTownTitle;
        [FoldoutGroup("Components/Utilities")] public TextMeshProUGUI m_TxtPuzzleTitle;

        #region MonoBehavour

        protected override void Awake()
        {
            base.Awake();
            if (m_State == State.Hidden)
            {
                Hide();
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.Instance.onWaitPlayerAct += Show;
            GameManager.Instance.onStartPlayTown += Hide;
            //WalletManager.balanceChanged += OnCurrencyBalanceChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if(GameManager.Instance != null)
            {
                GameManager.Instance.onWaitPlayerAct -= Show;
                GameManager.Instance.onStartPlayTown -= Hide;
            }
            //WalletManager.balanceChanged -= OnCurrencyBalanceChanged;
        }

        #endregion

        #region Controls

        public override void Show()
        {
            base.Show();
            

            m_TxtTownTitle.text = "Town " + (TownDataList.Instance.GetCurrentTownLevel()).ToString();
            m_TxtPuzzleTitle.text = "1";
            //m_CoinBar.UpdateCoin((int)WalletManager.GetBalance(GameManager.Instance.m_CoinCurrency), false);
        }
        
        #endregion

        #region Events

        /*private void OnCurrencyBalanceChanged(BalanceChangedEventArgs _args)
        {
            Debug.Log("Balance Changed :" + _args.currency.key);
            Debug.Log("Balance Changed :" + _args.newBalance);
            if (_args.currency.key == GameManager.Instance.m_CoinCurrency.key)
                m_CoinBar.UpdateCoin((int)_args.newBalance, false);
        }*/

        #endregion

        #region Buttons

        public void BtnTown()
        {
            GameManager.Instance.StartPlay_Town();
        }

        public void BtnPlay()
        {
            GameManager.Instance.StartPlay_Puzzle();

            TownScreen.Instance.Hide();
            PuzzleScreen.Instance.Show();
        }

        #endregion
    }
}