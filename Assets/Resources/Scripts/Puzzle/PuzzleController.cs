using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using System.Linq;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.Events;
using GameCore.Core;
using GameCore.Managers;

public class PuzzleController : SingletonComponent<PuzzleController>
{
    #region UTILITIES

    public UnityAction<TileCell> onTileCollect;

    #endregion

    #region FIELDS

    //Parameters
    [FoldoutGroup("Parameters"), SerializeField] private Vector2 m_TilePlacementMargin = Vector2.zero;

    //Components
    [FoldoutGroup("Components")]
    [FoldoutGroup("Components/Utilities"), SerializeField] private PuzzleLevelData m_PuzzleLevelData;
    [FoldoutGroup("Components/Utilities"), SerializeField] private TileCell m_RefTileCell;
    [FoldoutGroup("Components/Utilities"), SerializeField] private Transform m_TilesContainer;

    //Indicator
    //[FoldoutGroup("Indicator"), SerializeField, ReadOnly] private

    //Privates
    private List<TileCell> m_TileCellList = new List<TileCell>();

    #endregion

    private void Start() 
    {
        //collectedTiles = new TileCell[PuzzleSlotController.Instance.SlotCount()];
        /*if (m_PuzzleLevelData != null && m_PuzzleLevelData.TileLayers != null)
        {
           createTiles();
        }*/  
    }

    public void SetupPuzzle()
    {
        if (m_PuzzleLevelData != null && m_PuzzleLevelData.TileLayers != null)
        {
            createTiles();
        }
    }

    private void setTilesCellData(Dictionary<(int, int, int), TileCell> _tileCellPairs)
    {
        var tileLayers = m_PuzzleLevelData.TileLayers;
        for(short layerIndex = 1; layerIndex < tileLayers.Length; layerIndex++)
        {
            var tilesLayerUp = tileLayers[layerIndex];

            var downLayerIndex = layerIndex - 1;
            var tilesLayerDown = tileLayers[downLayerIndex];

            int normalizeDiffX = tilesLayerDown.ColCountX - tilesLayerUp.ColCountX;
            bool coverTwoTileX = normalizeDiffX % 2 >= 0;
            int normalizeDiffY = tilesLayerDown.RowCountY - tilesLayerUp.RowCountY;
            bool coverTwoTileY = normalizeDiffY % 2 >= 0;

            normalizeDiffX = normalizeDiffX / 2;
            normalizeDiffY = normalizeDiffY / 2;

            Tile[] tiles = tilesLayerUp.Tiles;

            for(short tileIndex = 0; tileIndex < tiles.Length; tileIndex++)
            {
                Tile tile = tiles[tileIndex];
                int coverRowY = tile.RowY + normalizeDiffX;
                int coverColX = tile.ColX + normalizeDiffY;

                if(coverRowY > tilesLayerDown.ColCountX || coverColX > tilesLayerDown.RowCountY)
                    continue;

                TileCell upCell = _tileCellPairs[(layerIndex, tile.RowY, tile.ColX)];

                if(_tileCellPairs.ContainsKey((downLayerIndex, coverRowY, coverColX)))
                    registerEvent(upCell, _tileCellPairs[(downLayerIndex, coverRowY, coverColX)]);

                if(coverTwoTileX && coverTwoTileY)
                {
                    if(_tileCellPairs.ContainsKey((downLayerIndex, coverRowY + 1, coverColX)))
                        registerEvent(upCell, _tileCellPairs[(downLayerIndex, coverRowY + 1, coverColX)]);
                    if(_tileCellPairs.ContainsKey((downLayerIndex, coverRowY, coverColX + 1)))
                        registerEvent(upCell, _tileCellPairs[(downLayerIndex, coverRowY, coverColX + 1)]);
                    if(_tileCellPairs.ContainsKey((downLayerIndex, coverRowY + 1, coverColX + 1)))
                        registerEvent(upCell, _tileCellPairs[(downLayerIndex, coverRowY + 1, coverColX + 1)]);
                }
                else if(coverTwoTileX && _tileCellPairs.ContainsKey((downLayerIndex, coverRowY + 1, coverColX)))
                    registerEvent(upCell, _tileCellPairs[(downLayerIndex, coverRowY + 1, coverColX)]);
                else if(coverTwoTileY && _tileCellPairs.ContainsKey((downLayerIndex, coverRowY, coverColX + 1)))
                    registerEvent(upCell, _tileCellPairs[(downLayerIndex, coverRowY, coverColX + 1)]);
            }
        }

        foreach(var dic in _tileCellPairs)
        {
            dic.Value.SetBlockState();
        }
    }

    private void registerEvent(TileCell _upCell, TileCell _downCell)
    {
        _downCell.AddBlockCount();
        _upCell.onRemoveTile.AddListener(_downCell.BlockCellRemove);
    }

    void onTileClick(TileCell _cell)
    {
        _cell.TriggerEnable(false);
        putIntoCollect(_cell);
    }

