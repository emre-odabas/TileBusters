using GameCore.Core.UI;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.GameFoundation;
using Sirenix.OdinInspector;
using GameCore.Core;
using GameCore.Managers;

namespace GameCore.UI
{
    public class ShopCarousel : CoreCarousel<ShopCarouselItem, CoreShopItem>
    {
        public override void Setup(List<CoreShopItem> _items)
        {
            base.Setup(_items);
            
            foreach (var _item in m_CarouselItems)
            {
                _item.m_Item.Setup();
                if (_item.m_Data.m_Transaction.key == DataManager.Instance.m_GameData.m_SeletedItem)
                {
                    _item.m_Item.SetSelected();
                }
                else if (_item.m_Data.GetAmount() > 0)
                {
                    _item.m_Item.SetUnlocked();
                }
                else
                {
                    _item.m_Item.SetLocked();
                }
            }
        }
        #region Item Controls
        public virtual void UnlockItem(ShopCarouselItem _item)
        {
            _item.Unlock(null);
        }
        public virtual void SelectItem(ShopCarouselItem _item)
        {
            _item.Select(null);
        }
        #endregion
        #region Events
        protected override void OnClickButtonCarouselItem(ShopCarouselItem _item)
        {
            base.OnClickButtonCarouselItem(_item);
        } 
        #endregion
    }
}
