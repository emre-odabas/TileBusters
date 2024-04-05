using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.GameFoundation;

namespace GameCore.Core
{
	[System.Serializable]
	public class CoreShopItem
	{
		public InventoryItemDefinition m_InventoryDefinition;
		public BaseTransaction m_Transaction;
		public int GetAmount()
		{
			return InventoryManager.FindItemsByDefinition(m_InventoryDefinition, null);
		}
	}

}