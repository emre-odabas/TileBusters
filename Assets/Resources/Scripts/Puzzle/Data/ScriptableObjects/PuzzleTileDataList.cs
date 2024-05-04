using GameCore.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[CreateAssetMenu(fileName = "PuzzleTileList", menuName = "GameCore/Create/PuzzleTileDataList")]
public class PuzzleTileDataList : SingletonScriptableObject<PuzzleTileDataList>
{
    public List<PuzzleTileData> m_TileDataList = new List<PuzzleTileData>();

    public PuzzleTileData GetPuzzleTileData(string id)
    {
        return m_TileDataList.FirstOrDefault(x => x.m_Id == id);
    }
}
