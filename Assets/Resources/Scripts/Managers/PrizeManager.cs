using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Core;
using GameCore.Controllers;
using UnityEngine.Events;
using Sirenix.OdinInspector;

using GameCore.Core.UI;
using System.Linq;
using UnityEngine.GameFoundation;
using UnityEngine.Promise;

namespace GameCore.Managers
{
    public class PrizeManager : SingletonComponent<PrizeManager>
    {
        [Range(0, 100)]
        public float m_ItemRewardRate = 100;
        public GameObject m_ItemUnlockFX;
        public GameObject m_ItemUIPrefab;
        //public List<SkinPrizeItem> m_Skins = new List<SkinPrizeItem>();
        [Range(0, 100)]
        public float m_CoinRewardRate = 100;
        public GameObject m_CoinUnlockFX;
        public GameObject m_CoinUIPrefab;
        //public List<CoinPrizeItem> m_Coins = new List<CoinPrizeItem>();
        //[HideInInspector]
        public List<BaseTransaction> m_PrizePool;

        protected override void Awake()
        {
            base.Awake();
            GameManager.Instance.onLevelComplete += OnLevelComplete;
        }

        private void OnLevelComplete()
        {
            SetPrizePool();
        }

        void Start()
        {

        }
        [Button]
        public void SetPrizePool()
        {
            var _store = GameFoundation.catalogs.storeCatalog.FindItem("prize");
            var _coinPacks = _store.GetStoreItemsByTag("coinPack").ToList();
            var _items = _store.GetStoreItemsByTag("item").ToList();

            m_PrizePool = new List<BaseTransaction>();
            List<BaseTransaction> _transactions = RandomizeItemList(_items);
            if (_transactions != null)
                m_PrizePool.AddRange(_transactions);

            _transactions = RandomizeCoinList(_coinPacks);
            if (_transactions != null)
                m_PrizePool.AddRange(_transactions);
            //m_CurrentPrizes.AddRange(RandomizeCoinList());
        }
        #region Randomize Lists
        List<BaseTransaction> RandomizeItemList(List<BaseTransaction> _transactions)
        {
            List<BaseTransaction> _prizeItems = new List<BaseTransaction>();
            List<BaseTransaction> _lockedItems = new List<BaseTransaction>();
            List<BaseTransaction> _validItems = new List<BaseTransaction>();

            //Eleminate Unlocked items
            foreach (var _transaction in _transactions)
            {
                var _rewards = _transaction.rewards;
                var _itemReward = _rewards.GetItemExchange(0);
                var _currentItemCount = InventoryManager.FindItemsByDefinition(_itemReward.item, null);
                if (_currentItemCount == 0) _lockedItems.Add(_transaction);
            }
            // Eleminate High Level Items
            foreach (var _item in _lockedItems)
            {
                if (_item.HasStaticProperty("prizeUnlockLevel"))
                {
                    if (_item.GetStaticProperty("prizeUnlockLevel") <= GameManager.Instance.m_CurrentLevelIndex + 1)
                        _validItems.Add(_item);
                }
                else
                    _validItems.Add(_item);

            }

            if (_validItems.Count == 0)
            {
                Debug.LogError("There is no valid items. Please check database");
                return null;
            }
            //Creating Pool
            for (int i = 0; i < 100; i++)
            {
                int _chance = (int)Random.Range(0, 100);
                if (_chance <= m_ItemRewardRate)
                {
                    int _randomItemIndex = Random.Range(0, _validItems.Count);

                    bool _isExists = _prizeItems.Exists(x => x.key == _validItems[_randomItemIndex].key);
                    if (!_isExists)
                    {
                        _prizeItems.Add(_validItems[_randomItemIndex]);
                    }
                }
            }
            return _prizeItems;
        }
        List<BaseTransaction> RandomizeCoinList(List<BaseTransaction> _transactions)
        {
            List<BaseTransaction> _prizeItems = new List<BaseTransaction>();
            List<BaseTransaction> _items = new List<BaseTransaction>();
            List<BaseTransaction> _validItems = new List<BaseTransaction>();
            //Load Item
            foreach (var _transaction in _transactions)
            {
                var _rewards = _transaction.rewards;
                var _itemReward = _rewards.GetCurrencyExchange(0);
                _items.Add(_transaction);
            }
            // Eleminate High Level Items
            foreach (var _item in _items)
            {
                if (_item.HasStaticProperty("prizeUnlockLevel"))
                {
                    if (_item.GetStaticProperty("prizeUnlockLevel") <= GameManager.Instance.m_CurrentLevelIndex + 1)
                        _validItems.Add(_item);
                }
                else
                    _validItems.Add(_item);
            }
            //Creating Pool
            for (int i = 0; i < 100; i++)
            {
                int _chance = (int)Random.Range(0, 100);
                if (_chance <= m_CoinRewardRate)
                {
                    int _randomItemIndex = Random.Range(0, _validItems.Count);
                    _prizeItems.Add(_validItems[_randomItemIndex]);
                }
            }
            return _prizeItems;
        }
        #endregion


        #region Transactions
        public void Purchase(BaseTransaction transaction)
        {
            if (transaction == null)
            {
                Debug.LogError($"Transaction shouldn't be null.");
                return;
            }

            StartCoroutine(ExecuteTransaction(transaction));
        }
        public IEnumerator ExecuteTransaction(BaseTransaction transaction)
        {
            Debug.Log($"Now processing purchase: {transaction.displayName}");

            Deferred<TransactionResult> deferred = TransactionManager.BeginTransaction(transaction);

            // wait for the transaction to be processed
            int currentStep = 0;

            while (!deferred.isDone)
            {
                // keep track of the current step and possibly show a progress UI
                if (deferred.currentStep != currentStep)
                {
                    currentStep = deferred.currentStep;

                    Debug.Log($"Transaction is now on step {currentStep} of {deferred.totalSteps}");
                }

                yield return null;
            }

            // now that the transaction has been processed, check for an error
            if (!deferred.isFulfilled)
            {
                Debug.LogError($"Transaction Id:  {transaction.key} - Error Message: {deferred.error}");

                deferred.Release();
                yield break;
            }

            // here we can assume success
            Debug.Log("The purchase was successful in both the platform store and the data layer!");

            foreach (var currencyReward in deferred.result.rewards.currencies)
            {
                Debug.Log($"Player was awarded {currencyReward.amount} " + $"of currency '{currencyReward.currency.displayName}'");
            }
            // all done
            deferred.Release();
        }
        #endregion

    }
}