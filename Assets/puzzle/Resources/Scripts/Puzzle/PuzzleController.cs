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

public class PuzzleController : SingletonComponent<PuzzleController>
{
    #region UTILITIES

    public UnityAction<TileCell> onTileCollect;

    #endregion

    #region FIELDS

    //Parameters
    [FoldoutGroup("Parameters"), SerializeField] private float tileYMargin;

    //Components
    [FoldoutGroup("Components")]
    //[FoldoutGroup("Components/Utilities"), SerializeField] private SpriteRenderer tileCollectorSpriteRenderer;
    //[FoldoutGroup("Components/Utilities"), SerializeField] private Vector3[] collectVects = new Vector3[7];
    [FoldoutGroup("Components/Utilities"), SerializeField] private TileLayersSO tileLayersSO;
    [FoldoutGroup("Components/Utilities"), SerializeField] private TileCell tileCellPrefab;
    [FoldoutGroup("Components/Utilities"), SerializeField] private Transform tileInstantiateTrans;
    //[FoldoutGroup("Components/Lists"), SerializeField] private Sprite[] iconSprites;  //Temp
    //[FoldoutGroup("Components/Lists"), SerializeField] private List<TileSlot> m_Slots = new List<TileSlot>();

    //Indicator
    //[FoldoutGroup("Indicator"), SerializeField, ReadOnly] private

    //Privates
    //private TileCell[] collectedTiles;

    #endregion


    private void Awake()
    {
        //setTileCollector();
    }

    private void Start() 
    {
        //collectedTiles = new TileCell[PuzzleSlotController.Instance.SlotCount()];
        if (tileLayersSO != null && tileLayersSO.TileLayers != null)
        {
           createTiles();
        }    
    }

    /*void createTiles()
    {
        var tileLayers = tileLayersSO.TileLayers;
        var tileSpriteRenderer = tileCellPrefab.TileSpriteRenderer;
        //var tileSpacingX = tileSpriteRenderer.sprite.bounds.extents.x;
        var tileSpacingX = 130/2;
        //var tileSpacingY = tileSpriteRenderer.sprite.bounds.extents.y;
        var tileSpacingY = 130/2;
        float posX;
        float posY;
        Vector3 tilePos = Vector3.zero;
        var layerCount = tileLayers.Length;
        var tileCellPairs = new Dictionary<(int, int, int), TileCell>();
        List<TileCell> unsetIdTile = new List<TileCell>();
        var idCountPairs = new Dictionary<int, int>();

        for(short layerIndex = 0; layerIndex < layerCount; layerIndex++)  //layer process
        {
            var tileLayer = tileLayers[layerIndex];
            var tiles = tileLayer.Tiles;
            var centerX = (float)(tileLayer.ColCountX - 1) / 2;
            var centerY = (float)(tileLayer.RowCountY - 1) / 2;
            var baseOrder = (layerIndex + 1) * 10;

            for(ushort tileIndex = 0; tileIndex < tiles.Length; tileIndex++)  //tile process
            {
                var tile = tiles[tileIndex];  
                posX = (tile.ColX - centerX) * tileSpacingX * 2;
                posY = (tile.RowY - centerY) * (tileSpacingY * -2 + tileYMargin); 
                tilePos.x = posX;
                tilePos.y = posY;
                var newTileCell = Instantiate(tileCellPrefab, tileInstantiateTrans);
                newTileCell.transform.localPosition = tilePos;
                newTileCell.SetData(tile, baseOrder + tile.RowY, onTileClick);

                if(tile.Id > 0)
                {
                    newTileCell.SetIcon(iconSprites[tile.Id - 1]);
                    if(!idCountPairs.ContainsKey(tile.Id))
                        idCountPairs.Add(tile.Id, 0);
                    idCountPairs[tile.Id] += 1;
                }
                else
                {
                    unsetIdTile.Add(newTileCell);
                }

                tileCellPairs.Add((layerIndex, tile.RowY, tile.ColX), newTileCell);

            #if UNITY_EDITOR
                var stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append(layerIndex);
                stringBuilder.Append(tile.RowY);
                stringBuilder.Append(tile.ColX);
                stringBuilder.Append(baseOrder + tile.ColX);
                newTileCell.gameObject.name = stringBuilder.ToString();
            #endif    
            }
        }

        if(tileCellPairs.Count > 0)
        {
            setTilesCellData(tileCellPairs);
        }

        if(unsetIdTile.Count > 0)
        {
            setTileId(unsetIdTile, idCountPairs);
        }
    }*/

    /*void setTileId(List<TileCell> _cellList, Dictionary<int, int> _idCountPairs)
    {
        Shuffle(_cellList);
        int needIdCount = _cellList.Count;
        List<int> unusedId = new List<int>();

        foreach(var pairs in _idCountPairs)
        {
            if(pairs.Value / tileLayersSO.DifferentIdCount < 1)
            {
                var leftCount = pairs.Value % tileLayersSO.DifferentIdCount;
                for(int index = 0; index < leftCount; index++)
                {
                    unusedId.Add(pairs.Key);
                }
            }
        }
        
        for(int index = 0; index > _cellList.Count; index++)
        {
            _cellList[index].SetId((ushort)unusedId[index]);
            _cellList[index].SetIcon(iconSprites[unusedId[index] - 1]);
        }
    }*/

