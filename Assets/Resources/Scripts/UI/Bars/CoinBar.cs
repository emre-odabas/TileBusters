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
    public class CoinBar : MonoBehaviour
    {
        public TextMeshProUGUI m_CoinText;
        public float m_UpdateDuration;
        void Start()
        {
            gameObject.SetActive(false);
        }
        public void UpdateCoin(int _currentCoin, int _coin, bool _animate)
        {
            if (_animate)
            {
                DOVirtual.Float(_currentCoin, _coin, m_UpdateDuration, (x) =>
                {
                    m_CoinText.text = Mathf.RoundToInt(x).ToString();
                });
            }
            else
            {
                m_CoinText.text = _coin.ToString();
            }
        }
        public void UpdateCoin(int _coin, bool _animate)
        {
            int _currentCoin = int.Parse(m_CoinText.text);
            UpdateCoin(_currentCoin, _coin, _animate);
        }
        public void SetCoinText(int _coin)
        {
            m_CoinText.text = _coin.ToString();
        }
    }
}