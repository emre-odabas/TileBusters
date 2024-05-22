using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using GameCore.Managers;
using UnityEngine.GameFoundation;
using GameCore.Core;

namespace GameCore.UI
{
    public class TitleBar : MonoBehaviour
    {
        public TextMeshProUGUI m_TxtTitle;
        
        private void OnEnable()
        {
            GameManager.Instance.onLevelSetup += OnLevelSetup;
        }

        private void OnDisable()
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.onLevelSetup -= OnLevelSetup;
            }
        }

        private void OnLevelSetup()
        {
            m_TxtTitle.text = TownDataList.Instance.GetCurrentTownData().m_TownName;
        }
    }
}