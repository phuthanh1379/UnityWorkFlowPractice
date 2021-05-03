using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileStatus
{
    Null = 0, // Nothing is on this tile yet
    Rock = 1, // The tile is now rock-based (can still be built upon)
    Occupied = 2 // The tile has a building built upon it (cannot be built upon anymore)
}

public class TileScript : MonoBehaviour
{
    #region Variables

    [Header("Tile Info")] 
    public int id;
    public int x;
    public int y;
    public Vector3 worldPosition;
    public TileStatus status;
    #endregion
    
    #region Events
    private void OnMouseDown()
    {
        SpawnTower();
        Debug.Log("Co-ordinate=" + x + "," + y + "; status=" + status.ToString());
    }
    #endregion

    #region Methods
    private void SpawnTower()
    {
        if (status != TileStatus.Occupied)
        {
            // Spawn tower on click
            GameObject tw = Helper.SpawnPrefab(
                Constants.P_TOWER_BASIC, 
                worldPosition, 
                Quaternion.Euler(0, 0, 0),
                transform);
            tw.transform.localScale = new Vector3(0.5f, 50f, 0.5f);
            tw.transform.localPosition += new Vector3(0f, 50f, 0f);
        }
        
        // Update status
        status = TileStatus.Occupied;
    }
    #endregion
}