    private void setTilesCellData(Dictionary<(int, int, int), TileCell> _tileCellPairs)
    {
        var tileLayers = tileLayersSO.TileLayers;
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

    /*void setTileCollector()
    {
        var tileSpacingX = tileCollectorSpriteRenderer.sprite.bounds.extents.x * 2;
        var collectorVect = tileCollectorSpriteRenderer.transform.localPosition;
        int middleNum = 3;

        for(ushort index = 0; index < collectVects.Length; index++)
        {
            float x = collectorVect.x + (index - middleNum) * tileSpacingX;
            collectVects[index] = new Vector3(x ,collectorVect.y, collectorVect.z);
        }
    }*/

    void onTileClick(TileCell _cell)
    {
        _cell.TriggerEnable(false);
        putIntoCollect(_cell);
    }

    /*void putIntoCollect(TileCell _putInCell)
    {
        var putInCellId = _putInCell.Id;
        short nullIndex = -1;
        short matchIndex = -1;
        bool lose = true;

        for(short index = 0; index < collectedTiles.Length; index++)
        {
            var cell = collectedTiles[index];
            if(cell != null)
            {
                if(cell.Id == putInCellId)
                {
                    if(matchIndex < 0)
                        matchIndex = index;
                    else
                    {
                        //Match
                        _putInCell.onRemoveTile?.Invoke();
                        _putInCell.OnMatch();
                        collectedTiles[matchIndex].OnMatch();
                        collectedTiles[matchIndex] = null;
                        cell.OnMatch();
                        collectedTiles[index] = null;
                        return;
                    }
                }
            }
            else
            {
                if(nullIndex < 0)
                    nullIndex = index;
                else
                    lose = false;
            }
        }

        if(nullIndex == -1)
            return;

        _putInCell.onRemoveTile?.Invoke();
        //_putInCell.transform.localPosition = collectVects[nullIndex];
        GetFirstEmptySlot().Filled(_putInCell);
        _putInCell.OnCollect();

        collectedTiles[nullIndex] = _putInCell;
    }*/

    private List<(int, int)> getTileBlockIndex((ushort, ushort) buildTileRowCol, (ushort, ushort) upLayerRowCol, (ushort, ushort) downLayerRowCol)
    {
        //TODO 這部分運算可以抽出來
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
    

    //Functions
    void createTiles()
    {
        TileLayer[] tileLayers = tileLayersSO.TileLayers;
        Image tileSpriteRenderer = tileCellPrefab.TileSpriteRenderer;
        int tileSpacingX = 130 / 2;
        int tileSpacingY = 130 / 2;
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
                posX = (tile.ColX - centerX) * tileSpacingX * 2;
                posY = (tile.RowY - centerY) * (tileSpacingY * -2 + tileYMargin);
                tilePos.x = posX;
                tilePos.y = posY;
                TileCell newTileCell = Instantiate(tileCellPrefab, tileInstantiateTrans);
                newTileCell.transform.localPosition = tilePos;
                newTileCell.SetData(tile, baseOrder + tile.RowY, onTileClick);

                /*if (tile.Id > 0)
                {
                    newTileCell.SetIcon(iconSprites[tile.Id - 1]);
                    if (!idCountPairs.ContainsKey(tile.Id))
                        idCountPairs.Add(tile.Id, 0);
                    idCountPairs[tile.Id] += 1;
                }
                else
                {
                    unsetIdTile.Add(newTileCell);
                }*/

                PuzzleTileData puzzleTileData = PuzzleTileDataList.Instance.GetPuzzleTileData(tile.Id);
                if (puzzleTileData == null)
                {
                    Debug.LogError($"PuzzleTileData not found. Id: {tile.Id}");
                }

                newTileCell.SetCustomize(puzzleTileData);
                tileCellPairs.Add((layerIndex, tile.RowY, tile.ColX), newTileCell);
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

        //var putInCellId = _putInCell.Id;
        short nullIndex = -1;
        short matchIndex = -1;
        bool lose = true;

        /*for (short index = 0; index < collectedTiles.Length; index++)
        {
            var cell = collectedTiles[index];
            if (cell != null)
            {
                if (cell.Id == putInCellId)
                {
                    if (matchIndex < 0)
                        matchIndex = index;
                    else
                    {
                        //Match
                        _putInCell.onRemoveTile?.Invoke();
                        _putInCell.OnMatch();
                        collectedTiles[matchIndex].OnMatch();
                        collectedTiles[matchIndex] = null;
                        cell.OnMatch();
                        collectedTiles[index] = null;
                        return;
                    }
                }
            }
            else
            {
                if (nullIndex < 0)
                    nullIndex = index;
                else
                    lose = false;
            }
        }

        if (nullIndex == -1)
            return;*/

        _putInCell.onRemoveTile?.Invoke();
        //_putInCell.transform.localPosition = collectVects[nullIndex];
        //PuzzleSlotController.Instance.GetFirstEmptySlot().Filled(_putInCell);
        //_putInCell.OnCollect();
        onTileCollect?.Invoke(_putInCell);
        //collectedTiles[nullIndex] = _putInCell;
    }
}
