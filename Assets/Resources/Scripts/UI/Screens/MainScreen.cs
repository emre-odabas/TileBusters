using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Core.UI;
using GameCore.Managers;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.GameFoundation;
using UnityEngine.UI;
using System.Linq;


namespace GameCore.UI
{
    public class MainScreen : CoreScreen<MainScreen>
    {
        #region FIELDS

        //Components
        [FoldoutGroup("Components", expanded: true)]
        [FoldoutGroup("Components/Town"), SerializeField] private Transform m_TownsPlaceholder;
        [FoldoutGroup("Components/Town"), SerializeField] private GameObject m_CurrentTownPlatform;
        [FoldoutGroup("Components/Town"), SerializeField] private Image m_Background;
        [FoldoutGroup("Components/Utilities")] public TextMeshProUGUI m_TxtButtonTownTitle;
        [FoldoutGroup("Components/Utilities")] public TextMeshProUGUI m_TxtButtonPuzzleTitle;
        [FoldoutGroup("Components/Bars"), SerializeField] private List<CurrencyBar> m_CurrencyBarList = new List<CurrencyBar>();

        #endregion

        #region MONOBEHAVIOUR

        protected override void Awake()
        {
            base.Awake();
            if (m_State == State.Hidden)
            {
                Hide();
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.Instance.onGameSetup += Show;
            GameManager.Instance.onInitialize += InitTown;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if(GameManager.Instance != null)
            {
                GameManager.Instance.onGameSetup -= Show;
                GameManager.Instance.onInitialize -= InitTown;
            }
        }

        #endregion

        #region OVERRIDES

        public override void Show()
        {
            base.Show();

            //InitTown();

            m_TxtButtonTownTitle.text = "Town " + (TownDataList.Instance.GetCurrentTownLevel()).ToString();
            m_TxtButtonPuzzleTitle.text = "1";

            m_Background.sprite = TownDataList.Instance.GetCurrentTownData().m_Background;

            for (int i = 0; i < m_CurrencyBarList.Count; i++)
            {
                m_CurrencyBarList[i].UpdateCurrency(false);
            }
        }

        #endregion

        #region CALLBACK FUNCTIONS



        #endregion

        #region RETURN FUNCTIONS

        public Image GetBarImage(CurrencyType currencyType)
        {
            CurrencyBar bar = m_CurrencyBarList.FirstOrDefault(x => x.m_CurrencyType == currencyType);
            if (bar == null)
            {
                Debug.LogError("Currency Bar not exist!");
                return null;
            }

            return bar.m_Image;
        }

        #endregion

        #region FUNCTIONS

        private void InitTown()
        {
            if (m_CurrentTownPlatform != null)
                Destroy(m_CurrentTownPlatform);

            m_CurrentTownPlatform = Instantiate(TownDataList.Instance.GetCurrentTownData().m_Platform, m_TownsPlaceholder);
        }

        #endregion

        #region BUTTONS

        public void BtnPlayTown()
        {
            GameManager.Instance.StartPlay_Town();
        }

        public void BtnPlayPuzzle()
        {
            GameManager.Instance.StartPlay_Puzzle();
        }

        public void OnClickBack()
        {
            GameManager.Instance.GoHome();
        }

        #endregion
    }
}