using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Core;
using UnityEngine.Events;
using System.Linq;
using DG.Tweening;
using GameCore.Controllers;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace GameCore.Managers
{
    public class ThemeManager : SingletonComponent<ThemeManager>
    {
        #region Events
        public UnityAction<Theme> onThemeSelected;
        public UnityAction<Theme> onThemeChanged;
        #endregion
        [FoldoutGroup("Components")]
        public Camera m_BGCamera;
        [FoldoutGroup("Components")]
        public GameObject m_TopGradient;
        [FoldoutGroup("Components")]
        public GameObject m_CenterGradient;
        [FoldoutGroup("Components")]
        public GameObject m_BottomGradient;
        protected override void Awake()
        {
            base.Awake();
            //PlayerController.Instance.onPlayerColorChange +=SetColors;
        }
        #region Get Values
        public Theme GetFirst()
        {
            return ThemeDB.Instance.m_List.First();
        }
        public Theme GetLast()
        {
            return ThemeDB.Instance.m_List.Last();
        }
        public Theme GetTheme(string id)
        {
            Theme _theme = ThemeDB.Instance.m_List.FirstOrDefault(x => x.m_Id == id);
            return _theme;
        }
        public Theme GetTheme(Theme theme)
        {
            Theme _theme = ThemeDB.Instance.m_List.FirstOrDefault(x => x == theme);
            return _theme;
        }
        #endregion
        public void SelectTheme(string id)
        {
            Theme _theme = GetTheme(id);
            if (_theme == null)
            {
                Debug.LogError("Theme Not Found : " + id);
                return;
            }
            else
            {
                ApplyThemeProperties(_theme);
            }
            
        }
        void ApplyThemeProperties(Theme _theme)
        {
            m_BGCamera.backgroundColor = _theme.m_BGColor;
            RenderSettings.fogColor = _theme.m_FogColor;
            RenderSettings.fogStartDistance = _theme.m_FogLimits.x;
            RenderSettings.fogEndDistance = _theme.m_FogLimits.y;
            RenderSettings.ambientLight = _theme.m_AmbientColor;
            m_BottomGradient.SetActive(false);
            m_TopGradient.SetActive(false);
            m_CenterGradient.SetActive(false);
            switch (_theme.m_RadialGradient)
            {
                case Theme.RadialGradientType.Top:
                    m_TopGradient.SetActive(true);
                    m_TopGradient.GetComponent<SpriteRenderer>().color = _theme.m_RadialGradientColor;
                    break;
                case Theme.RadialGradientType.Center:
                    m_CenterGradient.SetActive(true);
                    m_CenterGradient.GetComponent<SpriteRenderer>().color = _theme.m_RadialGradientColor;
                    break;
                case Theme.RadialGradientType.Bottom:
                    m_BottomGradient.SetActive(true);
                    m_BottomGradient.GetComponent<SpriteRenderer>().color = _theme.m_RadialGradientColor;
                    break;
            }
        }
    }
}
