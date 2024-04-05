using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using GameCore.Managers;
using MoreMountains.Feedbacks;

namespace GameCore.Core.UI
{
    public class CoreButton : Button
    {
        public MMFeedbacks m_ClickFeedback;
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            switch (state)
            {
                case SelectionState.Normal:
                    NormalState();
                    break;
                case SelectionState.Highlighted:
                    HighLightState();
                    break;
                case SelectionState.Pressed:
                    PressState();
                    break;
                case SelectionState.Selected:
                    SelectState();
                    break;
                case SelectionState.Disabled:
                    DisableState();
                    break;
            }
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (m_ClickFeedback != null)
            {
                m_ClickFeedback.PlayFeedbacks();
            }
        }
        
        void NormalState()
        {
            ApplyState(colors.normalColor);
        }
        void HighLightState()
        {
            ApplyState(colors.highlightedColor);
        }
        void PressState()
        {
            ApplyState(colors.pressedColor);
        }        
        void SelectState()
        {
            ApplyState(colors.selectedColor);
        }
        void DisableState()
        {
            ApplyState(colors.disabledColor);
        }
        void ApplyState(Color _color)
        {
            Image[] _graphics = GetComponentsInChildren<Image>();
            TextMeshProUGUI[] _texts = GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var _target in _graphics)
            {
                _target.CrossFadeColor(_color, (false) ? this.colors.fadeDuration : 0f, true, true);
            }
            foreach (var _target in _texts)
            {
                _target.CrossFadeColor(_color, (false) ? this.colors.fadeDuration : 0f, true, true);
            }
        }
    }
}
