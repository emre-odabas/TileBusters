using UnityEngine;
using TMPro;
using DG.Tweening;
using GameCore.Managers;
//using UnityEngine.GameFoundation;

namespace GameCore.UI
{
    public class CoinBar : MonoBehaviour
    {
        public CurrencyType m_CurrencyType = CurrencyType.Coin;
        public TextMeshProUGUI m_CurrencyText;
        public float m_UpdateDuration;

        #region Monobehaviour

        private void OnEnable()
        {
            GameManager.Instance.onLevelSetup += OnLevelSetup;
            GameManager.Instance.onInGameCoinChange += OnCoinChange;
        }

        private void OnDisable()
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.onLevelSetup -= OnLevelSetup;
                GameManager.Instance.onInGameCoinChange -= OnCoinChange;
            }
        }

        #endregion

        #region Events

        private void OnLevelSetup()
        {
            //int coin = (int)WalletManager.GetBalance(GameFoundation.catalogs.currencyCatalog.FindItem("coin"));
            int coin = 0;
            UpdateCurrency(coin, false);
        }

        void OnCoinChange(int _coin)
        {
            UpdateCurrency(_coin, true);
        }

        #endregion

        #region Functions

        private void UpdateCurrency(int _currentCoin, int _coin, bool _animate)
        {
            if (_animate)
            {
                DOVirtual.Float(_currentCoin, _coin, m_UpdateDuration, (x) =>
                {
                    m_CurrencyText.text = Mathf.RoundToInt(x).ToString();
                });
            }
            else
            {
                m_CurrencyText.text = _coin.ToString();
            }
        }

        private void UpdateCurrency(int _coin, bool _animate)
        {
            int _currentCoin = int.Parse(m_CurrencyText.text);
            UpdateCurrency(_currentCoin, _coin, _animate);
        }

        #endregion
    }
}