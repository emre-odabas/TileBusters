using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Core;
using System.Linq;
using System;

namespace GameCore.Managers
{
    public class TutorialManager : SingletonComponent<TutorialManager>
    {
        public List<TutorialStep> m_Steps;

        private void Start()
        {
            HideAll();
        }

        private void OnUndoMoves()
        {
            HideStep("Undo");
        }
        private void OnFirstDraw()
        {
            HideStep("Draw");
        }

        public void ShowStep(string _id)
        {
            HideAll();
            TutorialStep _step = m_Steps.FirstOrDefault(x => x.m_Id == _id);
            if (_step != null)
                _step.m_Object.SetActive(true);
        }
        public void HideAll()
        {
            foreach (var _step in m_Steps)
            {
                _step.m_Object.SetActive(false);
            }
        }
        public void HideStep(string _id)
        {
            TutorialStep _step = m_Steps.FirstOrDefault(x => x.m_Id == _id);
            if (_step != null)
                _step.m_Object.SetActive(false);
        }
    }
}
