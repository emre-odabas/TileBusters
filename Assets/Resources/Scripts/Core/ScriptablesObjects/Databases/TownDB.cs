using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Core
{
	[System.Serializable]
	public class TownDB : SingletonScriptableObject<TownDB>
	{
		public List<Town> m_List = new List<Town>();
	} 
}
