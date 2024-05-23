using UnityEngine;
using GameCore.Core.UI;
using GameCore.Managers;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using GameCore.Controllers;
using System;

namespace GameCore.UI
{
    public class PuzzleScreen : CoreScreen<PuzzleScreen>
    {
        //[FoldoutGroup("Components")]
        //[FoldoutGroup("Components/Utilities"), SerializeField] private Image m_Background; 
        
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
            GameManager.Instance.onStateChange += OnStateChange;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (GameManager.Instance != null)
            {
                GameManager.Instance.onStateChange -= OnStateChange;
            }
        }

        #endregion

        #region OVERRIDES

        public override void Show()
        {
            base.Show();
            //m_Background.sprite = TownDataList.Instance.GetCurrentTownData().m_Background;
        }

        #endregion

        #region RECALL FUNCTIONS

        private void OnStateChange()
        {
            switch (GameManager.Instance.m_State)
            {
                case GameManager.State.Home:
                    Hide();
                    break;
                case GameManager.State.PlayingTown:
                    Hide();
                    break;
                case GameManager.State.PlayingPuzzle:
                    Show();
                    break;
                default:
                    Hide();
                    break;
            }
        }

        #endregion

        #region BUTTONS

        public void OnClickBack()
        {
            GameManager.Instance.GoHome();
        }

        #endregion
    }
}
