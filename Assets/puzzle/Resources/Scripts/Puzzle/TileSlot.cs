using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

public class TileSlot : MonoBehaviour
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
    [FoldoutGroup("Indicator"), SerializeField, ReadOnly] private TileCell m_TileCell;

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

    public TileCell GetTile()
    {
        return m_TileCell;
    }

    #endregion

    #region FUNCTIONS

    public void EmptyIt()
    {
        m_TileCell = null;
        m_EmptyItFeedbacks.PlayFeedbacks();
    }

    public void Filled(TileCell tile)
    {
        m_TileCell = tile;
        tile.transform.SetParent(m_ItemContainer);
        tile.onMatch += OnChildTileMatch;
        m_FilledFeedbacks.PlayFeedbacks();
    }

    #endregion

    #region CALLBACKS

    private void OnChildTileMatch()
    {
        EmptyIt();
    }

    #endregion

}
