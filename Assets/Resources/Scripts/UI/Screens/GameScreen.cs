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
        [FoldoutGroup("Components/Utilities", expanded: true)] public TextMeshProUGUI m_LevelText;
        
        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            GameManager.Instance.onAppStart += Show;
            GameManager.Instance.onInGameCoinChange += OnCoinChange;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if(m_TempLevel != null)
            {
                Destroy(m_TempLevel);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
        
        #endregion

        #region Controls
        
        public override void Show()
        {
            base.Show();
            
            m_CoinBar.UpdateCoin(GameManager.Instance.m_InGameCoin,false);
            m_LevelText.text = "Level " + (GameManager.Instance.m_CurrentTownLevelIndex + 1).ToString();
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
