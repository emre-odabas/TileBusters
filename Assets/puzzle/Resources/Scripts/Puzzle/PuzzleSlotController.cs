using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using GameCore.Core;
using GameCore.Managers;
using System.Linq;
using System;

public class PuzzleSlotController : SingletonComponent<PuzzleSlotController>
{
    #region FIELDS

    //Parameters
    //[FoldoutGroup("Parameters"), SerializeField] private 

    //Components
    [FoldoutGroup("Components")]
    //[FoldoutGroup("Components/Utilities"), SerializeField] private 
    [FoldoutGroup("Components/Lists"), SerializeField] private List<TileSlot> m_Slots = new List<TileSlot>();

    //Indicator
    //[FoldoutGroup("Indicator"), SerializeField, ReadOnly] private

    //Privates

    #endregion

    #region MONOBEHAVIOUR

    private void OnEnable()
    {
        PuzzleController.Instance.onTileCollect += OnTileCollect;
    }

    private void OnDisable()
    {
        if(PuzzleController.Instance != null)
        {
            PuzzleController.Instance.onTileCollect -= OnTileCollect;
        }
    }

    #endregion

    #region RETURN FUNCTIONS

    public int SlotCount()
    {
        return m_Slots.Count;
    }

    public TileSlot GetFirstEmptySlot()
    {
        TileSlot slot = m_Slots.FirstOrDefault(c => c.GetTile() == null);
        return slot;
    }

    public bool AllSlotsFull()
    {
        for (int i = 0; i < m_Slots.Count; i++)
        {
            if (m_Slots[i].GetTile() == null)
            {
                return false;
            }
        }
        return true;
    }

    #endregion

    #region CALLBACKS

    private void OnTileCollect(TileCell tileCell)
    {
        GetFirstEmptySlot().Filled(tileCell);
        tileCell.OnCollect();

        CheckMatch();

        if (AllSlotsFull())
        {
            Debug.LogError("Puzzle game over!");
        }
    }

    #endregion

    #region FUNCTIONS

    private void CheckMatch()
    {
        int requiredMatchCount = 3;
        for (int i = 0; i < m_Slots.Count; i++)
        {
            if (m_Slots[i].GetTile() == null) continue;

            List<TileCell> tempTileList = new List<TileCell>();
            string checkedId = m_Slots[i].GetTile().m_Id;

            foreach (TileSlot slot in m_Slots)
            {
                if (slot.GetTile() == null) continue;

                if (slot.GetTile().m_Id == checkedId)
                    tempTileList.Add(slot.GetTile());

            }

            //Match!
            if (tempTileList.Count >= requiredMatchCount)
            {
                foreach (TileCell tileCell in tempTileList)
                    tileCell.OnMatch();
            }
            tempTileList.Clear();
        }
    }

    #endregion

}
