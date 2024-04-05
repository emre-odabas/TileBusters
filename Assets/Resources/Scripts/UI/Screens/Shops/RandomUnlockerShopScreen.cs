using GameCore.Core.UI;
using GameCore.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.GameFoundation;
using TMPro;
using GameCore.Core;
using GameCore.Managers;
using DG.Tweening;
using System.Linq;
using MoreMountains.Feedbacks;
using System;

namespace GameCore.UI
{
    public class RandomUnlockerShopScreen : ShopScreen
    {
        [FoldoutGroup("Buttons", expanded: true)]
        public CoreButton m_RandomUnlockButton;
        [FoldoutGroup("Buttons")]
        public TextMeshProUGUI m_RandomUnlockButtonCostText;

        [FoldoutGroup("Random Unlock Settings"), ReadOnly]
        public int m_CurrentUnlockCost = 0;

        [FoldoutGroup("Random Unlock Settings")]
        public float m_UnlockTime;


        protected override void Awake()
        {
            base.Awake();
            ShopManager.Instance.onItemUnlock += OnItemUnlocked;
            WalletManager.balanceChanged += OnBalanceChanged;
            //TransactionManager.transactionSuccee ded += OnItemUnlocked;
        }

        public override void Setup()
        {
            base.Setup();
            UpdateUnlockCost();
            UpdateUnlockButton();
        }

        void UpdateUnlockCost()
        {
            m_CurrentUnlockCost = ShopManager.Instance.m_RandomUnlockCost;
        }

        void UpdateUnlockButton()
        {
            m_RandomUnlockButtonCostText.text = m_CurrentUnlockCost.ToString();
            if (m_CurrentUnlockCost > WalletManager.GetBalance(GameManager.Instance.m_CoinCurrency))
            {
                m_RandomUnlockButton.interactable = false;
            }
            else
            {
                m_RandomUnlockButton.interactable = true;
            }
        }
        private void OnItemUnlocked(InventoryItem _item)
        {
            Debug.Log("Item Unlocked");
            UpdateUnlockCost();
            UpdateUnlockButton();
        }
        private void OnBalanceChanged(BalanceChangedEventArgs _args)
        {
            UpdateUnlockCost();
            UpdateUnlockButton();
        }
        #region Random Unlock 
        public void RandomUnlock()
        {
            StartCoroutine(DORandomUnlock());
        }
        IEnumerator DORandomUnlock()
        {
            m_RandomUnlockButton.interactable = false;
            m_TouchBlockPanel.gameObject.SetActive(true);

            float _currentTime = 0;

            CoreShop _shop = ShopManager.Instance.GetShop(m_StoreKey);
            List<CoreShopItem> _lockedItems = _shop.GetLockedItems();

            List<ShopCarousel.ItemData> _itemDatas = new List<ShopCarousel.ItemData>();
            _itemDatas = m_ItemsCarousel.m_CarouselItems.Where(x => x.m_Page == m_ItemsCarousel.m_CurrentPage && x.m_Data.GetAmount() == 0).ToList();

            RandomUnlockerShopCarouselItem _selectedItem = null;
            ShopCarousel.ItemData _selectedItemData = null;

            if (_shop.GetLockedItems().Count < 2)
            {
                _selectedItemData = _itemDatas[0];
                _selectedItem = _selectedItemData.m_Item as RandomUnlockerShopCarouselItem;
                _selectedItem.RandomSelect();
            }
            else
            {
                while (_currentTime < m_UnlockTime)
                {

                    ShopCarousel.ItemData _randomItemData = _itemDatas.GetRandomItemFromList();

                    while (_selectedItemData == _randomItemData)
                    {
                       _randomItemData = _itemDatas.GetRandomItemFromList();
                    }
                    _selectedItemData = _randomItemData;
                    _selectedItem = _selectedItemData.m_Item as RandomUnlockerShopCarouselItem;

                    foreach (var _data in _itemDatas)
                    {
                        _data.m_Item.SetLocked();
                    }

                    _selectedItem.RandomSelect();
                    yield return new WaitForSeconds(0.1f);
                    _currentTime += 0.1f;
                }
            }

            if (_selectedItem != null)
            {
                ShopManager.Instance.PurchaseItem(_selectedItemData.m_Data);
                _selectedItem.Unlock(null);
            }
            //m_RandomUnlockButton.interactable = true;
            m_TouchBlockPanel.gameObject.SetActive(false);
        }
        #endregion
    }
}