    private List<(int, int)> getTileBlockIndex((ushort, ushort) buildTileRowCol, (ushort, ushort) upLayerRowCol, (ushort, ushort) downLayerRowCol)
    {
        int normalizeDiffX = downLayerRowCol.Item1 - upLayerRowCol.Item1;
        bool coverTwoTileX = normalizeDiffX % 2 > 0;
        int normalizeDiffY = downLayerRowCol.Item2 - upLayerRowCol.Item2;
        bool coverTwoTileY = normalizeDiffY % 2 > 0;

        normalizeDiffX = normalizeDiffX / 2;
        normalizeDiffY = normalizeDiffY / 2;
        //

        int coverRowY = buildTileRowCol.Item1 + normalizeDiffX;
        int coverColX = buildTileRowCol.Item2 + normalizeDiffY;
    #if UNITY_EDITOR
        Debug.LogFormat("checkObjectBlock target({0}, {1}), downLayer({2}, {3}), upLayer({4}, {5}), cover({6}, {7}), coverTwoTileX: {8}, coverTwoTileY: {9}."
            , buildTileRowCol.Item1, buildTileRowCol.Item2, downLayerRowCol.Item1, downLayerRowCol.Item2
            , upLayerRowCol.Item1, upLayerRowCol.Item2, coverRowY, coverColX, coverTwoTileX, coverTwoTileY);
    #endif

        List<(int, int)> blockIndexList = null;
        if(coverRowY > downLayerRowCol.Item1 || coverColX > downLayerRowCol.Item2)
            return blockIndexList;

        blockIndexList = new List<(int, int)>() {(coverRowY, coverColX)};

        if(coverTwoTileX && coverTwoTileY)
        {
            blockIndexList.Add((coverRowY + 1, coverColX));
            blockIndexList.Add((coverRowY, coverColX + 1));
            blockIndexList.Add((coverRowY + 1, coverColX + 1));
        }
        else if(coverTwoTileX)
            blockIndexList.Add((coverRowY + 1, coverColX));
        else if(coverTwoTileY)
            blockIndexList.Add((coverRowY, coverColX + 1));

        return blockIndexList;
    }

    public static void Shuffle<T>(List<T> list)
    {
        var rng = new System.Random();
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /////////////////////////////////////////////
    
    //Return Functions
    public bool GetIsSolved()
    {
        for (int i = 0; i < m_TileCellList.Count; i++)
        {
            if (!m_TileCellList[i].isMatched())
            {
                return false;
            }
        }
        return true;
    }


    //Functions

    private void ClearTiles()
    {
        for(int i = 0; i < m_TileCellList.Count; i++)
        {
            Destroy(m_TileCellList[i].gameObject);
        }
        m_TileCellList.Clear();
    }

    private void createTiles()
    {
        ClearTiles();
        TileLayer[] tileLayers = m_PuzzleLevelData.TileLayers;
        int tileWidth = (int)m_RefTileCell.GetComponent<RectTransform>().rect.width;
        int tileHeight = (int)m_RefTileCell.GetComponent<RectTransform>().rect.height;
        float posX;
        float posY;
        Vector3 tilePos = Vector3.zero;
        int layerCount = tileLayers.Length;
        Dictionary<(int, int, int), TileCell> tileCellPairs = new Dictionary<(int, int, int), TileCell>();

        for (short layerIndex = 0; layerIndex < layerCount; layerIndex++)  //layer process
        {
            TileLayer tileLayer = tileLayers[layerIndex];
            Tile[] tiles = tileLayer.Tiles;
            float centerX = (float)(tileLayer.ColCountX - 1) / 2;
            float centerY = (float)(tileLayer.RowCountY - 1) / 2;
            int baseOrder = (layerIndex + 1) * 10;

            for (ushort tileIndex = 0; tileIndex < tiles.Length; tileIndex++)  //tile process
            {
                Tile tile = tiles[tileIndex];
                posX = (tile.ColX - centerX) * tileWidth + m_TilePlacementMargin.x;
                posY = (tile.RowY - centerY) * (tileHeight + m_TilePlacementMargin.y);
                tilePos.x = posX;
                tilePos.y = posY;
                TileCell newTileCell = Instantiate(m_RefTileCell, m_TilesContainer);
                newTileCell.transform.localPosition = tilePos;
                newTileCell.SetData(tile, baseOrder + tile.RowY, onTileClick);

                PuzzleTileData puzzleTileData = PuzzleTileDataList.Instance.GetPuzzleTileData(tile.Id);
                if (puzzleTileData == null)
                {
                    Debug.LogError($"PuzzleTileData not found. Id: {tile.Id}");
                }

                newTileCell.SetCustomize(puzzleTileData);
                tileCellPairs.Add((layerIndex, tile.RowY, tile.ColX), newTileCell);
                m_TileCellList.Add(newTileCell);
            }
        }

        if (tileCellPairs.Count > 0)
        {
            setTilesCellData(tileCellPairs);
        }
    }

    void putIntoCollect(TileCell _putInCell)
    {
        if(PuzzleSlotController.Instance.AllSlotsFull())
        {
            Debug.LogError ("All slots full");
            return;
        }

        _putInCell.onRemoveTile?.Invoke();
        onTileCollect?.Invoke(_putInCell);
    }
}
