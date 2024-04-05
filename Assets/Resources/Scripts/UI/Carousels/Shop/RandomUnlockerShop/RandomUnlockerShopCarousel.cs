using GameCore.Core.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.GameFoundation;

namespace GameCore.UI
{
	public class RandomUnlockerShopCarousel : ShopCarousel
	{
        #region Item Controls
        public override void UnlockItem(ShopCarouselItem _item)
        {
            base.UnlockItem(_item);
        }
        public override void SelectItem(ShopCarouselItem _item)
        {
            base.SelectItem(_item);
        }
        #endregion
        #region Events
        protected override void OnClickButtonCarouselItem(ShopCarouselItem _item)
        {
            base.OnClickButtonCarouselItem(_item);
            switch (_item.m_State)
            {
                case ShopCarouselItem.State.Unlocked:
                    SelectItem(_item);
                    break;
            }
        }
        #endregion
    }
}
