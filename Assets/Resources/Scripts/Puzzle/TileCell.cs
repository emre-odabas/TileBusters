using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using System.Security.Cryptography;
using GameCore.Core;

public class TileCell : MonoBehaviour
{
    #region UTILITIES

    public UnityEvent onRemoveTile;
    public UnityAction onMatch;

    #endregion

    #region FIELDS

    //Parameters
    //[FoldoutGroup("Parameters"), SerializeField] private 

    //Components
    [FoldoutGroup("Components")]
    [FoldoutGroup("Components/Utilities"), SerializeField] private Image m_ItemImage;
    [FoldoutGroup("Components/Utilities"), SerializeField] private GameObject m_BlockedImage;
    [FoldoutGroup("Components/Utilities"), SerializeField] private BoxCollider2D boxCollider;
    [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_CollectFeedbacks;
    [FoldoutGroup("Components/Feedbacks"), SerializeField] private MMFeedbacks m_MatchFeedbacks;

    //Indicator
    [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private int blockCellCount = 0;
    [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private Tile tile;
    [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private bool m_isMatched = false;

    //Privates
    private Action<TileCell> onClickCallback;
    [HideInInspector] public string m_Id => tile.Id;

    #endregion

    #region MONOBEHAVIOUR

    void OnMouseDown()
    {
        Debug.Log("OnMouseDown " + this.gameObject.name);
        onClickCallback?.Invoke(this);
    }

    private void OnDestroy()
    {
        onRemoveTile.RemoveAllListeners();
    }

    #endregion

    #region RECALL FUNCTIONS

    public bool isMatched()
    {
        return m_isMatched;
    }

    #endregion

    #region FUNCTIONS

    public void SetData(Tile _tile, int _order, Action<TileCell> _callback)
    {
        tile = _tile;
        //tileSpriteRenderer.sortingOrder = _order;
        //iconSpriteRenderer.sortingOrder = _order;
        onClickCallback = _callback;
    }

    /*public void SetIcon(Sprite _sprite)
    {
        iconSpriteRenderer.sprite = _sprite;
    }

    public void SetId(string _id)
    {
        tile.Id = _id;
    }*/

    public void SetCustomize(PuzzleTileData tileData)
    {
        tile.Id = tileData.m_Id;
        m_ItemImage.sprite = tileData.m_TileSprite;
    }

    public void SetBlockState()
    {
        bool block = blockCellCount > 0;
        //tileSpriteRenderer.color = block ? Color.gray: Color.white;
        //m_ItemImage.color = block ? Color.gray: Color.white;
        m_BlockedImage.SetActive(block);
        TriggerEnable(!block);
    }

    public void TriggerEnable(bool _enable)
    {
        boxCollider.enabled = _enable;
    }

    public void BlockCellRemove()
    {
        blockCellCount -= 1;
        if (blockCellCount <= 0)
        {
            TriggerEnable(true);
            m_BlockedImage.SetActive(false);
            //tileSpriteRenderer.color = Color.white;
            //m_ItemImage.color = Color.white;
        }
    }

    public void OnCollect()
    {
        m_CollectFeedbacks.PlayFeedbacks();
    }

    public void OnMatch()
    {
        m_isMatched = true;
        onRemoveTile.RemoveAllListeners();
        onMatch?.Invoke();
        Utilities.DelayedCall(0.25f, () =>
        {
            m_MatchFeedbacks.PlayFeedbacks();    
        });
    }

    public void AddBlockCount()
    {
        blockCellCount += 1;
    }

    #endregion
}
