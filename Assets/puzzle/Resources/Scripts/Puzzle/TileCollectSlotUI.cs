using GameCore.Core;
using GameCore.Gameplay;
using GameCore.Managers;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TileCollectSlotUI : MonoBehaviour
{
    #region UTILITIES

    #endregion

    #region FIELDS

    //Parameters
    //[FoldoutGroup("Parameters"), SerializeField] private

    //Components
    [FoldoutGroup("Components")]
    [FoldoutGroup("Components/Utilities"), SerializeField] private RectTransform m_ItemContainer;
    [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_FilledFeedbacks;
    [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_EmptyItFeedbacks;

    //Indicator
    [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private bool m_isFull;

    //Privates


    #endregion

    #region MONOBEHAVIOUR


    private void Start()
    {
        EmptyIt();
    }

    #endregion

    #region CALLBACKS

    

    #endregion

    #region RETURN FUNCTIONS

    public bool IsFull() => m_isFull;

    #endregion

    #region FUNCTIONS

    private void EmptyIt()
    {
        m_isFull = false;
        m_EmptyItFeedbacks.PlayFeedbacks();
    }

    public void Filled(TileCell tile)
    {
        m_isFull = true;
        tile.transform.SetParent(m_ItemContainer);
        tile.transform.localPosition = Vector3.zero;
        m_FilledFeedbacks.PlayFeedbacks();
    }

    #endregion

}
