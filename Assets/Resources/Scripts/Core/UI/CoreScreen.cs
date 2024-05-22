using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
namespace GameCore.Core.UI
{
    public class CoreScreen<T> : SingletonComponent<T> where T: Object
    {
        public enum State {
            Shown = 0, 
            Hidden = 1 
        };

        public State m_State = State.Hidden;

        [FoldoutGroup("Components")] public RectTransform m_Container;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_ShowFeedbacks;
        [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_HideFeedbacks;

        protected virtual new void Awake()
        {
            base.Awake();
            if (m_State == State.Hidden)
            {
                Hide();
            }
        }
        protected virtual void Start()
        {

        }
        protected virtual void OnEnable()
        {

        }
        protected virtual void OnDisable()
        {

        }
        public virtual void Show()
        {
            m_State = State.Shown;
            m_ShowFeedbacks.PlayFeedbacks();
        }
        public virtual void Hide()
        {
            m_State = State.Hidden;
            m_HideFeedbacks.PlayFeedbacks();
        }
    }
}
