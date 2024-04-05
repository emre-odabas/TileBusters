using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;
using GameCore.Core;
using UnityEngine.Events;

namespace GameCore.Managers
{
    public class RemoteConfigManager : SingletonComponent<RemoteConfigManager>
    {
        [System.Serializable]
        public class RemoteParameters
        {
            public bool m_IsDebug = false;
            public int m_StartLevel = 1;
        }
#region Events
        public UnityAction<RemoteParameters> onRemoteConfigLoad;
#endregion
        public enum LoadState
        {
            Remote = 0,
            Local = 1,
            Unloaded = 2
        }
        [ReadOnly]
        public LoadState m_LoadState = LoadState.Unloaded;
        public bool m_UseRemote = true;
        public RemoteParameters m_RemoteParameters = null;
        public string m_JsonData;
        private string m_FileName = "remoteParameters.json";
        protected override void Awake()
        {
            base.Awake();
            m_RemoteParameters = null;
            m_LoadState = LoadState.Unloaded;
#if UNITY_EDITOR
            // detect is this user new to the SDK
            bool isOldUser = false;
            if (m_UseRemote)
            {
                // initialize SDK
            }
            
#else
            LoadParametersFromRemote();
#endif
        }
        void Start()
        {

        }
#if UNITY_EDITOR
        void OnRemoteConfigLoad()
        {
            LoadParametersFromRemote();
        }
#endif
        [Button]
        public void LoadParametersFromRemote()
        {
            m_RemoteParameters = null;
            /*#if LPHTN
            if (m_UseRemote)
            {
                m_JsonData = RemoteConfig.GetInstance().Get("Parameters", null);
                m_RemoteParameters = JsonUtility.FromJson<RemoteParameters>(m_JsonData);
            }
            #endif*/

            if (m_RemoteParameters != null)
            {
                m_LoadState = LoadState.Remote;
                SaveParametersToLocal();
            }
            else if (m_RemoteParameters == null)
            {
                LoadParametersFromLocal();
            }
            onRemoteConfigLoad?.Invoke(m_RemoteParameters);
        }
        [Button]
        public void LoadParametersFromLocal()
        {
            string filePath = Application.persistentDataPath + "/" + m_FileName;

            if (File.Exists(filePath))
            {
                m_JsonData = File.ReadAllText(filePath);
                m_RemoteParameters = JsonUtility.FromJson<RemoteParameters>(m_JsonData);
            }


            if (m_RemoteParameters == null)
            {
                m_RemoteParameters = new RemoteParameters();
                m_JsonData = JsonUtility.ToJson(m_RemoteParameters);
                m_LoadState = LoadState.Unloaded;
            }
            else
                m_LoadState = LoadState.Local;
        }
        [Button]
        public void SaveParametersToLocal()
        {
            string dataAsJson = JsonUtility.ToJson(m_RemoteParameters);
            string filePath = Application.persistentDataPath + "/" + m_FileName;
            Debug.Log("Parameters Saved : " + filePath);
            File.WriteAllText(filePath, dataAsJson);
        }
        [Button]
        public void JsonOutput()
        {
            m_JsonData = JsonUtility.ToJson(m_RemoteParameters);
        }
    }
}
