using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace GameCore.Core.UI
{
    [RequireComponent(typeof(DOTweenAnimation))]
    [RequireComponent(typeof(DOTweenVisualManager))]
    public class CoreUIAnimation : MonoBehaviour
    {
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
