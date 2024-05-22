using GameCore.Managers;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class UIElementController : MonoBehaviour
{
    #region FIELDS

    //Parameters
    [FoldoutGroup("Parameters"), SerializeField] private List<string> m_ShowEvents = new List<string>();

    //Components
    [FoldoutGroup("Components")]
    [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_ShowFeedbacks;
    [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_HideFeedbacks;

    //Privates
    private bool _isShowing = true;

    #endregion

    #region MONOBEHAVIOUR

    private void Start()
    {

    }

    private void OnEnable()
    {
        _isShowing = true;
        GameManager.Instance.onStateChange += OnStateChange;
    }
    private void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.onStateChange -= OnStateChange;
    }

    #endregion

    #region FUNCTIONS

    private void Show(bool force = false)
    {
        if (!force && _isShowing) return;
        _isShowing = true;
        m_ShowFeedbacks.PlayFeedbacks();
    }

    private void Hide(bool force = false)
    {
        if (!force && !_isShowing) return;
        _isShowing = false;
        m_HideFeedbacks.PlayFeedbacks();
    }

    #endregion

    #region RECALL FUNCTIONS

    private void OnStateChange()
    {
        if (m_ShowEvents.Contains(GameManager.Instance.m_State.ToString()))
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    #endregion
}
