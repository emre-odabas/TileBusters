using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using GameCore.Controllers;
using GameCore.Managers;
namespace GameCore.Core
{
    public enum Direction
    {
        Forward,
        Left,
        Right
    }

    [System.Serializable]
    [HideLabel]
    [CreateAssetMenu(fileName = "New Town", menuName = "GameCore/Create/Town", order = 1)]
   
    public class Town : ScriptableObject
    {
        public GameObject m_Platform;

/*#if UNITY_EDITOR
        [ValueDropdown("GetThemes")]
#endif
        public string m_Theme;
#if UNITY_EDITOR
        IEnumerable GetThemes()
        {
            List<string> _themes = new List<string>();
            foreach(var _theme in ThemeDB.Instance.m_List)
            {
                _themes.Add(_theme.m_Id);
            }
            return _themes;
        }
#endif*/

    }
}