using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Managers;
using UnityEngine.GameFoundation;
using GameCore.Core;

namespace GameCore.UI
{
    public class KeyBar : MonoBehaviour
    {
        public List<Transform> m_Keys = new List<Transform>();

        private void Start()
        {
            //gameObject.SetActive(false);
        }
        public virtual void UpdateKeys(int _unlockedKeys)
        {
            HideAllKey();
            for(int i = 0; i < _unlockedKeys; i++)
            {
                ShowKey(i);
            }
        }
        public virtual void ShowKey(int _index)
        {
            m_Keys[_index].gameObject.SetActive(true);
        }
        public virtual void HideKey(int _index)
        {
            m_Keys[_index].gameObject.SetActive(false);
        }
        public virtual void HideAllKey()
        {
            for(int i = 0; i < m_Keys.Count; i++)
            {
                HideKey(i);
            }
        }
        public virtual void ShowAllKey()
        {
            for (int i = 0; i < m_Keys.Count; i++)
            {
                ShowKey(i);
            }
        }
    } 
}
