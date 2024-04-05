using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GameCore.Core.UI
{
    [RequireComponent(typeof(CoreButton))]
    public class CoreCarouselPaginationItem : MonoBehaviour
    {
        public Transform m_UnSelectedTransform;
        public Transform m_SelectedTransform;
        [HideInInspector]
        public CoreButton m_Button;
        protected virtual void Awake()
        {
            m_Button = GetComponent<CoreButton>();
        }
        public virtual void Select()
        {
            m_SelectedTransform.gameObject.SetActive(true);
            m_UnSelectedTransform.gameObject.SetActive(false);
        }

        public virtual void UnSelect()
        {
            m_SelectedTransform.gameObject.SetActive(false);
            m_UnSelectedTransform.gameObject.SetActive(true);
        }
    } 
}
