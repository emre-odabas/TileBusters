using GameCore.Core;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleTileList", menuName = "GameCore/Create/PuzzleTileDataList")]
public class PuzzleTileDataList : SingletonScriptableObject<PuzzleTileDataList>
{
    public List<PuzzleTileData> m_TileDataList = new List<PuzzleTileData>();
}
