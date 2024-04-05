using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Core.UI;
using GameCore.Managers;
namespace GameCore.UI
{
    public class TapticButtonToggle : CoreButtonToggle
    {
        protected override void Start()
        {
            base.Start();
            ToggleImage();
        }
        public override void OnClickBtn()
        {
            base.OnClickBtn();
            SettingsManager.Instance.SetOnOffHaptic(m_IsOn,true);
        }
        public override void ToggleOn()
        {
            base.ToggleOn();
            m_OffImage.gameObject.SetActive(false);            
        }
        public override void ToggleOff()
        {
            base.ToggleOff();
            m_OffImage.gameObject.SetActive(true);
        }
    } 
}
