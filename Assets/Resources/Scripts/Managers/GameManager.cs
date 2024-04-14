﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Core;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
using UnityEngine.GameFoundation;
using Currency = UnityEngine.GameFoundation.Currency;
using HedgehogTeam.EasyTouch;
using GameCore.UI;
using UnityEngine.Serialization;

namespace GameCore.Managers
{
    public class GameManager : SingletonComponent<GameManager>
    {
        public enum State
        {
            Awaiting = 0,
            SettingUp = 1,
            Playing = 2,
            Finished = 3,
            Completed = 4,
            Failed = 5,
            Paused = 6
        }

        #region Events
        public UnityAction onAppStart;
        public UnityAction onLevelSetup;
        public UnityAction onUISetup;
        public UnityAction onLevelReady;
        public UnityAction onInitialize;
        public UnityAction onWaitPlayerAct;
        public UnityAction onPlayerFirstAct;
        public UnityAction onStartPlay;
        public UnityAction onLevelFinish;
        public UnityAction onLevelComplete;
        public UnityAction onLevelFail;
        public UnityAction onNextLevel;
        public UnityAction onRestartLevel;
        public UnityAction<int> onCoinChange;
        public UnityAction<int> onInGameCoinChange;
        public UnityAction<int> onKeyChange;
        public UnityAction<int> onScoreChange;
        
        #endregion

        #region Variables
        
        [ReadOnly] public State m_State = State.Awaiting;
        [FoldoutGroup("Currencies", expanded:true), ReadOnly] public int m_InGameCoin;
        [FoldoutGroup("Currencies"), ReadOnly] public Currency m_CoinCurrency;
        [FoldoutGroup("Level", expanded: true), ReadOnly] public List<TownData> m_Levels = new List<TownData>();
        [FoldoutGroup("Level"), SerializeField, Range(1, 100)] private int StartTownLevel;
        public int m_StartTownLevel
        {
            get
            {
                return StartTownLevel - 1;
            }
            set
            {
                StartTownLevel = value;
            }
        }
        [FoldoutGroup("Level"), ReadOnly] public int m_CurrentTownLevelIndex = 0;
        [FoldoutGroup("Level"), ReadOnly] public TownData m_CurrentTownData;
        [FoldoutGroup("Level"), ReadOnly] public GameObject m_CurrentPlatform;
        [FoldoutGroup("Level"), ReadOnly] public bool m_IsPlayerAct = false;
        [FoldoutGroup("Level"), ReadOnly] public bool m_IsPlayerFirstAct = true;
        [FoldoutGroup("Level")] public bool m_IsDebug = true;
        
        #endregion
        
        [FoldoutGroup("Components")] public Camera m_UICamera;
        [FoldoutGroup("Feedbacks", expanded: true)] public MMFeedbacks m_WinFeedback;
        [FoldoutGroup("Feedbacks")] public MMFeedbacks m_FailFeedback;
        [FoldoutGroup("Loop Delay Settings", expanded: true)] public float m_DelayOnStartPlay = 0;
        [FoldoutGroup("Loop Delay Settings")] public float m_DelayOnLevelSetup = 0;
        [FoldoutGroup("Loop Delay Settings")] public float m_DelayAfterLevelSetup = 0;
        [FoldoutGroup("Loop Delay Settings")] public float m_DelayOnFinish = 0;
        [FoldoutGroup("Loop Delay Settings")] public float m_DelayOnComplete = 0;
        [FoldoutGroup("Loop Delay Settings")] public float m_DelayOnFail = 0;
        [FoldoutGroup("Database")] public CurrencyData m_CurrencyData;
        public GameObject m_SplashScreen;

        #region MonoBehaviour

        protected override void Awake()
        {
#if UNITY_IOS
            Application.targetFrameRate = 60;
#endif
            m_Levels.Clear();
            base.Awake();
            Init();
        }
        
        void Start()
        {
            Core.Logger.Log("Game Manager", "On App Start");
            onAppStart?.Invoke();
        }

        #endregion

        #region Initialization

