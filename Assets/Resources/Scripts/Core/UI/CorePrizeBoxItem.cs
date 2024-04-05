using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Core;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using GameCore.Managers;
using UnityEngine.GameFoundation;
using MoreMountains.Feedbacks;


namespace GameCore.Core.UI
{

    public class CorePrizeBoxItem : MonoBehaviour
    {
        public BaseTransaction m_Transaction;
        public InventoryItemDefinition m_ItemDefinition;
        [HideInInspector]
        public CoreButton m_Button;
        public RectTransform m_LockedTransform;
        public RectTransform m_UnlockedTransform;
        public MMFeedbacks m_UnlockFeedback;
        public Image m_Icon;
        [HideInInspector]
        public GameObject m_UnlockFX;
        #region MonoBehaviour
        // Start is called before the first frame update
        protected virtual void Start()
        {

        }
        // Update is called once per frame
        protected virtual void Update()
        {

        }
        #endregion
        #region Setup
        public virtual void Setup(GameObject _unlockFX, Transform _VFXContainer, InventoryItemDefinition _itemDefinition)
        {
            m_Button = GetComponent<CoreButton>();
            m_UnlockFX = Instantiate(_unlockFX, _VFXContainer);
            m_UnlockFX.gameObject.SetActive(false);
            if (_itemDefinition != null)
            {
                Debug.Log(_itemDefinition.GetDetail<AssetsDetail>()?.GetAssetPath("prize_box_icon"));
                Debug.Log(_itemDefinition.GetDetail<AssetsDetail>()?.GetAsset<Sprite>("prize_box_icon"));
                Debug.Log(_itemDefinition.GetDetail<AssetsDetail>()?.GetAsset<Texture2D>("prize_box_icon"));
                m_Icon.sprite = _itemDefinition.GetDetail<AssetsDetail>()?.GetAsset<Sprite>("prize_box_icon");
            }

            m_Button.onClick.AddListener(() =>
            {
                Unlock();
            });
        }
        #endregion
        #region Controls
        public virtual void Unlock(UnityAction _callback = null)
        {
            if (WalletManager.GetBalance(GameManager.Instance.m_KeyCurrency) <= 0) return;
            
            m_UnlockFeedback.PlayFeedbacks();
            Sequence _sequence = DOTween.Sequence();
            _sequence.Pause();
            _sequence.Insert(0, m_LockedTransform.DOScale(1.5f, 1));
            _sequence.Insert(0, m_LockedTransform.DOShakeRotation(1, new Vector3(0, 0, 90), 90, 100, true));
            _sequence.Play().OnComplete(() =>
            {
                Debug.Log("Sequence Completed");
                m_UnlockFX.transform.position = transform.position;
                m_UnlockFX.gameObject.SetActive(true);
                m_LockedTransform.gameObject.SetActive(false);
                m_UnlockedTransform.gameObject.SetActive(true);
                m_Button.enabled = false;
                PrizeManager.Instance.Purchase(m_Transaction);
                if (_callback != null) _callback();
            });
        }
        public virtual void Destroy()
        {
            Destroy(m_UnlockFX);
            Destroy(gameObject);
        }
        #endregion
    }
}