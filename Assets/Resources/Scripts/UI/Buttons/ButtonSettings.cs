using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class ButtonSettings : MonoBehaviour
{
    public enum State
    {
        Shown = 0,
        Hidden = 1
    };
    public State m_State = State.Hidden;
    public RectTransform m_Container;
    public float m_ShownHeight;
    public float m_HiddenHeight;
    public void Start()
    {
        if (m_State == State.Hidden) Hide();
        else Show();
    }
    public void OnClickBtn()
    {
        if (m_State == State.Hidden) Show();
        else Hide();
    }
    public virtual void Show()
    {
        m_State = State.Shown;
        DOVirtual.Float(m_HiddenHeight, m_ShownHeight, 0.2f,(y)=>{
            m_Container.sizeDelta = new Vector2(m_Container.sizeDelta.x, y);
        });
    }
    public virtual void Hide()
    {
        m_State = State.Hidden;
        DOVirtual.Float(m_ShownHeight, m_HiddenHeight, 0.2f, (y) => {
            m_Container.sizeDelta = new Vector2(m_Container.sizeDelta.x, y);
        });
    }
}
