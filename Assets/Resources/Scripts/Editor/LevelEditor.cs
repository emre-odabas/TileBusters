
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Linq;
using UnityEngine;
using Sirenix.Utilities.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using GameCore.Controllers;
using GameCore.Managers;
using GameCore.Core;

namespace GameCore.Editor
{
#if UNITY_EDITOR
    public class LevelEditor : OdinMenuEditorWindow
    {
        [MenuItem("Tools/GameCore/Core", priority = 0)]
        private static void OpenWindow()
        {
            var window = GetWindow<LevelEditor>();

            // Nifty little trick to quickly position the window in the middle of the editor.
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1600, 700);
        }
        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: false)
            {
                {"Levels", new LevelListEditor(), EditorIcons.List},
                {"Themes", new ThemeListEditor(), EditorIcons.EyeDropper},
                //{"Skins", new SkinListEditor(), EditorIcons.Podium},
                //{"Prize - Skin Packs", new SkinPrizeListEditor(), EditorIcons.ShoppingBasket},
                //{"Prize - Coin Packs", new CoinPrizeListEditor(), EditorIcons.ShoppingCart},
                {"ProjectSettings", ProjectSettings.Instance, EditorIcons.Globe}
            };
            return tree;
        }

        #region Selections
        [MenuItem("Tools/GameCore/DB/LevelDB", priority = 0)]
        private static void SelectLevelDB()
        {
            if (!Application.isPlaying)
            {
                if (!System.IO.Directory.Exists(Application.dataPath + "/Resources/Data"))
                {
                    System.IO.Directory.CreateDirectory(Application.dataPath + "/Resources/Data");
                }
                Object _db = LevelDB.Instance;
                if (_db != null)
                    Selection.activeObject = _db;
                else
                {
                    LevelDB _newdb = new LevelDB();
                    AssetDatabase.CreateAsset(_newdb, Application.dataPath + "/Resources/Data/LevelDB.asset");
                    AssetDatabase.SaveAssets();
                }
            }
        }
        [MenuItem("Tools/GameCore/DB/ThemeDB", priority = 2)]
        private static void SelectThemeDB()
        {
            if (!Application.isPlaying)
            {
                if (!System.IO.Directory.Exists(Application.dataPath + "/Resources/Data"))
                {
                    System.IO.Directory.CreateDirectory(Application.dataPath + "/Resources/Data");
                }
                Object _db = ThemeDB.Instance;
                Debug.Log(ThemeDB.Instance);
                if (_db != null)
                    Selection.activeObject = _db;
                else
                {
                    ThemeDB _newdb = new ThemeDB();
                    AssetDatabase.CreateAsset(_newdb, Application.dataPath + "/Resources/Data/ThemeDB.asset");
                    AssetDatabase.SaveAssets();
                }
                EditorUtility.FocusProjectWindow();
            }
        }
        [MenuItem("Tools/GameCore/ProjectSettings", priority = 4)]
        private static void SelectProjectSettings()
        {
            if (!Application.isPlaying)
            {
                if (!System.IO.Directory.Exists(Application.dataPath + "/Resources/Data"))
                {
                    System.IO.Directory.CreateDirectory(Application.dataPath + "/Resources/Data");
                }
                Object _db = ProjectSettings.Instance;
                if (_db != null)
                    Selection.activeObject = _db;
                else
                {
                    ProjectSettings _newdb = new ProjectSettings();
                    AssetDatabase.CreateAsset(_newdb, Application.dataPath + "/Resources/Data/ProjectSettings.asset");
                    AssetDatabase.SaveAssets();
                }
            }
            EditorUtility.FocusProjectWindow();
        }
        #endregion
        #region New Object Creating
        [MenuItem("Tools/GameCore/Create/Level")]
        private static void CreateLevel()
        {
            if (!Application.isPlaying)
            {
                if (!System.IO.Directory.Exists(Application.dataPath + "/Resources/Data"))
                {
                    System.IO.Directory.CreateDirectory(Application.dataPath + "/Resources/Data");
                }
                Level _level = new Level();
                UnityEditor.AssetDatabase.CreateAsset(_level, ProjectSettings.Instance.m_LevelsPath + "/New Level.asset");
                UnityEditor.AssetDatabase.SaveAssets();
                if (_level != null)
                    Selection.activeObject = _level;
            }
        }
        [MenuItem("Tools/GameCore/Create/Theme")]
        private static void CreateTheme()
        {
            if (!Application.isPlaying)
            {
                if (!System.IO.Directory.Exists(Application.dataPath + "/Resources/Data"))
                {
                    System.IO.Directory.CreateDirectory(Application.dataPath + "/Resources/Data");
                }
                Theme _theme = new Theme();
                UnityEditor.AssetDatabase.CreateAsset(_theme, ProjectSettings.Instance.m_ThemesPath + "/New Theme.asset");
                UnityEditor.AssetDatabase.SaveAssets();
                if (_theme != null)
                    Selection.activeObject = _theme;
            }
        }
        #endregion
    }
    public class LevelListEditor
    {
        [TableList(AlwaysExpanded = true, NumberOfItemsPerPage = 25, ShowPaging = true, ShowIndexLabels = true)]
        public List<Level> m_Levels;
        public LevelListEditor()
        {
            m_Levels = LevelDB.Instance.m_List;
        }
    }
    public class ThemeListEditor
    {
        [TableList(AlwaysExpanded = true, NumberOfItemsPerPage = 25, ShowPaging = true, ShowIndexLabels = true)]
        public List<Theme> m_Themes;
        public ThemeListEditor()
        {
            m_Themes = ThemeDB.Instance.m_List;
        }
    }
#endif
}