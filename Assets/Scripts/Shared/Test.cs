using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    private SquareGrid grid;
    private List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        // Create grid
        grid = new SquareGrid(11, 11, 1.01f);
        
        // Spawn enemies
        InvokeRepeating("SpawnEnemy", 0.5f, 0.2f);
    }

    private void Update()
    {
        if (enemies.Count >= 10)
        {
            CancelInvoke();
        }
    }

    private void SpawnEnemy()
    {
        // Spawn
        GameObject enemy = Helper.SpawnPrefab(
            Constants.P_ENEMY_SPHERE, 
            grid.GetTilePosition(0, 0) + new Vector3(0f, 0.5f, 0f), 
            Quaternion.identity);
        enemy.AddComponent<Enemy>();
        enemy.GetComponent<Enemy>().stopPoints.Add(grid.GetTileFromCoordinate(0, 5));
        enemy.GetComponent<Enemy>().stopPoints.Add(grid.GetTileFromCoordinate(10, 5));
        enemy.GetComponent<Enemy>().stopPoints.Add(grid.GetTileFromCoordinate(10, 10));
        enemy.GetComponent<Enemy>().stopPoints.Add(grid.GetTileFromCoordinate(5, 10));
        enemy.GetComponent<Enemy>().stopPoints.Add(grid.GetTileFromCoordinate(5, 0));
        enemy.GetComponent<Enemy>().stopPoints.Add(grid.GetTileFromCoordinate(10, 0));
        enemy.GetComponent<Enemy>().stopPoints.Add(grid.GetTileFromCoordinate(0, 0));
        enemies.Add(enemy);
    }
}
