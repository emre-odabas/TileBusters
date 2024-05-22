using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace GameCore.Core
{
	[System.Serializable]	
	public class ProjectSettings : SingletonScriptableObject<ProjectSettings>
	{
		[FoldoutGroup("Paths", expanded: true), FolderPath]public string m_TownsPath;
		[FoldoutGroup("Databases", expanded: true)] public TownDataList m_TownDB;
		[FoldoutGroup("Object ShortCuts", expanded: true)] public GameObject m_GameObjectWithContainer;
		[FoldoutGroup("Object ShortCuts")] public GameObject m_UIObjectWithContainer;
		[FoldoutGroup("Object ShortCuts")] public GameObject m_UIObjectWithAnimatedContainer;
		[FoldoutGroup("Object ShortCuts")] public GameObject m_ButtonPub;
		[FoldoutGroup("Object ShortCuts")] public GameObject m_ButtonRounded_1;
		[FoldoutGroup("Object ShortCuts")] public GameObject m_ButtonOutlinedRounded_1;
		[FoldoutGroup("Object ShortCuts")] public GameObject m_ButtonRounded_2;
		[FoldoutGroup("Object ShortCuts")] public GameObject m_ButtonOutlinedRounded_2;
		[FoldoutGroup("Haptic Sound", expanded :true)] public AudioClip m_SelectionSound;
		[FoldoutGroup("Haptic Sound")] public AudioClip m_HeavySound;
		[FoldoutGroup("Haptic Sound")]public AudioClip m_MediumSound;
		[FoldoutGroup("Haptic Sound")]public AudioClip m_LightSound;
		[FoldoutGroup("Haptic Sound")]public AudioClip m_RigidSound;
		[FoldoutGroup("Haptic Sound")]public AudioClip m_SoftSound;
		[FoldoutGroup("Haptic Sound")]public AudioClip m_SuccessSound;
		[FoldoutGroup("Haptic Sound")]public AudioClip m_WarningSound;
		[FoldoutGroup("Haptic Sound")]public AudioClip m_FailSound;
	} 
}
