using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using GameCore.Core;
using Sirenix.OdinInspector;
using UnityEngine.GameFoundation;
using UnityEngine.GameFoundation.DefaultLayers;
using UnityEngine.GameFoundation.DefaultLayers.Persistence;
using UnityEngine.Promise;
namespace GameCore.Managers
{
    public class DataManager : SingletonComponent<DataManager>
    {
        public enum LoadState
        {
            Loaded = 1,
            Unloaded = 2
        }
        [ReadOnly]
        public LoadState m_LoadState = LoadState.Unloaded;

        public UnityAction<GameData> onDataLoad;
        public UnityAction<GameData> onDataSave;
        public GameData m_GameData = null;

        private string m_SavedGamesFileName = "data.json";

        public PersistenceDataLayer m_DataLayer;

        public GameParameter m_PlayerData; 
        public GameParameter m_SettingsData; 
        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
            m_GameData = null;
            m_LoadState = LoadState.Unloaded;
            TransactionManager.transactionSucceeded += OnTransactionSucceeded;
        }

        private void Start()
        {
            Debug.Log("DataManager Start");
            m_DataLayer = new PersistenceDataLayer(
              new LocalPersistence("FoundationData", new JsonDataSerializer()));
            GameFoundation.Initialize(m_DataLayer, OnGameFoundationInitialized, Debug.LogError);
            LoadGameData();
        } 
        #endregion
        void OnGameFoundationInitialized()
        {
            m_PlayerData = GameFoundation.catalogs.gameParameterCatalog.FindItem("PlayerData");
            m_SettingsData = GameFoundation.catalogs.gameParameterCatalog.FindItem("SettingsData");
        }
        #region Save / Load / Initialize Games
        public void LoadGameData()
        {
            string filePath = Application.persistentDataPath + "/" + m_SavedGamesFileName;

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                m_GameData = JsonUtility.FromJson<GameData>(dataAsJson);
                if (m_GameData == null)
                {
                    m_GameData = new GameData();
                    SaveGameData();
                }
                Debug.Log("Game Data Loaded" + filePath);
            }
            else
            {
                m_GameData = new GameData();
                SaveGameData();
            }
            m_LoadState = LoadState.Loaded;
            onDataLoad?.Invoke(m_GameData);
        }
        public void SaveGameData()
        {
            string dataAsJson = JsonUtility.ToJson(m_GameData);
            string filePath = Application.persistentDataPath + "/" + m_SavedGamesFileName;
            File.WriteAllText(filePath, dataAsJson);
            //Debug.Log("Game Data Saved : " + filePath);
            //onDataSave?.Invoke(m_GameData);
            // Deferred is a struct that helps you track the progress of an asynchronous operation of Game Foundation.
            Deferred saveOperation = m_DataLayer.Save();

            // Check if the operation is already done.
            if (saveOperation.isDone)
            {
                LogSaveOperationCompletion(saveOperation);
            }
            //else
            //{
            //    StartCoroutine(WaitForSaveCompletion(saveOperation));
            //}
        }
        
        private  IEnumerator WaitForSaveCompletion(Deferred saveOperation)
        {
            // Wait for the operation to complete.
            yield return saveOperation.Wait();

            LogSaveOperationCompletion(saveOperation);
        }
        private static void LogSaveOperationCompletion(Deferred saveOperation)
        {
            // Check if the operation was successful.
            if (saveOperation.isFulfilled)
            {
                Debug.Log("Saved!");
            }
            else
            {
                Debug.LogError($"Save failed! Error: {saveOperation.error}");
            }
        }
        #endregion

        private void OnTransactionSucceeded(BaseTransaction transaction, TransactionResult result)
        {
            Debug.Log("OnTransactionSucceeded");
            DataManager.Instance.SaveGameData();
        }

        [Button]
        private void ResetLocalData()
        {
            string jsonFile = Application.persistentDataPath + "/" + m_SavedGamesFileName;
            File.Delete(jsonFile);
            Debug.Log("Game Data Deleted: " + jsonFile);
        }

    }

}