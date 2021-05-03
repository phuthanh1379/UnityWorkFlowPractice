using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class SquareGrid {
    #region Variables
    // Measurements info
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;
    
    // References info
    private Transform tilesParent;
    private List<TileScript> tiles = new List<TileScript>();
    #endregion

    #region Main Methods
    /// Constructor
    public SquareGrid(int width, int height, float cellSize) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridArray = new int[width, height];
        
        // Count tiles' indexes
        int tileIndex = 0;
        if (tilesParent == null)
        {
            tilesParent = GameObject.Instantiate(new GameObject("TilesParent"), Vector3.zero, Quaternion.identity).transform;
        }

        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int y = 0; y < gridArray.GetLength(1); y++) {
                // Spawn tiles
                GameObject tile = Helper.SpawnPrefab(
                    Constants.P_TILE_BASE, 
                    GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f, 
                    Quaternion.Euler(0, 0, 0),
                    tilesParent);
                
                // Add info to the spawned tile
                tile.AddComponent<TileScript>();
                tile.GetComponent<TileScript>().id = tileIndex;
                tile.GetComponent<TileScript>().x = x;
                tile.GetComponent<TileScript>().y = y;
                tile.GetComponent<TileScript>().worldPosition = GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f;
                tile.GetComponent<TileScript>().status = TileStatus.Null;
                tiles.Add(tile.GetComponent<TileScript>());
                tileIndex++;

                // Draw debug-lines for visual
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        // Draw debug-lines for visual
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    /// Private
    private Vector3 GetWorldPosition (int x, int y) {
        return new Vector3(x, 0, y) * cellSize;
    }

    /// Public
    public TileScript GetTileFromCoordinate(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
            return null;
        
        foreach (TileScript tile in tiles)
        {
            if (tile.x == x && tile.y == y)
            {
                return tile;
            }
        }
        return null;
    }
    
    public TileScript GetTileFromIndex(int id)
    {
        if (id >= 0 && id < tiles.Count)
            return tiles[id];
        return null;
    }
    
    public void SetValue(int x, int y, int value) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            gridArray[x, y] = value;
        }
    }

    public Vector3 GetTilePosition(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return new Vector3(x, 0, y) * cellSize + new Vector3(cellSize, 0, cellSize) * .5f;
        }

        return Vector3.zero;
    }
    #endregion
}