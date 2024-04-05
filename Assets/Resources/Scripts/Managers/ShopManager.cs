using GameCore.Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.GameFoundation;
using UnityEngine.GameFoundation.DefaultCatalog;
using UnityEngine.Promise;

namespace GameCore.Managers
{
    public class ShopManager : SingletonComponent<ShopManager>
    {

        #region Events
        public UnityAction<BaseTransaction> onPurchaseSuccess;
        public UnityAction<BaseTransaction, System.Exception> onPurchaseFail;
        public UnityAction<InventoryItem> onItemUnlock;
        public UnityAction<InventoryItem> onItemSelect;
        #endregion
        [ReadOnly]
        public readonly List<InventoryItem> m_InventoryItems = new List<InventoryItem>();
        [ReadOnly]
        public List<CoreShop> m_Shops;

        public int m_RandomUnlockCost
        {
            get
            {
                return ProjectSettings.Instance.m_BaseRandomUnlockCost + (ProjectSettings.Instance.m_BaseRandomUnlockCost * DataManager.Instance.m_GameData.m_RandomUnlockedItemCount);
            }
        }
        #region Monobehaviour
        protected override void Awake()
        {
            base.Awake();
            GameManager.Instance.onInitialize += OnManagerInitialize;
        }
        private void Start()
        {

        }
        #endregion

        #region Initialize
        public void Init()
        {
            CreateShops();
            InventoryManager.GetItems(m_InventoryItems);
        }
        public void Setup()
        {

        }
        public void CreateShops()
        {
            StoreAsset[] _storeAssets = GameFoundationDatabaseSettings.database.storeCatalog.GetItems();

            foreach (var _storeAsset in _storeAssets)
            {
                CoreShop _shop = new CoreShop();
                _shop.m_StoreKey = _storeAsset.key;
                var _store = GetStore(_storeAsset.key);

                //SetUnlockedItems
                foreach (var _transaction in _store.GetStoreItems())
                {
                    if (_transaction.rewards.ItemExchangeCount > 0)
                    {
                        var _itemExchange = _transaction.rewards.GetItemExchange(0);
                        var _itemDefinition = _itemExchange.item;

                        CoreShopItem _shopItem = new CoreShopItem();
                        _shopItem.m_InventoryDefinition = _itemDefinition;
                        _shopItem.m_Transaction = _transaction;
                        _shop.m_Items.Add(_shopItem);
                    }
                }
                m_Shops.Add(_shop);
            }
        }
        #endregion
        #region Events
        private void OnManagerInitialize()
        {
            Init();
        }
        void OnPurchaseSuccess(BaseTransaction _transaction)
        {
            if (ProjectSettings.Instance.m_ShopUnlockType == ProjectSettings.ShopUnlockType.UnlockRandom)
            {
                long _balance = m_RandomUnlockCost;
                WalletManager.RemoveBalance(GameManager.Instance.m_CoinCurrency, _balance);
                DataManager.Instance.m_GameData.m_RandomUnlockedItemCount++;
            }
            onItemUnlock?.Invoke(InventoryManager.FindItem(_transaction.rewards.GetItemExchange(0).item.key));
            SetItemSelected(_transaction);
            DataManager.Instance.SaveGameData();
        }
        void OnPurchaseFail()
        {

        }
        #endregion

        #region Store Controls
        public Store GetStore(string _key)
        {
            return GameFoundation.catalogs.storeCatalog.FindItem(_key);
        }
        public CoreShop GetShop(string _key)
        {
            return m_Shops.FirstOrDefault(x => x.m_StoreKey == _key);
        }
        public void PurchaseItem(CoreShopItem _shopItem)
        {
            Purchase(_shopItem.m_Transaction);
        }
        public void SetItemSelected(BaseTransaction _transaction)
        {
            var _itemExchange = _transaction.rewards.GetItemExchange(0);
            DataManager.Instance.m_GameData.m_SeletedItem = _itemExchange.item.key;
            onItemSelect?.Invoke(InventoryManager.FindItem(_transaction.rewards.GetItemExchange(0).item.key));
        }
        
        #endregion

        #region Transactions
        void Purchase(BaseTransaction _transaction)
        {
            StartCoroutine(ExecuteTransaction(_transaction));
        }
        IEnumerator ExecuteTransaction(BaseTransaction transaction)
        {
            Deferred<TransactionResult> deferred = TransactionManager.BeginTransaction(transaction);


            // wait for the transaction to be processed
            int currentStep = 0;

            while (!deferred.isDone)
            {
                // keep track of the current step and possibly show a progress UI
                if (deferred.currentStep != currentStep)
                {
                    currentStep = deferred.currentStep;
                }

                yield return null;
            }

            // now that the transaction has been processed, check for an error
            if (!deferred.isFulfilled)
            {
                OnPurchaseFail();
                onPurchaseFail?.Invoke(transaction, deferred.error);
                deferred.Release();
                yield break;
            }

            OnPurchaseSuccess(transaction);
            onPurchaseSuccess?.Invoke(transaction);
            // all done
            deferred.Release();
        }


        protected virtual void OnInventoryItemChanged(InventoryItem itemChanged)
        {
            Debug.Log("Inventory Item Changed +" + itemChanged.id);
        }
        
        #endregion
    }
}
