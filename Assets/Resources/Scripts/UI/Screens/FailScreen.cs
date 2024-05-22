using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Core.UI;
using GameCore.Managers;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using GameCore.Controllers;

namespace GameCore.UI
{
    public class FailScreen : CoreScreen<FailScreen>
    {
        public Vector2 m_WorldAvarageRange;
        public TextMeshProUGUI m_WorldAvarageText;
        public TextMeshProUGUI m_MatchSliderRateText;
        public Slider m_MatchSlider;
        public Slider m_WolrdAvarageSlider;

        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.Instance.onAppStart += Hide;
            GameManager.Instance.onLevelFail += Show;
            GameManager.Instance.onRestartLevel += Hide;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (GameManager.Instance != null)
            {
                GameManager.Instance.onAppStart -= Hide;
                GameManager.Instance.onLevelFail -= Show;
                GameManager.Instance.onRestartLevel -= Hide;
            }
        }

        public override void Show()
        {
            base.Show();
            Debug.LogError("dsadasdas");
        }

        #region BUTTONS

        public void OnClickRestart()
        {
            GameManager.Instance.RestartLevel();
        }

        #endregion

    }

}