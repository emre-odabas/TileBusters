using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Core;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
using GameCore.UI;
using System;

namespace GameCore.Managers
{
    public class GameManager : SingletonComponent<GameManager>
    {
        /*public enum State
        {
            Awaiting = 0,
            SettingUp = 1,
            PlayingTown = 2,
            Finished = 3,
            Completed = 4,
            Failed = 5
        }*/

        public enum State
        {
            Awaiting = 0,
            Home = 1,
            PlayingTown = 2,
            PlayingPuzzle = 3
        }

        #region EVENTS

        /*public UnityAction onAppStart;
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
        public UnityAction<int, CurrencyType, bool> onCurrencyChange;*/

        public UnityAction onAppStart;
        public UnityAction onGameSetup;
        public UnityAction onInitialize;
        public UnityAction onWaitPlayerAct;
        public UnityAction onStartPlay;
        public UnityAction<int, CurrencyType, bool> onCurrencyChange;
        public UnityAction onStateChange;

        //Town
        public UnityAction onStartPlayTown;
        public UnityAction onTownComplete;

        //Puzzle
        public UnityAction onStartPlayPuzzle;
        public UnityAction onPuzzleComplete;
        public UnityAction onPuzzleFail;

        #endregion

        #region FIELDS

        [ReadOnly] public State m_State = State.Awaiting;
        [FoldoutGroup("Level"), ReadOnly] public bool m_IsPlayerAct = false;
        [FoldoutGroup("Level"), ReadOnly] public bool m_IsPlayerFirstAct = true;
        [FoldoutGroup("Components")] public Camera m_UICamera;
        [FoldoutGroup("Components")] public GameObject m_SplashScreen;
        [FoldoutGroup("Feedbacks", expanded: true)] public MMFeedbacks m_WinFeedback;
        [FoldoutGroup("Feedbacks")] public MMFeedbacks m_FailFeedback;
        [FoldoutGroup("Loop Delay Settings")] public float m_DelayOnSplashScreen = 1.5f;
        [FoldoutGroup("Loop Delay Settings")] public float m_DelayOnStartPlay = 0;
        [FoldoutGroup("Loop Delay Settings")] public float m_DelayOnLevelSetup = 0;
        //[FoldoutGroup("Loop Delay Settings")] public float m_DelayAfterLevelSetup = 0;
        //[FoldoutGroup("Loop Delay Settings")] public float m_DelayOnFinish = 0;
        //[FoldoutGroup("Loop Delay Settings")] public float m_DelayOnComplete = 0;
        //[FoldoutGroup("Loop Delay Settings")] public float m_DelayOnFail = 0;
        [FoldoutGroup("Database"), HideLabel] public CurrencyData m_CurrencyData;
        [FoldoutGroup("Database"), HideLabel] public PuzzleTileDataList m_TileDataList;
        [FoldoutGroup("Database"), HideLabel] public TownDataList m_TownDataList;

        //Privates

        #endregion

        #region MONOBEHAVIOUR

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

        #region INIT

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
            onInitialize?.Invoke();
            StartCoroutine(DOGameLoop());
        }

        #endregion

        #region GAME LOOP

        private IEnumerator DOGameLoop()
        {
            Core.Logger.Log("Game Manager", "Game Loop Started");
            yield return StartCoroutine(DOSetup());
            if (!m_IsPlayerAct)
                yield return StartCoroutine(DOWaitPlayerAct());

            yield return StartCoroutine(DOStartPlay());
            m_IsPlayerAct = false;
        }

