﻿using GameCore.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameCore.Core.UI;
using GameCore.Managers;
using GameCore.Core;

namespace GameCore.UI
{
    public class PuzzleCompletePopup : CoreScreen<PuzzleCompletePopup>, IPopup
    {
        #region FIELDS

        //[FoldoutGroup("Components"), SerializeField] private

        #endregion

        #region MONOBEHAVIOUR

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.Instance.onPuzzleComplete += Show;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (GameManager.Instance != null)
            {
                GameManager.Instance.onPuzzleComplete -= Show;
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        #endregion

        #region RECALL FUNCTIONS

        

        #endregion

        #region BUTTONS

        public void OnClickTopToContinue()
        {
            Hide();
            GameManager.Instance.GoHome();
        }

        #endregion
    }
}