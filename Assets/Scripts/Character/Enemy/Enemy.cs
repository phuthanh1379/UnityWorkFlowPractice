using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Private
    private float speed = 10.0f;
    private int currentStopIndex = 0;
    private Vector3 buffer = new Vector3(0f, 0.5f, 0f);
    
    // Public
    [Header("Stop points")] 
    public List<TileScript> stopPoints = new List<TileScript>();

    private void Update()
    {
        MoveToPoints();
    }

    private void MoveToPoints()
    {
        if (transform.position != stopPoints[currentStopIndex].worldPosition + buffer)
        {
            // Move to stop point
            transform.position = Vector3.MoveTowards(
                transform.position, 
                stopPoints[currentStopIndex].worldPosition + buffer, 
                speed * Time.deltaTime
            );
        }
        // If arrived, change stop point
        else
        {
            // If currently at final stopPoint
            if (currentStopIndex == stopPoints.Count - 1)
                currentStopIndex = 0;
            else
                currentStopIndex++;
        }
    }

    private void CalculatePath(TileScript startingTile, TileScript destinationTile)
    {
        
    }
}