        private IEnumerator DOSetup()
        {
            yield return new WaitForSeconds(m_DelayOnSplashScreen);
            m_SplashScreen.SetActive(false);
            onGameSetup?.Invoke();
            GoHome();
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

        private IEnumerator DOStartPlay()
        {
            yield return new WaitForSeconds(m_DelayOnStartPlay);
            onStartPlay?.Invoke();
            Core.Logger.Log("Game Manager", "On Start Play");

            if (m_State == State.PlayingTown)
            { 
                yield return StartCoroutine(DOPlayingTown()); 
            }
            else if (m_State == State.PlayingPuzzle)
            {
                yield return StartCoroutine(DOPlayingPuzzle());
            }
        }

        private IEnumerator DOPlayingTown()
        {
            onStartPlayTown?.Invoke();
            Core.Logger.Log("Game Manager", "On Start Play Town");
            while (m_State == State.PlayingTown)
            {
                yield return null;
            }
            yield return null;
        }

        private IEnumerator DOPlayingPuzzle()
        {
            onStartPlayPuzzle?.Invoke();
            Core.Logger.Log("Game Manager", "On Start Play Puzzle");
            while (m_State == State.PlayingPuzzle)
            {
                yield return null;
            }
            yield return null;
        }

        #region _old

        /*private IEnumerator DOLevelLoop()
        {
            Core.Logger.Log("Game Manager", "Level Loop Started");
            //yield return StartCoroutine(DOSetup());
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

        private IEnumerator DOLevelLoop()
        {
            Core.Logger.Log("Game Manager", "Level Loop Started");
            //yield return StartCoroutine(DOSetup());
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
            //if (_currentTownPlatform != null)
                //Destroy(_currentTownPlatform);

            yield return null;
        }

        private IEnumerator DOSetupLevel()
        {
            //_currentTownPlatform = Instantiate(TownDataList.Instance.GetCurrentTownData().m_Platform, TownScreen.Instance.m_TownsPlaceholder);

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

        private IEnumerator DOWaitFirstPlayerAct(TownData townData)
        {
            Core.Logger.Log("Game Manager", "On Wait Player First Act");
            while (m_IsPlayerFirstAct)
            {
                yield return null;
            }
            onPlayerFirstAct?.Invoke();
        }

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
        }*/

        #endregion

        #region Level Loop Controls

        public void GoHome()
        {
            m_State = State.Home;
            onStateChange?.Invoke();
        }

        //Town
        public void StartPlay_Town()
        {
            m_IsPlayerAct = true;
            m_State = State.PlayingTown;
            onStateChange?.Invoke();
        }
        public void TownComplete()
        {
            Debug.LogError("Town Complete");
            onTownComplete?.Invoke();
            DataManager.Instance.m_GameData.m_TownLocalData.m_TownLevel++;
            DataManager.Instance.SaveGameData();
        }

        //Puzzle
        public void StartPlay_Puzzle()
        {
            m_IsPlayerAct = true;
            m_State = State.PlayingPuzzle;
            onStateChange?.Invoke();
        }
        public void PuzzleComplete()
        {
            Debug.LogError("Puzzle Complete");
            onPuzzleComplete?.Invoke();
        }
        public void PuzzleFail()
        {
            Debug.LogError("Puzzle Fail");
            onPuzzleFail?.Invoke();
        }

        //
        /*public void FailLevel()
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
            StartLevelLoop();
            onNextLevel?.Invoke();
        }

        public void RestartLevel()
        {
            StartLevelLoop();
            onRestartLevel?.Invoke();
        }*/

        #endregion

        #endregion

        #region CURRENCY

        public void IncreaseCurrency(CurrencyType currencyType, int value)
        {
            int currentValue = m_CurrencyData.GetCurrentCurrencyValue(currencyType);
            int increasedValue = currentValue + value;

            m_CurrencyData.SetCurrentCurrencyValue(currencyType, increasedValue);
            onCurrencyChange?.Invoke(increasedValue, currencyType, true);
            DataManager.Instance.SaveGameData();
        }

        public void DecreaseCurrency(CurrencyType currencyType, int value)
        {
            int currentCoin = m_CurrencyData.GetCurrentCurrencyValue(currencyType);
            int decreasedValue = Mathf.Clamp(currentCoin - value, 0, 999999);

            m_CurrencyData.SetCurrentCurrencyValue(currencyType, decreasedValue);
            onCurrencyChange?.Invoke(decreasedValue, currencyType, false);
            DataManager.Instance.SaveGameData();
        }

        public void SetCurrency(CurrencyType currencyType, int value)
        {
            m_CurrencyData.SetCurrentCurrencyValue(currencyType, value);
            onCurrencyChange?.Invoke(value, currencyType, false);
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