using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleTile", menuName = "GameCore/Create/PuzzleTileData")]
public class PuzzleTileData : ScriptableObject
{
    public string m_Id
        ;
    public Sprite m_TileSprite;
}
