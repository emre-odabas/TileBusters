using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleLevelData", menuName = "GameCore/Create/PuzzleLevelData")]
public class PuzzleLevelData : ScriptableObject
{
    public TileLayer[] TileLayers;
    public ushort DifferentIdCount;
}

[System.Serializable]
public class TileLayer
{
    public Tile[] Tiles;
    public ushort ColCountX;
    public ushort RowCountY;
}

[System.Serializable]
public class Tile
{
    public string Id;
    public ushort RowY;
    public ushort ColX;
}