        void Init()
        {
            StartCoroutine(DOInitialize());
        }

        IEnumerator DOInitialize()
        {
            m_SplashScreen.SetActive(true);
            while (DataManager.Instance.m_LoadState == DataManager.LoadState.Unloaded)
            {
                yield return null;
            }
            Core.Logger.Log("Game Manager", "On Data Loaded");
            Core.Logger.Log("Game Manager", JsonUtility.ToJson(DataManager.Instance.m_GameData));

            while (DataManager.Instance.m_DataLayer == null)
            {
                yield return null;
            }
            Core.Logger.Log("Game Manager", "On Foundation Loaded");
            Core.Logger.Log("Game Manager", JsonUtility.ToJson(DataManager.Instance.m_DataLayer));
            InitializeGameFoundation();
            InitializeGame();
            onInitialize?.Invoke();
            
        }

        void InitializeGameFoundation()
        {
            m_CoinCurrency = GameFoundation.catalogs.currencyCatalog.FindItem("coin");
        }

        void InitializeGame()
        {
            GameData gameData = DataManager.Instance.m_GameData;

            if (!m_IsDebug)
                m_CurrentTownLevelIndex = gameData.m_TownLocalData.m_TownLevel;
            else
                m_CurrentTownLevelIndex = m_StartTownLevel;

            //Loop Levels
            m_Levels.AddRange(TownDB.Instance.m_List);
            List<TownData> _levels = new List<TownData>();
            for (int i = 0; i < 10; i++)
            {
                _levels.AddRange(m_Levels);
            }
            m_Levels = _levels;
            StartLevelLoop();
        }

        #endregion

        #region EasyTouch

        public void OnTouchStart(Gesture gesture)
        {
            if (m_IsPlayerAct && m_IsPlayerFirstAct) m_IsPlayerFirstAct = false;
        }
        #endregion

        #region Level Loop

        public void StartLevelLoop()
        {
            StartCoroutine(DOLevelLoop(GetCurrentLevel()));
        }
        public void StartPlay()
        {
            m_IsPlayerAct = true;
        }
        IEnumerator DOLevelLoop(TownData townData)
        {
            Core.Logger.Log("Game Manager", "Level Loop Started");
            yield return StartCoroutine(DOSetup(townData));
            if (!m_IsPlayerAct)
                yield return StartCoroutine(DOWaitPlayerAct(townData));
            //else
            //    yield return StartCoroutine(DOWaitFirstPlayerAct(level));
            yield return StartCoroutine(DOStartPlay(townData));
            yield return StartCoroutine(DOLevelFinish(townData));
            if (m_State == State.Completed)
                yield return StartCoroutine(DOLevelComplete(townData));
            else if (m_State == State.Failed)
                yield return StartCoroutine(DOLevelFail(townData));

            m_IsPlayerAct = false;
        }
        IEnumerator DOSetup(TownData townData)
        {
            m_State = State.SettingUp;
            yield return StartCoroutine(DOClearCurrentLevel());
            yield return StartCoroutine(DOSetupLevel(townData));
            yield return StartCoroutine(DOSetupUI(townData));
            onLevelReady?.Invoke();
            m_SplashScreen.SetActive(false);
        }
        IEnumerator DOClearCurrentLevel()
        {
            //if (m_CurrentTownData != null)
                //Destroy(m_CurrentTownData.m_Platform);

            if (m_CurrentPlatform != null)
                Destroy(m_CurrentPlatform); 

            yield return null;
        }
        IEnumerator DOSetupLevel(TownData townData)
        {
            //m_CurrentTownData = ScriptableObject.CreateInstance<TownData>();
            //Transform platform = GameScreen.Instance.m_TownsPlaceholder;
            //m_CurrentTownData.m_Platform = Instantiate(townData.m_Platform, platform);

            m_CurrentTownData = townData.Clone<TownData>();
            m_CurrentPlatform = Instantiate(m_CurrentTownData.m_Platform, GameScreen.Instance.m_TownsPlaceholder);

            m_IsPlayerFirstAct = true;
            //New System
            m_InGameCoin = (int)WalletManager.GetBalance(m_CoinCurrency);
            yield return new WaitForSeconds(m_DelayOnLevelSetup);
            onLevelSetup?.Invoke();
            yield return new WaitForSeconds(m_DelayAfterLevelSetup);
            Core.Logger.Log("Game Manager", "On Level Generator Setup");
        }
        IEnumerator DOSetupUI(TownData townData)
        {
            onUISetup?.Invoke();
            yield return null;
            Core.Logger.Log("Game Manager", "On UI Setup");
        }
        IEnumerator DOWaitPlayerAct(TownData townData)
        {
            onWaitPlayerAct?.Invoke();
            Core.Logger.Log("Game Manager", "On Wait Player Act");
            while (!m_IsPlayerAct)
            {
                yield return null;
            }
        }
        IEnumerator DOWaitFirstPlayerAct(TownData townData)
        {
            Core.Logger.Log("Game Manager", "On Wait Player First Act");
            while (m_IsPlayerFirstAct)
            {
                yield return null;
            }
            onPlayerFirstAct?.Invoke();
        }
        IEnumerator DOStartPlay(TownData townData)
        {
            m_State = State.Playing;
            yield return new WaitForSeconds(m_DelayOnStartPlay);
            Core.Logger.Log("Game Manager", "On Start Play");
            onStartPlay?.Invoke();
            while (m_State == State.Playing)
            {
                yield return null;
            }
            yield return null;
        }
        IEnumerator DOLevelFinish(TownData townData)
        {
            Core.Logger.Log("Game Manager", "On Finish Game");
            onLevelFinish?.Invoke();
            yield return new WaitForSeconds(m_DelayOnFinish);
        }

