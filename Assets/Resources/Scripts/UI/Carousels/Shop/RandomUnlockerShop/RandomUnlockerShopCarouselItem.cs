using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Core.UI;
using UnityEngine.GameFoundation;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
using UnityEngine.Events;
using DG.Tweening;
using GameCore.Managers;

namespace GameCore.UI
{
    public class RandomUnlockerShopCarouselItem : ShopCarouselItem
    {
        [FoldoutGroup("Components")]
        public Transform m_RandomUnlockTransform;

        [FoldoutGroup("Feedbacks")]
        public MMFeedbacks m_RandomSelectFeedback;

        public void RandomSelect()
        {
            m_LockedTransform.gameObject.SetActive(false);
            m_UnlockedTransform.gameObject.SetActive(false);
            m_SelectedTransform.gameObject.SetActive(false);
            m_RandomUnlockTransform.gameObject.SetActive(true);

            if (m_RandomSelectFeedback != null)
                m_RandomSelectFeedback.PlayFeedbacks();
        }
        public override void Unlock(UnityAction _callback)
        {
            base.Unlock(_callback);
            SetUnlocked();
            DOVirtual.DelayedCall(0.5f, () =>
            {
                SetSelected();
            });
            
            if (_callback != null)
                _callback();
        }

        public override void Select(UnityAction _callback)
        {
            base.Select(_callback);
            SetSelected();
        }
        public override void SetLocked()
        {
            base.SetLocked();
            m_RandomUnlockTransform.gameObject.SetActive(false);
        }
        public override void SetSelected()
        {
            base.SetSelected();
            m_RandomUnlockTransform.gameObject.SetActive(false);
        }
        public override void SetUnlocked()
        {
            base.SetUnlocked();
            m_RandomUnlockTransform.gameObject.SetActive(false);
        }
    } 
}
