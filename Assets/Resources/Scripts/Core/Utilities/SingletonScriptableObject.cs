using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace GameCore.Core
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>, new()
    {

        static T _instance = null;
        static Object[] m_Scriptables = new Object[0];
        public static T Instance
        {
            get
            {

#if UNITY_EDITOR
                if (_instance == null)
                {

                    if (m_Scriptables.Length == 0)
                        m_Scriptables = Resources.LoadAll("Data");
                    //GetAsset();
                    //_instance = (T)AssetDatabase.LoadAssetAtPath(GetAsset(), typeof(T));
                    _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                }
#else
                if (_instance == null){
                    if (m_Scriptables.Length == 0)
                        m_Scriptables = Resources.LoadAll("Data");
                    _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                }
#endif


                return _instance;
            }
        }

#if UNITY_EDITOR
        static string GetAsset()
        {
            string[] _guids = AssetDatabase.FindAssets("t:" + typeof(T).ToString());
            _instance = ScriptableObject.CreateInstance<T>();
            if (_guids.Length == 0)
            {
                if (!Application.isPlaying)
                {
                    if (!System.IO.Directory.Exists(Application.dataPath + "/Resources/Data"))
                    {
                        System.IO.Directory.CreateDirectory(Application.dataPath + "/Resources/Data");
                    }

                    UnityEditor.AssetDatabase.CreateAsset(_instance, "Assets/Resources/Data/" + typeof(T).ToString() + ".asset");
                    UnityEditor.AssetDatabase.SaveAssets();
                }
                return "Assets/Resources/Data/" + typeof(T).ToString() + ".asset";
            }
            return AssetDatabase.GUIDToAssetPath(_guids[0]);
        }
#endif
    }


    //    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    //    {
    //        #region Member Variables

    //        private static T instance;

    //        #endregion

    //        #region Properties

    //        public static T Instance
    //        {
    //            get
    //            {
    //                if (instance == null)
    //                {
    //                    GetInstance(typeof(T).Name);
    //                }

    //                return instance;
    //            }
    //        }

    //        #endregion

    //        #region Private Methods

    //        private static void GetInstance(string name)
    //        {
    //            instance = Resources.Load<T>(name);
    //            Debug.Log("Name = " + name);
    //            Debug.Log("Instance = " + instance);
    //            if (instance != null)
    //            {
    //                return;
    //            }

    //            Debug.Log("Data/" + name + ".asset");

    //            instance = ScriptableObject.CreateInstance<T>();

    //#if UNITY_EDITOR
    //            if (!Application.isPlaying)
    //            {
    //                if (!System.IO.Directory.Exists(Application.dataPath + "/Resources/Data"))
    //                {
    //                    System.IO.Directory.CreateDirectory(Application.dataPath + "/Resources/Data");
    //                }

    //                UnityEditor.AssetDatabase.CreateAsset(instance, "Assets/Resources/Data/" + name + ".asset");
    //                UnityEditor.AssetDatabase.SaveAssets();
    //            }
    //#endif
    //        }

    //        #endregion
    //    }
}
