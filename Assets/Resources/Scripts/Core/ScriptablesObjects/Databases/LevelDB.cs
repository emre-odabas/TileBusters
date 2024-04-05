using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Core
{
	[System.Serializable]
	public class LevelDB : SingletonScriptableObject<LevelDB>
	{
		public List<Level> m_List = new List<Level>();
	} 
}
