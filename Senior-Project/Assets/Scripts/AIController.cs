using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using UnityEngine;

public class AIController : MonoBehaviour
{

    public void Start()
    {
        Debug.Log(GeneratePath(new Vector2Int(0,0), new Vector2Int(3,3)));
    }

    public Vector2Int Size { private set; get; }
    public List<Tile> Grid { private set; get; }


    public AIController()
    {

    }

    public List<Tile> GeneratePath(Vector2Int start, Vector2Int end)
    {
        if (GridManager.GetTile(start).Equals(GridManager.GetTile(end)))
        {
            return new();
        }

        List<Tile> checkedTiles = new()
        {
            GridManager.GetTile(start).GetInstance().SetGCost(GridManager.GetTile(start).LinearDistanceToTarget(end))
        };
        SortedList tilesToCheck = new() {};

        foreach(Tile neighbor in GridManager.GetTile(start).Neighbors)
        {
            Tile copy = neighbor.GetInstance();
            copy.SetGCost(copy.LinearDistanceToTarget(end));
            copy.SetHCost(copy.HCost + 0.5f);

            tilesToCheck.Add((copy.HCost + copy.GCost) * 1000 * UnityEngine.Random.Range(-0.5f,0.5f) ,copy);
            
        }
        Debug.Log(tilesToCheck.GetByIndex(0));
        int iterations = 0;
        
            
        while (true)
        {
            foreach (Tile neighbor in GridManager.GetTile(tilesToCheck.GetByIndex(0).Position).Neighbors)
            {
                Tile copy = neighbor.GetInstance();
                copy.SetGCost(copy.LinearDistanceToTarget(end));
                copy.SetHCost(copy.HCost + 0.5f);

                tilesToCheck.Add((copy.HCost + copy.GCost) * 1000 * UnityEngine.Random.Range(-0.5f, 0.5f), copy);

            }




            if (iterations++ == 100)
            {
                break;
            }
        }

        return null;
    }

    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        foreach (Tile tile in GridManager.Grid)
        {
            Gizmos.DrawCube(new Vector3(tile.X, 0, tile.Y), new Vector3(0.5f, 0.5f, 0.5f));
        }
        
    }


}
