
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
                {"Towns", new TownListEditor(), EditorIcons.List},
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
                Object _db = TownDB.Instance;
                if (_db != null)
                    Selection.activeObject = _db;
                else
                {
                    TownDB _newdb = new TownDB();
                    AssetDatabase.CreateAsset(_newdb, Application.dataPath + "/Resources/Data/LevelDB.asset");
                    AssetDatabase.SaveAssets();
                }
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
        [MenuItem("Tools/GameCore/Create/Town")]
        private static void CreateTown()
        {
            if (!Application.isPlaying)
            {
                if (!System.IO.Directory.Exists(Application.dataPath + "/Resources/Data"))
                {
                    System.IO.Directory.CreateDirectory(Application.dataPath + "/Resources/Data");
                }
                Town _town = new Town();
                UnityEditor.AssetDatabase.CreateAsset(_town, ProjectSettings.Instance.m_TownsPath + "/New Town.asset");
                UnityEditor.AssetDatabase.SaveAssets();
                if (_town != null)
                    Selection.activeObject = _town;
            }
        }
        
        #endregion
    }
    public class TownListEditor
    {
        [TableList(AlwaysExpanded = true, NumberOfItemsPerPage = 25, ShowPaging = true, ShowIndexLabels = true)]
        public List<Town> m_Towns;
        public TownListEditor()
        {
            m_Towns = TownDB.Instance.m_List;
        }
    }
    
#endif
}