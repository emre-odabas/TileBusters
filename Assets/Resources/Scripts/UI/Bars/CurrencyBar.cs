using UnityEngine;
using TMPro;
using DG.Tweening;
using GameCore.Managers;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.GameFoundation;

namespace GameCore.UI
{
    public class CurrencyBar : MonoBehaviour
    {
        //Parameters
        [FoldoutGroup("Parameters", expanded: true), SerializeField] private float m_UpdateDuration = 0.2f;

        //Components
        [FoldoutGroup("Components", expanded: true)]
        [FoldoutGroup("Components/Utilities"), SerializeField] private CurrencyType m_CurrencyType = CurrencyType.Coin;
        [FoldoutGroup("Components/Utilities"), SerializeField] private TextMeshProUGUI m_CurrencyText;
        [FoldoutGroup("Components/Utilities"), SerializeField] private Image m_Image;

        #region MONOBEHAVIOUR

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

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                Customize();
            }
        }

        #endregion

        #region CALLBACKS

        private void OnLevelSetup()
        {
            UpdateCurrency(false);
        }

        void OnCoinChange(int _coin)
        {
            UpdateCurrency(true);
        }

        #endregion

        #region FUNCTIONS

        private void Customize()
        {
            CurrencyProperty currencyProperty = GameManager.Instance.m_CurrencyData.GetCurrencyProperty(m_CurrencyType);
            if (currencyProperty == null) return;

            m_Image.sprite = currencyProperty.m_Sprite;
        }

        private void UpdateCurrency(int currentCoin, int targetCoin, bool animate)
        {
            if (animate)
            {
                DOVirtual.Float(currentCoin, targetCoin, m_UpdateDuration, (x) =>
                {
                    m_CurrencyText.text = Mathf.RoundToInt(x).ToString();
                });
            }
            else
            {
                m_CurrencyText.text = targetCoin.ToString();
            }
        }

        private void UpdateCurrency(bool animate)
        {
            int currentCoin = int.Parse(m_CurrencyText.text);
            int targetCoin = 0;

            switch(m_CurrencyType)
            {
                case CurrencyType.Coin:
                    targetCoin = (int)WalletManager.GetBalance(GameFoundation.catalogs.currencyCatalog.FindItem("coin"));
                    break;

                case CurrencyType.Hammer:
                    targetCoin = (int)WalletManager.GetBalance(GameFoundation.catalogs.currencyCatalog.FindItem("hammer"));
                    break;

                case CurrencyType.Star:
                    targetCoin = (int)WalletManager.GetBalance(GameFoundation.catalogs.currencyCatalog.FindItem("star"));
                    break;
            }

            UpdateCurrency(currentCoin, targetCoin, animate);
        }

        #endregion
    }
}