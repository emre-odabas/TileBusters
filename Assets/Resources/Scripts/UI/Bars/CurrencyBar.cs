using UnityEngine;
using TMPro;
using GameCore.Managers;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.GameFoundation;
using MoreMountains.Feedbacks;

namespace GameCore.UI
{
    public class CurrencyBar : MonoBehaviour
    {
        //Parameters
        [FoldoutGroup("Parameters", expanded: true), SerializeField] private float m_UpdateDuration = 0.2f;

        //Components
        [FoldoutGroup("Components", expanded: true)]
        [FoldoutGroup("Components/Utilities"), SerializeField] public CurrencyType m_CurrencyType = CurrencyType.Coin;
        [FoldoutGroup("Components/Utilities"), SerializeField] private TextMeshProUGUI m_CurrencyText;
        [FoldoutGroup("Components/Utilities"), SerializeField] public Image m_Image;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_ValueChangedFeedbacks;

        //Privates

        #region MONOBEHAVIOUR

        private void OnEnable()
        {
            GameManager.Instance.onGameSetup += OnLevelSetup;
            GameManager.Instance.onCurrencyChange += OnCoinChange;
        }

        private void OnDisable()
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.onGameSetup -= OnLevelSetup;
                GameManager.Instance.onCurrencyChange -= OnCoinChange;
            }
        }

        private void OnValidate()
        {
            if (!Application.isPlaying && GameManager.Instance != null)
            {
                CurrencyProperty currencyProperty = GameManager.Instance.m_CurrencyData.GetCurrencyProperty(m_CurrencyType);
                if (currencyProperty == null) return;

                m_Image.sprite = currencyProperty.m_Sprite;
            }
        }

        #endregion

        #region CALLBACKS

        private void OnLevelSetup()
        {
            Customize();
            UpdateCurrency(false);
        }

        void OnCoinChange(int coin, CurrencyType currencyType, bool isIncrease)
        {
            if (m_CurrencyType != currencyType) return;

            UpdateCurrency(true);

            if (isIncrease)
                m_ValueChangedFeedbacks.PlayFeedbacks();
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
                /*DOVirtual.Float(currentCoin, targetCoin, m_UpdateDuration, (x) =>
                {
                    m_CurrencyText.text = Mathf.RoundToInt(x).ToString();
                });*/
                m_CurrencyText.text = targetCoin.ToString();
            }
            else
            {
                m_CurrencyText.text = targetCoin.ToString();
            }
        }

        public void UpdateCurrency(bool animate)
        {
            int currentCoin = int.Parse(m_CurrencyText.text);
            int targetCoin = GameManager.Instance.m_CurrencyData.GetCurrentCurrencyValue(m_CurrencyType);

            UpdateCurrency(currentCoin, targetCoin, animate);
        }

        #endregion
    }
}