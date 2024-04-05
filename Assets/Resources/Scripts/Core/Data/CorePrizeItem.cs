using UnityEngine;
using System.Collections;
using GameCore.Core.UI;
namespace GameCore.Core
{
	[System.Serializable]
	public class CorePrizeItem
	{
		public int m_PrizeUnlockLevel;
		public int m_Piece;
		protected virtual void Unlock() { }
	}

}