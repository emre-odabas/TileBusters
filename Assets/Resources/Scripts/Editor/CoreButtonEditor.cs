using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using GameCore.Core.UI;
[CustomEditor(typeof(CoreButton))]
public class CoreButtonEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        CoreButton targetMyButton = (CoreButton)target;
        var _clickFeedback = serializedObject.FindProperty("m_ClickFeedback");
        EditorGUILayout.PropertyField(_clickFeedback, new GUIContent("Click Feedback"), true);
        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}
