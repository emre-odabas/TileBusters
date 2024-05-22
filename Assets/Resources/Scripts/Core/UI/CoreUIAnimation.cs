using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GameCore.Managers;
namespace GameCore.Core.UI
{
    [RequireComponent(typeof(DOTweenAnimation))]
    [RequireComponent(typeof(DOTweenVisualManager))]
    public class CoreUIAnimation : MonoBehaviour
    {
        //public GameManager.State m_TriggerState;
        public List<string> m_ShowEvents = new List<string>();
        public List<string> m_HideEvents = new List<string>();

        DOTweenAnimation[] m_DGAnimations;
        private void Awake()
        {
            m_DGAnimations = GetComponents<DOTweenAnimation>();
            PlayAnimations();
            RewindAnimations();
        }
        private void OnEnable()
        {
            PlayAnimations();

            Utilities.DelayedCall(7, ()=> RewindAnimations());
        }

        private void OnDisable()
        {            
            RewindAnimations();
        }

        void PlayAnimations()
        {
            foreach(var animation in m_DGAnimations)
            {
                animation.DOPlay();
            }
        }

        void RewindAnimations()
        {
            foreach (var animation in m_DGAnimations)
            {
                animation.DORewind();
            }
        }
    } 
}
