using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using GameCore.Core;
using GameCore.UI;
using Sirenix.OdinInspector;
using UnityEngine.GameFoundation;


namespace GameCore.Core.UI
{
    [RequireComponent(typeof(CoreButton))]
    public class CoreCarouselItem : MonoBehaviour
    {
        [HideInInspector]
        public CoreButton m_Button;

        #region MonoBehaviour
        protected virtual void Awake()
        {
            m_Button = GetComponent<CoreButton>();
        }
        // Start is called before the first frame update
        protected virtual void Start()
        {
            m_Button = GetComponent<CoreButton>();
        }
        // Update is called once per frame
        protected virtual void Update()
        {

        }
        #endregion
        public virtual void Setup()
        {
            
        }
    }
}
