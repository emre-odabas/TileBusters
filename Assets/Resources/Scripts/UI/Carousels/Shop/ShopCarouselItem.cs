using GameCore.Core;
using GameCore.Core.UI;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.GameFoundation;
using UnityEngine.UI;

namespace GameCore.UI
{
    public class ShopCarouselItem : CoreCarouselItem
    {
        public enum State
        {
            Locked,
            Unlocked,
            Selected
        }
        public State m_State = State.Locked;

        [FoldoutGroup("Components", expanded: true)]
        public Transform m_LockedTransform;
        [FoldoutGroup("Components")]
        public Transform m_UnlockedTransform;
        [FoldoutGroup("Components")]
        public Transform m_SelectedTransform;
        [FoldoutGroup("Components")]
        public Image m_UnlockedItemImage;
        [FoldoutGroup("Components")]
        public Image m_SelectedItemImage;

        [FoldoutGroup("Feedbacks", expanded: true)]
        public MMFeedbacks m_UnlockFeedback;
        [FoldoutGroup("Feedbacks")]
        public MMFeedbacks m_SelectFeedback;


        public override void Setup()
        {
            base.Setup();
            m_LockedTransform.gameObject.SetActive(true);
            m_UnlockedTransform.gameObject.SetActive(false);
            m_SelectedTransform.gameObject.SetActive(false);
        }
        public virtual void Unlock(UnityAction _callback)
        {
            if (m_UnlockFeedback != null)
                m_UnlockFeedback.PlayFeedbacks();
        }
        public virtual void Select(UnityAction _callback)
        {
            if (m_SelectFeedback != null)
                m_SelectFeedback.PlayFeedbacks();
        }
        public virtual void SetUnlocked()
        {
            m_State = State.Unlocked;
            m_LockedTransform.gameObject.SetActive(false);
            m_UnlockedTransform.gameObject.SetActive(true);
            m_SelectedTransform.gameObject.SetActive(false);
        }
        public virtual void SetLocked()
        {
            m_State = State.Locked;
            m_LockedTransform.gameObject.SetActive(true);
            m_UnlockedTransform.gameObject.SetActive(false);
            m_SelectedTransform.gameObject.SetActive(false);
        }
        public virtual void SetSelected()
        {
            m_State = State.Selected;
            m_LockedTransform.gameObject.SetActive(false);
            m_UnlockedTransform.gameObject.SetActive(false);
            m_SelectedTransform.gameObject.SetActive(true);
        }
    }
}
