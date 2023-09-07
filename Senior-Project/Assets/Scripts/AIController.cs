using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class AIController : MonoBehaviour
{

    public void Start()
    {

        GenerateGrid();

        Debug.Log(GeneratePath(new Vector2Int(0,0), new Vector2Int(3,3)));
    }

    [SerializeField] private int _xSize;
    [SerializeField] private int _ySize;

    public int XSize { private set { _xSize = value; } get { return _xSize; } }
    public int YSize { private set { _ySize = value; } get { return _ySize; } }
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
        List<Tile> viableTiles = new() { GetTile(start).SetHCost(GetTile(start).LinearDistanceToTarget(end)) };
        List<Tile> checkedTiles = new();
        int iterations = 0;
        if (GetTile(start).Equals(GetTile(end)))
        {
            return new();
        }
        while (true)
        {
            int i = 0;

            for ( i = 0; i < viableTiles.Count; i++)
            {
                foreach (Tile canidate in viableTiles[i].Neighbors)
                {
                    
                    if (viableTiles.Contains(canidate) || checkedTiles.Contains(canidate) || canidate.IsOccupied)
                    {
                        continue;
                    }
                    Tile copy = canidate.GetInstance();
                    List<Tile> path = viableTiles[i].PathPerTile;
                    path.Add(copy);
                    copy.PathPerTile = path;
                    copy.SetGCost(viableTiles[i].GCost + 0.5f);
                    copy.SetHCost(canidate.LinearDistanceToTarget(end));
                    
                    viableTiles.Add(copy);
                    viableTiles.Sort();
                    if (viableTiles[i].Equals(GetTile(end)))
                    {
                        return path;
                    }
                }
                checkedTiles.AddRange(viableTiles);
            }
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

    private void GenerateGrid()
    {
        int length = XSize * YSize;

        Grid = new List<Tile>();
        for(int i = 0; i < length; i++)
        {
            Grid.Add(new Tile(i % XSize, Mathf.FloorToInt(i/YSize)));
        }
        
        foreach (Tile tile in Grid)
        {
            int[] spots = new int[] { tile.X - 1, tile.X + 1, tile.X + tile.Y * _xSize - _xSize, tile.X + tile.Y * _xSize + _xSize };
            foreach (int spot in spots)
            {
                
                try
                {
                    tile.AddNeighbor(Grid[spot]);
                }
                catch
                {
                    //discard because its outside the array
                }
                
            }
            
        }
        
    }
    

    public Tile GetTile(Vector2 pos)
    {
        return GetTile(new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)));
    }
    public Tile GetTile(Vector2Int pos)
    {
        return Grid[pos.x + pos.y * XSize];
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
