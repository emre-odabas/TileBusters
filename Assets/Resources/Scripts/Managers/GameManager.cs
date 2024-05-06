using System.Collections;
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
using System;

namespace GameCore.Managers
{
    public class GameManager : SingletonComponent<GameManager>
    {
        public enum State
        {
            Awaiting = 0,
            SettingUp = 1,
            PlayingTown = 2,
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
        public UnityAction onStartPlayTown;
        public UnityAction onStartPlayPuzzle;
        public UnityAction onLevelFinish;
        public UnityAction onLevelComplete;
        public UnityAction onLevelFail;
        public UnityAction onNextLevel;
        public UnityAction onRestartLevel;
        public UnityAction<int> onCoinChange;
        
        #endregion

        #region Variables
        
        [ReadOnly] public State m_State = State.Awaiting;
        [FoldoutGroup("Level"), SerializeField, Range(1, 100)] private int m_DegubStartTownLevel;
        public int StartTownLevel
        {
            get
            {
                return m_DegubStartTownLevel - 1;
            }
            set
            {
                m_DegubStartTownLevel = value;
            }
        }
        [FoldoutGroup("Level"), ReadOnly] public int m_CurrentTownLevelIndex = 0;
        [FoldoutGroup("Level"), ReadOnly] public TownData m_CurrentTownData;
        [FoldoutGroup("Level"), ReadOnly] public GameObject m_CurrentTownPlatform;
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

        [FoldoutGroup("Database"), HideLabel] public CurrencyData m_CurrencyData;
        [FoldoutGroup("Database"), HideLabel] public PuzzleTileDataList m_TileDataList;

        public GameObject m_SplashScreen;

        #region MonoBehaviour

        protected override void Awake()
        {
#if UNITY_IOS
            Application.targetFrameRate = 60;
#endif
            base.Awake();
            Init();
        }
        
        private void Start()
        {
            Core.Logger.Log("Game Manager", "On App Start");
            onAppStart?.Invoke();
        }

        private void Update()
        {
#if UNITY_EDITOR
            EditorDebugKeys();
#endif
        }

        #endregion

        #region Initialization

        private void Init()
        {
            StartCoroutine(DOInitialize());
        }

        private IEnumerator DOInitialize()
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

        private void InitializeGameFoundation()
        {

        }

        private void InitializeGame()
        {
            GameData gameData = DataManager.Instance.m_GameData;

            if (!m_IsDebug)
                m_CurrentTownLevelIndex = gameData.m_TownLocalData.m_TownLevel;
            else
                m_CurrentTownLevelIndex = StartTownLevel;

            StartLevelLoop();
        }

        #endregion

        #region Level Loop

        public void StartLevelLoop()
        {
            StartCoroutine(DOLevelLoop());
        }


        public void StartPlay_Town()
        {
            m_IsPlayerAct = true;
        }

        public void StartPlay_Puzzle()
        {
            m_IsPlayerAct = true;
        }


        private IEnumerator DOLevelLoop()
        {
            Core.Logger.Log("Game Manager", "Level Loop Started");
            yield return StartCoroutine(DOSetup());
            if (!m_IsPlayerAct)
                yield return StartCoroutine(DOWaitPlayerAct());
            //else
            //    yield return StartCoroutine(DOWaitFirstPlayerAct(level));

            yield return StartCoroutine(DOStartPlay());

            yield return StartCoroutine(DOLevelFinish());
            if (m_State == State.Completed)
                yield return StartCoroutine(DOLevelComplete());
            else if (m_State == State.Failed)
                yield return StartCoroutine(DOLevelFail());

            m_IsPlayerAct = false;
        }
        private IEnumerator DOSetup()
        {
            m_State = State.SettingUp;
            yield return StartCoroutine(DOClearCurrentLevel());
            yield return StartCoroutine(DOSetupLevel());
            yield return StartCoroutine(DOSetupUI());
            onLevelReady?.Invoke();
            m_SplashScreen.SetActive(false);
        }
        private IEnumerator DOClearCurrentLevel()
        {
            //if (m_CurrentTownData != null)
                //Destroy(m_CurrentTownData.m_Platform);

            if (m_CurrentTownPlatform != null)
                Destroy(m_CurrentTownPlatform); 

            yield return null;
        }
        private IEnumerator DOSetupLevel()
        {
            //m_CurrentTownData = ScriptableObject.CreateInstance<TownData>();
            //Transform platform = GameScreen.Instance.m_TownsPlaceholder;
            //m_CurrentTownData.m_Platform = Instantiate(townData.m_Platform, platform);

            m_CurrentTownData = GetCurrentTownData().Clone<TownData>();
            m_CurrentTownPlatform = Instantiate(m_CurrentTownData.m_Platform, TownScreen.Instance.m_TownsPlaceholder);

            m_IsPlayerFirstAct = true;
            yield return new WaitForSeconds(m_DelayOnLevelSetup);
            onLevelSetup?.Invoke();
            yield return new WaitForSeconds(m_DelayAfterLevelSetup);
            Core.Logger.Log("Game Manager", "On Level Generator Setup");
        }
        private IEnumerator DOSetupUI()
        {
            onUISetup?.Invoke();
            yield return null;
            Core.Logger.Log("Game Manager", "On UI Setup");
        }
        private IEnumerator DOWaitPlayerAct()
        {
            onWaitPlayerAct?.Invoke();
            Core.Logger.Log("Game Manager", "On Wait Player Act");
            while (!m_IsPlayerAct)
            {
                yield return null;
            }
        }
        /*private IEnumerator DOWaitFirstPlayerAct(TownData townData)
        {
            Core.Logger.Log("Game Manager", "On Wait Player First Act");
            while (m_IsPlayerFirstAct)
            {
                yield return null;
            }
            onPlayerFirstAct?.Invoke();
        }*/
        private IEnumerator DOStartPlay()
        {
            m_State = State.PlayingTown;
            yield return new WaitForSeconds(m_DelayOnStartPlay);
            Core.Logger.Log("Game Manager", "On Start Play");
            onStartPlayTown?.Invoke();
            while (m_State == State.PlayingTown)
            {
                yield return null;
            }
            yield return null;
        }
        private IEnumerator DOLevelFinish()
        {
            Core.Logger.Log("Game Manager", "On Finish Game");
            onLevelFinish?.Invoke();
            yield return new WaitForSeconds(m_DelayOnFinish);
        }

