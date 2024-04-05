using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore.Managers;
namespace GameCore.UI
{
    public class TutorialBar : MonoBehaviour
    {
        public List<RectTransform> m_Steps;
        int m_CurrentStep = 0;
       
        public void Show()
        {
            m_CurrentStep = 0;
            PlayStep(m_CurrentStep);
        }

        public void Hide()
        {
            HideAllSteps();
        }
        public void HideAllSteps()
        {
            foreach(var step in m_Steps)
            {
                step.gameObject.SetActive(false);
            }
        }
        public void PlayNextStep()
        {
            HideAllSteps();
            m_CurrentStep++;
            if (m_CurrentStep >= m_Steps.Count)
            {
                Hide();
            }
            else
            {
                PlayStep(m_CurrentStep);
            }
        }
        void PlayStep(int index)
        {
            HideAllSteps();
            m_Steps[index].gameObject.SetActive(true);
        }
    } 
}
