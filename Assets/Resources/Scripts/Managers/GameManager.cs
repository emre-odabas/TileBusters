using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Core;
using GameCore.Controllers;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Cinemachine;
using MoreMountains.Feedbacks;
using UnityEngine.GameFoundation;
using UnityEngine.GameFoundation.DefaultLayers;
using UnityEngine.GameFoundation.DefaultLayers.Persistence;
using UnityEngine.Promise;
using Currency = UnityEngine.GameFoundation.Currency;
using HedgehogTeam.EasyTouch;

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
        public UnityAction<Theme> onThemeSetup;
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
        [FoldoutGroup("Level", expanded: true), ReadOnly] public List<Level> m_Levels = new List<Level>();
        [FoldoutGroup("Level"), SerializeField, Range(1, 100)] private int StartLevel;
        public int m_StartLevel
        {
            get
            {
                return StartLevel - 1;
            }
            set
            {
                StartLevel = value;
            }
        }
        [FoldoutGroup("Level"), ReadOnly]
        public int m_CurrentLevelIndex = 0;
        [FoldoutGroup("Level"), ReadOnly]
        public Level m_CurrentLevel;
        [FoldoutGroup("Level"), ReadOnly]
        public bool m_IsPlayerAct = false;
        [FoldoutGroup("Level"), ReadOnly]
        public bool m_IsPlayerFirstAct = true;
        [FoldoutGroup("Level")]
        public bool m_IsDebug = true;
        #endregion
        [FoldoutGroup("Components", expanded: true)]
        public Transform m_PlatformPlaceHolder;
        [FoldoutGroup("Components")]
        public Camera m_UICamera;
        [FoldoutGroup("Feedbacks", expanded: true)]
        public MMFeedbacks m_WinFeedback;
        [FoldoutGroup("Feedbacks")]
        public MMFeedbacks m_FailFeedback;
        [FoldoutGroup("Loop Delay Settings", expanded: true)]
        public float m_DelayOnStartPlay = 0;
        [FoldoutGroup("Loop Delay Settings")]
        public float m_DelayOnLevelSetup = 0;
        [FoldoutGroup("Loop Delay Settings")]
        public float m_DelayAfterLevelSetup = 0;
        [FoldoutGroup("Loop Delay Settings")]
        public float m_DelayOnFinish = 0;
        [FoldoutGroup("Loop Delay Settings")]
        public float m_DelayOnComplete = 0;
        [FoldoutGroup("Loop Delay Settings")]
        public float m_DelayOnFail = 0;

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
        // Start is called before the first frame update
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
                m_CurrentLevelIndex = gameData.m_PlayerLevel;
            else
                m_CurrentLevelIndex = m_StartLevel;

            //Loop Levels
            m_Levels.AddRange(LevelDB.Instance.m_List);
            List<Level> _levels = new List<Level>();
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
        IEnumerator DOLevelLoop(Level level)
        {
            Core.Logger.Log("Game Manager", "Level Loop Started");
            yield return StartCoroutine(DOSetup(level));
            if (!m_IsPlayerAct)
                yield return StartCoroutine(DOWaitPlayerAct(level));
            //else
            //    yield return StartCoroutine(DOWaitFirstPlayerAct(level));
            yield return StartCoroutine(DOStartPlay(level));
            yield return StartCoroutine(DOLevelFinish(level));
            if (m_State == State.Completed)
                yield return StartCoroutine(DOLevelComplete(level));
            else if (m_State == State.Failed)
                yield return StartCoroutine(DOLevelFail(level));

            m_IsPlayerAct = false;
        }
        IEnumerator DOSetup(Level level)
        {
            m_State = State.SettingUp;
            yield return StartCoroutine(DOClearCurrentLevel());
            yield return StartCoroutine(DOSetupLevel(level));
            yield return StartCoroutine(DOSetupUI(level));
            onLevelReady?.Invoke();
            m_SplashScreen.SetActive(false);
        }
        IEnumerator DOClearCurrentLevel()
        {
            if (m_CurrentLevel != null)
                Destroy(m_CurrentLevel.m_Platform);

            yield return null;
        }
        IEnumerator DOSetupLevel(Level level)
        {
            m_CurrentLevel = ScriptableObject.CreateInstance<Level>();
            m_CurrentLevel.m_Theme = level.m_Theme;
            m_CurrentLevel.m_Platform = Instantiate(level.m_Platform, m_PlatformPlaceHolder);
            ThemeManager.Instance.SelectTheme(level.m_Theme);
            m_IsPlayerFirstAct = true;
            //New System
            m_InGameCoin = (int)WalletManager.GetBalance(m_CoinCurrency);
            yield return new WaitForSeconds(m_DelayOnLevelSetup);
            onLevelSetup?.Invoke();
            yield return new WaitForSeconds(m_DelayAfterLevelSetup);
            Core.Logger.Log("Game Manager", "On Level Generator Setup");
        }
        IEnumerator DOSetupUI(Level level)
        {
            onUISetup?.Invoke();
            yield return null;
            Core.Logger.Log("Game Manager", "On UI Setup");
        }
        IEnumerator DOWaitPlayerAct(Level level)
        {
            onWaitPlayerAct?.Invoke();
            Core.Logger.Log("Game Manager", "On Wait Player Act");
            while (!m_IsPlayerAct)
            {
                yield return null;
            }
        }
        IEnumerator DOWaitFirstPlayerAct(Level level)
        {
            Core.Logger.Log("Game Manager", "On Wait Player First Act");
            while (m_IsPlayerFirstAct)
            {
                yield return null;
            }
            onPlayerFirstAct?.Invoke();
        }
        IEnumerator DOStartPlay(Level level)
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
        IEnumerator DOLevelFinish(Level level)
        {
            Core.Logger.Log("Game Manager", "On Finish Game");
            onLevelFinish?.Invoke();
            yield return new WaitForSeconds(m_DelayOnFinish);
        }
        IEnumerator DOLevelComplete(Level level)
        {
            Core.Logger.Log("Game Manager", "On Complete Level");
            if (m_WinFeedback != null)
                m_WinFeedback.PlayFeedbacks();
            onLevelComplete?.Invoke();
            yield return new WaitForSeconds(m_DelayOnComplete);
            SaveCoin();
            DataManager.Instance.m_GameData.m_PlayerLevel++;
            DataManager.Instance.SaveGameData();
        }
        IEnumerator DOLevelFail(Level level)
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
            m_CurrentLevelIndex++;
            StartLevelLoop();
            onNextLevel?.Invoke();
        }
        [Button]
        public void RestartLevel()
        {
            StartLevelLoop();
            onRestartLevel?.Invoke();
        }
        public Level GetLevel(int index)
        {
            return m_Levels[index];
        }

        public Level GetCurrentLevel()
        {
            return GetLevel(m_CurrentLevelIndex);
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