        private IEnumerator DOLevelComplete()
        {
            Core.Logger.Log("Game Manager", "On Complete Level");
            if (m_WinFeedback != null)
                m_WinFeedback.PlayFeedbacks();
            onLevelComplete?.Invoke();
            yield return new WaitForSeconds(m_DelayOnComplete);
            //SaveCoin();
            DataManager.Instance.m_GameData.m_TownLocalData.m_TownLevel++;
            DataManager.Instance.SaveGameData();
        }

        private IEnumerator DOLevelFail()
        {
            yield return new WaitForSeconds(m_DelayOnFail);
            Core.Logger.Log("Game Manager", "On Level Fail");
            if (m_FailFeedback != null)
                m_FailFeedback.PlayFeedbacks();
            onLevelFail?.Invoke();
            yield return null;
        }

        #region Level Loop Controls

        public void FailLevel()
        {
            m_State = State.Failed;
        }
        public void FinishLevel()
        {
            m_State = State.Finished;
        }
        public void CompleteLevel()
        {
            m_State = State.Completed;
        }
        public void NextLevel()
        {
            m_CurrentTownLevelIndex++;
            StartLevelLoop();
            onNextLevel?.Invoke();
        }
        public void RestartLevel()
        {
            StartLevelLoop();
            onRestartLevel?.Invoke();
        }

        public TownData GetCurrentTownData()
        {
            //We prevent it from giving an error when it reaches the last town.
            if (m_CurrentTownLevelIndex > TownDB.Instance.m_List.Count - 1)
                return TownDB.Instance.m_List[0];

            return TownDB.Instance.m_List[m_CurrentTownLevelIndex];
        }
        
        #endregion
        #endregion

        #region Currency

        public void IncreaseCurrency(CurrencyType currencyType, int value)
        {
            int currentValue = m_CurrencyData.GetCurrentCurrencyValue(currencyType);
            int increasedValue = currentValue + value;

            m_CurrencyData.SetCurrentCurrencyValue(currencyType, increasedValue);
            onCoinChange?.Invoke(increasedValue);
            DataManager.Instance.SaveGameData();
        }

        public void DecreaseCurrency(CurrencyType currencyType, int value)
        {
            int currentCoin = m_CurrencyData.GetCurrentCurrencyValue(currencyType);
            int decreasedValue = Mathf.Clamp(currentCoin - value, 0, 999999);

            m_CurrencyData.SetCurrentCurrencyValue(currencyType, decreasedValue);
            onCoinChange?.Invoke(decreasedValue);
            DataManager.Instance.SaveGameData();
        }

        public void SetCurrency(CurrencyType currencyType, int value)
        {
            m_CurrencyData.SetCurrentCurrencyValue(currencyType, value);
            onCoinChange?.Invoke(value);
            DataManager.Instance.SaveGameData();
        }

#if UNITY_EDITOR
        [Button]
        private void IncreaseCurrency_Coin(int value)
        {
            IncreaseCurrency(CurrencyType.Coin, value);
        }

        [Button]
        private void DecreaseCurrency_Coin(int value)
        {
            DecreaseCurrency(CurrencyType.Coin, value);
        }

        [Button]
        private void IncreaseCurrency_Hammer(int value)
        {
            IncreaseCurrency(CurrencyType.Hammer, value);
        }

        [Button]
        private void DecreaseCurrency_Hammer(int value)
        {
            DecreaseCurrency(CurrencyType.Hammer, value);
        }

        [Button]
        private void IncreaseCurrency_Star(int value)
        {
            IncreaseCurrency(CurrencyType.Star, value);
        }

        [Button]
        private void DecreaseCurrency_Star(int value)
        {
            DecreaseCurrency(CurrencyType.Star, value);
        }
#endif

        #endregion

        #region FUNCTIONS
        #if UNITY_EDITOR    
        private void EditorDebugKeys()
        {
            //if (!m_IsDebug) return;

            //Give Currencies
            if(Input.GetKeyDown(KeyCode.G))
            {
                IncreaseCurrency(CurrencyType.Hammer, 10);
                IncreaseCurrency(CurrencyType.Coin, 10);
                IncreaseCurrency(CurrencyType.Star, 10);
            }

            //Reset Currencies
            if (Input.GetKeyDown(KeyCode.R))
            {
                SetCurrency(CurrencyType.Hammer, 0);
                SetCurrency(CurrencyType.Coin, 0);
                SetCurrency(CurrencyType.Star, 0);
            }
        }
        #endif
        #endregion
    }
}