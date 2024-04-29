#if UNITY_EDITOR
using Hyperlab.Gameplay.Editor;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace GameCore.Editor
{
    public class LevelCreator : OdinMenuEditorWindow
    {
        [MenuItem("Tools/GameCore/Level Creator")]
        private static void OpenWindow()
        {
            GetWindow<LevelCreator>().Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;
            tree.DefaultMenuStyle.Borders = true;
            tree.DefaultMenuStyle.BorderAlpha = 1;
            tree.DefaultMenuStyle.Height = 50;
            tree.DefaultMenuStyle.IconSize = 35;
            
            tree.Add("Town Creator", new LCTab_Town(), Resources.Load<Sprite>("LCTabIcon_Town"));
            //tree.Add("Character Skin Creator", new GP_ACTabCharacterSkin(), Resources.Load<Sprite>("ACTabIcon_Character"));
            return tree;
        }

        
    }
    
    
}

#endif