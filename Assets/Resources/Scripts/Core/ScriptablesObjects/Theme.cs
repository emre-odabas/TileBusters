using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using GameCore.Managers;
#endif
namespace GameCore.Core
{
    [System.Serializable]
    public class Theme : ScriptableObject
    {
        public enum RadialGradientType
        {
            Empty,
            Top,
            Center,
            Bottom
        }
        
        public string m_Id;
        public RadialGradientType m_RadialGradient;
        public Color m_FogColor;
        public Vector2 m_FogLimits;
        public Color m_BGColor;
        public Color m_RadialGradientColor;
        public Color m_AmbientColor;


#if UNITY_EDITOR
        [Button("Get Curret Settings", ButtonHeight = 50),  HideInTables]
        
        public void SaveThemeSettings()
        {
            m_BGColor = ThemeManager.Instance.m_BGCamera.backgroundColor;
            m_FogColor = RenderSettings.fogColor;
            m_FogLimits.x = RenderSettings.fogStartDistance;
            m_FogLimits.y = RenderSettings.fogEndDistance;
            m_AmbientColor = RenderSettings.ambientLight;
            if (ThemeManager.Instance.m_TopGradient.activeInHierarchy)
            {
                m_RadialGradientColor = ThemeManager.Instance.m_TopGradient.GetComponent<SpriteRenderer>().color;
                m_RadialGradient = RadialGradientType.Top;
            }
            else if (ThemeManager.Instance.m_BottomGradient.activeInHierarchy)
            {
                m_RadialGradientColor = ThemeManager.Instance.m_BottomGradient.GetComponent<SpriteRenderer>().color;
                m_RadialGradient = RadialGradientType.Bottom;
            }
            else if (ThemeManager.Instance.m_CenterGradient.activeInHierarchy)
            {
                m_RadialGradientColor = ThemeManager.Instance.m_CenterGradient.GetComponent<SpriteRenderer>().color;
                m_RadialGradient = RadialGradientType.Center;
            }
            else
            {
                m_RadialGradient = RadialGradientType.Empty;
            }
        }
#endif
    }
}