using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Core
{
	[System.Serializable]
	//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
	public class ThemeDB : SingletonScriptableObject<ThemeDB>
	{
		public List<Theme> m_List = new List<Theme>();
	} 
}
