using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameCore.Core;
namespace GameCore.Managers
{
    public class SettingsManager : SingletonComponent<SettingsManager>
    {
        public bool m_IsHapticOn = true;
        public bool m_IsSoundOn = true;
        public UnityAction<bool> onHapticToggle;
        public UnityAction<bool> onSoundToggle;
        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
            DataManager.Instance.onDataLoad += OnDataLoad;
        }
        // Start is called before the first frame update
        void Start()
        {

        }
        // Update is called once per frame
        void Update()
        {

        } 
        void OnDataLoad(GameData _data)
        {
            SetOnOffHaptic(_data.m_SettingsLocalData.m_IsHapticON, false);
            SetOnOffSound(_data.m_SettingsLocalData.m_IsSoundON, false);
        }
        #endregion
        public void SetOnOffHaptic(bool value, bool _saveData)
        {
            m_IsHapticOn = value;
            onHapticToggle?.Invoke(m_IsHapticOn);
            if (_saveData)
            {
                DataManager.Instance.m_GameData.m_SettingsLocalData.m_IsHapticON = m_IsHapticOn;
                DataManager.Instance.SaveGameData();
            }
        }
        public void SetOnOffSound(bool value, bool _saveData)
        {
            m_IsSoundOn = value;
            onSoundToggle?.Invoke(m_IsSoundOn);
            if (_saveData)
            {
                DataManager.Instance.m_GameData.m_SettingsLocalData.m_IsSoundON = m_IsSoundOn;
                DataManager.Instance.SaveGameData();
            }
        }
    } 
}