        IEnumerator DOLevelComplete(TownData townData)
        {
            Core.Logger.Log("Game Manager", "On Complete Level");
            if (m_WinFeedback != null)
                m_WinFeedback.PlayFeedbacks();
            onLevelComplete?.Invoke();
            yield return new WaitForSeconds(m_DelayOnComplete);
            SaveCoin();
            DataManager.Instance.m_GameData.m_TownLocalData.m_TownLevel++;
            DataManager.Instance.SaveGameData();
        }

        IEnumerator DOLevelFail(TownData townData)
        {
            yield return new WaitForSeconds(m_DelayOnFail);
            Core.Logger.Log("Game Manager", "On Level Fail");
            if (m_FailFeedback != null)
                m_FailFeedback.PlayFeedbacks();
            onLevelFail?.Invoke();
            yield return null;
        }

        #region Level Loop Controls

        [Title("Debug")]
        [Button]
        public void FailLevel()
        {
            m_State = State.Failed;
        }
        [Button]
        public void FinishLevel()
        {
            m_State = State.Finished;
        }
        [Button]
        public void CompleteLevel()
        {
            m_State = State.Completed;
        }
        [Button]
        public void NextLevel()
        {
            m_CurrentTownLevelIndex++;
            StartLevelLoop();
            onNextLevel?.Invoke();
        }
        [Button]
        public void RestartLevel()
        {
            StartLevelLoop();
            onRestartLevel?.Invoke();
        }
        public TownData GetLevel(int index)
        {
            return m_Levels[index];
        }

        public TownData GetCurrentLevel()
        {
            return GetLevel(m_CurrentTownLevelIndex);
        }
        
        #endregion
        #endregion
        #region Economy
       
        [Button]
        public void IncreaseInGameCoin(int coin)
        {
            if (m_InGameCoin>=m_CoinCurrency.maximumBalance)
                return;
            m_InGameCoin += coin;
            onInGameCoinChange?.Invoke(m_InGameCoin);
        }

        [Button]
        public void DecreaseInGameCoin(int coin)
        {
            if (m_InGameCoin <= 0)
                return;
            m_InGameCoin -= coin;
            onInGameCoinChange?.Invoke(m_InGameCoin);
        }

        public void SaveCoin()
        {
            WalletManager.SetBalance(m_CoinCurrency, m_InGameCoin);
        }
        #endregion


    }
}