using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore.Managers;
using GameCore.Core;
using TMPro;
using GameCore.Core.UI;
using UnityEngine.GameFoundation;
using Sirenix.OdinInspector;

namespace GameCore.UI
{
    public class PrizeBox : MonoBehaviour
    {
        public GridLayoutGroup m_Grid;
        public RectTransform m_VFXContainer;

        public List<GameObject> m_SpawnedBoxes;
        [Button]
        public void Setup()
        {
            m_SpawnedBoxes = new List<GameObject>();
            for (int i = 0; i < 9; i++)
            {
                m_SpawnedBoxes.Add(CreateRandomItem());
            }
        }
        public void ClearCurrentItems()
        {
            foreach (var _box in m_SpawnedBoxes)
            {
                _box.GetComponent<CorePrizeBoxItem>().Destroy();
                
            }
            m_SpawnedBoxes.Clear();
        }

        GameObject CreateRandomItem()
        {
            List<BaseTransaction> _currentPrizes = PrizeManager.Instance.m_PrizePool;
            int _randomItemIndex = Random.Range(0, _currentPrizes.Count);

            BaseTransaction _selectedPrize = _currentPrizes[_randomItemIndex];
            var _itemRewards = _selectedPrize.rewards;
            GameObject _cloneItem = null;
            if (_selectedPrize.FindTag("coinPack") != null)
            {
                _cloneItem = Instantiate(PrizeManager.Instance.m_CoinUIPrefab, m_Grid.transform);
                _cloneItem.GetComponent<CorePrizeBoxItem>().m_Transaction = _selectedPrize;
                SetupCoin(_cloneItem.GetComponent<CorePrizeBoxItem>(), null);
            }
            else if (_selectedPrize.FindTag("item") != null)
            {
                _cloneItem = Instantiate(PrizeManager.Instance.m_ItemUIPrefab, m_Grid.transform);
                _cloneItem.GetComponent<CorePrizeBoxItem>().m_Transaction = _selectedPrize;
                var _itemDefinition = _itemRewards.GetItemExchange(0);
                SetupItem(_cloneItem.GetComponent<CorePrizeBoxItem>(), _itemDefinition.item);
            }
            return _cloneItem;
        }

       
        void SetupItem(CorePrizeBoxItem _boxItem, InventoryItemDefinition _itemDefinition)
        {
            _boxItem.Setup(PrizeManager.Instance.m_ItemUnlockFX, m_VFXContainer, _itemDefinition);
        }
        void SetupCoin(CorePrizeBoxItem _boxItem, InventoryItemDefinition _itemDefinition)
        {
            _boxItem.Setup(PrizeManager.Instance.m_CoinUnlockFX, m_VFXContainer, _itemDefinition);
        }
    }
}