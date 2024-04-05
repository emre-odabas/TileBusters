using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Core.UI;
using GameCore.Managers;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using GameCore.Core;
using Sirenix.OdinInspector;
using UnityEngine.GameFoundation;
using UnityEngine.GameFoundation.DefaultCatalog;
using UnityEngine.Promise;

namespace GameCore.UI
{
    public class ShopScreen : CoreScreen<ShopScreen>
    {

#if UNITY_EDITOR
        [ValueDropdown("GetStoreKeys")]
#endif
        public string m_StoreKey;
        [FoldoutGroup("Components")]
        public RectTransform m_TouchBlockPanel;
        [FoldoutGroup("Components")]
        public ShopCarousel m_ItemsCarousel;
        [FoldoutGroup("Components")]
        public GameObject m_ShopButtonContainer;

        [ReadOnly]
        public Store m_Store;
        [ReadOnly]
        public CoreShop m_Shop;
        #region MonoBehaviour
        protected override void Awake()
        {
            m_Container.DOLocalMoveY(-1500, 0);
        }
        protected override void Start()
        {
            base.Start();

            GameManager.Instance.onAppStart += Hide;
            GameManager.Instance.onUISetup += OnUISetup;
            GameManager.Instance.onStartPlay += OnStartPlay;
            GameManager.Instance.onInitialize += OnManagerInitialize;

            m_ItemsCarousel.onPageChange += OnCarouselPageChange;

            //InventoryManager.itemAdded += OnInventoryItemChanged;
            //InventoryManager.itemRemoved += OnInventoryItemChanged;

            //TransactionManager.transactionInitiated += OnTransactionInitiated;
            //TransactionManager.transactionProgressed += OnTransactionProgress;
            //TransactionManager.transactionSucceeded += OnTransactionSucceeded;
            //TransactionManager.transactionFailed += OnTransactionFailed;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
        public virtual void Initialize()
        {
            m_Store = GetStore();
            m_Shop = GetShop();
        }
        public virtual void Setup()
        {
            m_ItemsCarousel.Setup(m_Shop.m_Items);
        }
        #endregion
        #region Screen Controls
        public override void Show()
        {
            Setup();
            base.Show();
            m_Container.DOLocalMoveY(0, 0.7f).SetEase(Ease.OutBounce);
        }
        public override void Hide()
        {
            m_Container.DOLocalMoveY(-1500, 0.7f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                base.Hide();
            });
        }
        #endregion
        #region Store Controls
        public virtual Store GetStore()
        {
            return GameFoundation.catalogs.storeCatalog.FindItem(m_StoreKey);
        }
        public virtual CoreShop GetShop()
        {
            return ShopManager.Instance.GetShop(m_StoreKey);
        }
        public virtual void UnlockItem()
        {

        }
        public virtual void SetItemLocked()
        {

        }
        public virtual void SetItemUnlocked()
        {

        }
        public virtual void SetItemSelected()
        {

        }
        #endregion
        #region Events
        protected virtual void OnManagerInitialize()
        {
            Initialize();
        }
        protected virtual void OnUISetup()
        {
            if (ProjectSettings.Instance.m_Shop)
                m_ShopButtonContainer.SetActive(true);
            else
                m_ShopButtonContainer.SetActive(false);
        }
        protected virtual void OnStartPlay()
        {
            if (ProjectSettings.Instance.m_Shop)
                m_ShopButtonContainer.SetActive(false);
        }
        protected virtual void OnCarouselPageChange(int _pageIndex)
        {

        }
        #endregion
        #region Transactions
        /// <summary>
        /// Listener for changes in InventoryManager. Will get called whenever an item is added or removed.
        /// Because many items can get added or removed at a time, we will have the listener only set a flag
        /// that changes exist, and on our next update, we will check the flag to see whether changes to the UI
        /// need to be made.
        /// </summary>
        /// <param name="itemChanged">This parameter will not be used, but must exist so the signature is compatible with the inventory callbacks so we can bind it.</param>
        protected virtual void OnInventoryItemChanged(InventoryItem itemChanged)
        {
            Debug.Log("Inventory Item Changed +" + itemChanged.id);
        }

        /// <summary>
        /// Listens to OnTransactionInitiated callback and logs the name of the transaction initiated.
        /// </summary>
        /// <param name="transaction">The transaction initiated.</param>
        //protected virtual void OnTransactionInitiated(BaseTransaction transaction)
        //{
        //    Debug.Log("Transaction " + transaction.displayName + " initiated");
        //}

        /// <summary>
        /// Listens to OnTransactionProgress callback and logs the name and step count of the transaction.
        /// Could be used to display a progress bar to waiting users.
        /// </summary>
        /// <param name="transaction">The transaction initiated.</param>
        /// <param name="currentStep">The current step of the transaction progress.</param>
        /// <param name="totalSteps">The total number of steps to complete the transaction.</param>
        //protected virtual void OnTransactionProgress(BaseTransaction transaction, int currentStep, int totalSteps)
        //{
        //    Debug.Log("Transaction " + transaction.displayName + " is on step " + currentStep + " out of " + totalSteps);
        //}

        /// <summary>
        /// Listens to OnTransactionSucceeded callback and logs the name of the successful transaction.
        /// </summary>
        /// <param name="transaction">The transaction that succeeded</param>
        /// <param name="result">The result of the transaction</param>
        //protected virtual void OnTransactionSucceeded(BaseTransaction transaction, TransactionResult result)
        //{
        //    Debug.Log("Transaction " + transaction.displayName + " has succeeded.");
        //}

        /// <summary>
        /// Listens to OnTransactionFailed callback and logs the name and exception of the failed transaction.
        /// </summary>
        /// <param name="transaction">The transaction that failed.</param>
        /// <param name="exception">The failure reason.</param>
        //protected virtual void OnTransactionFailed(BaseTransaction transaction, System.Exception exception)
        //{
        //    Debug.Log("Transaction " + transaction.displayName + " has failed. " + exception);
        //}
        #endregion

        #region Editor
        IEnumerable GetStoreKeys()
        {
            StoreAsset[] _stores = GameFoundationDatabaseSettings.database.storeCatalog.GetItems();
            List<string> _storeKeys = new List<string>();

            foreach (var _store in _stores)
            {
                _storeKeys.Add(_store.key);
            }
            return _storeKeys;
        }
        #endregion
    }

}