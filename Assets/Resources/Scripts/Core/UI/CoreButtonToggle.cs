using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GameCore.Core.UI
{
    [RequireComponent(typeof(Button))]
    public class CoreButtonToggle : MonoBehaviour
    {
        public bool m_IsOn = true;
        public Image m_OnImage;
        public Image m_OffImage;
        Button m_Btn;
        protected virtual void Awake()
        {
            m_Btn = GetComponent<Button>();
            m_Btn.onClick.AddListener(()=> OnClickBtn());
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        public virtual void OnClickBtn()
        {
            m_IsOn = !m_IsOn;
            ToggleImage();
        }
        public virtual void SetOnOff(bool _isOn)
        {
            m_IsOn = _isOn;
            if (_isOn)
            {
                ToggleOn();
            }
            else
            {
                ToggleOff();
            }
        }
        public virtual void ToggleImage()
        {
            if (m_IsOn)
            {
                ToggleOn();
            }
            else
            {
                ToggleOff();
            }
        }
        public virtual void ToggleOn()
        {
            
        }
        public virtual void ToggleOff()
        {

        }
        
    } 
}
