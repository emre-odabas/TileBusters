using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Core;
using HedgehogTeam.EasyTouch;
using DG.Tweening;
using GameCore.Managers;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Cinemachine;

namespace GameCore.Controllers
{
    public class PlayerController : SingletonComponent<PlayerController>
    {
        //Settings
        //[FoldoutGroup("Settings")] public

        //Components
        //[FoldoutGroup("Components")] public

        //Profiler
        //[FoldoutGroup("Profiler"), ReadOnly] public


        #region MonoBehaviour

        private void OnEnable()
        {
            GameManager.Instance.onLevelSetup += OnLevelSetup;
            GameManager.Instance.onStartPlay += OnStartPlay;
            GameManager.Instance.onLevelFinish += OnLevelFinish;
            EasyTouch.On_SimpleTap += OnTap;
            EasyTouch.On_TouchStart += OnTouchStart;
            EasyTouch.On_TouchDown += OnTouchDown;
            EasyTouch.On_TouchUp += OnTouchUp;
            EasyTouch.On_SwipeStart += OnSwipeStart;
            EasyTouch.On_Swipe += OnSwipe;
            EasyTouch.On_SwipeEnd += OnSwipeEnd;
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.onLevelSetup -= OnLevelSetup;
                GameManager.Instance.onStartPlay -= OnStartPlay;
                GameManager.Instance.onLevelFinish -= OnLevelFinish;
            }
            EasyTouch.On_SimpleTap -= OnTap;
            EasyTouch.On_TouchStart -= OnTouchStart;
            EasyTouch.On_TouchDown -= OnTouchDown;
            EasyTouch.On_TouchUp -= OnTouchUp;
            EasyTouch.On_SwipeStart -= OnSwipeStart;
            EasyTouch.On_Swipe -= OnSwipe;
            EasyTouch.On_SwipeEnd -= OnSwipeEnd;
        }

        void Start()
        {
            
        }

        private void Update()
        {

        }

        #endregion

        #region Events

        void OnLevelSetup()
        {
           
        }

        void OnStartPlay()
        {
            
        }

        void OnLevelFinish()
        {
            
        }

        #endregion

        #region Easytouch Controls

        public void OnTap(Gesture gesture)
        {
            if (GameManager.Instance.m_State == GameManager.State.Playing)
            {

            }
        }

        public void OnSwipeStart(Gesture gesture)
        {
            if (GameManager.Instance.m_State == GameManager.State.Playing)
            {

            }
        }

        public void OnSwipe(Gesture gesture)
        {
            if (GameManager.Instance.m_State == GameManager.State.Playing)
            {

            }
        }

        public void OnSwipeEnd(Gesture gesture)
        {
            if (GameManager.Instance.m_State == GameManager.State.Playing)
            {

            }
        }

        public void OnTouchStart(Gesture gesture)
        {
            if (GameManager.Instance.m_State == GameManager.State.Playing)
            {

            }
        }

        public void OnTouchDown(Gesture gesture)
        {
            if (GameManager.Instance.m_State == GameManager.State.Playing)
            {

            }
        }

        public void OnTouchUp(Gesture gesture)
        {
            if (GameManager.Instance.m_State == GameManager.State.Playing)
            {

            }
        }

        #endregion
    }
}