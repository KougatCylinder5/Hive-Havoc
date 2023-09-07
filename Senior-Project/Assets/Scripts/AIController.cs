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

        GenerateGrid();

        Debug.Log(GeneratePath(new Vector2Int(0,0), new Vector2Int(3,3)));
    }

    public Vector2Int Size { private set; get; }
    public List<Tile> Grid { private set; get; }


    public AIController(int XSize, int YSize)
    {
        this.XSize = XSize;
        this.YSize = YSize;
        this.Size = new Vector2Int(XSize, YSize);
    }

    public List<Tile> GeneratePath(Vector2Int start, Vector2Int end)
    {
        List<Tile> checkedTiles = new(){GetTile(start)};
        SortedList<Tile>
        
        if (GetTile(start).Equals(GetTile(end)))
        {
            return new();
        }
            
        while (true)
        {
            
            


            
            if(iterations++ == 100)
            {
                break;
            }
        }

        return null;
    }

    
    
    public static List<Tile> SortByScore(List<Tile> tiles)
    {

        tiles.Sort(delegate(Tile a, Tile b)
        {
            return Mathf.RoundToInt((a.HCost + a.GCost) - (b.HCost + b.GCost) * 1000);
        });

        return tiles;
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        foreach (Tile tile in Grid)
        {
            Gizmos.DrawCube(new Vector3(tile.X, 0, tile.Y), new Vector3(0.5f, 0.5f, 0.5f));
        }
        
    }


}
