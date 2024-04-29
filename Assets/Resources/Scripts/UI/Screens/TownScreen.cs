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
    public class TownScreen : CoreScreen<TownScreen>
    {
        [FoldoutGroup("Components")]
        [FoldoutGroup("Components/Utilities")] public Transform m_TownsPlaceholder;
        [FoldoutGroup("Components/Utilities")] public GameObject m_TempLevel;
        
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

            if(m_TempLevel != null)
                Destroy(m_TempLevel);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if(GameManager.Instance != null)
            {
                GameManager.Instance.onAppStart -= Show;
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

        

        #endregion
    }
}
