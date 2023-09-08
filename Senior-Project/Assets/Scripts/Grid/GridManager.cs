
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private static int _xSize;
    [SerializeField] private static int _ySize;

    public static int XSize {private set { _xSize = value; } get { return _xSize; } }
    public static int YSize {private set { _ySize = value; } get { return _ySize; } }
    
    public Vector2Int Size { private set; get; }

    public static List<Tile> Grid {private set; get;} 
    
    public GridManager(int xSize, int ySize)
    {
        _xSize = xSize;
        _ySize = ySize;
    }
    
    public void Awake()
    {
        new GridManager(500, 500);
        GenerateGrid();
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
            int[] spots = new int[] { tile.X - 1, tile.X + tile.Y * _xSize - _xSize - 1, tile.X + tile.Y * _xSize - _xSize, tile.X + tile.Y * _xSize - _xSize + 1, tile.X + 1, tile.X + tile.Y * _xSize + _xSize + 1, tile.X + tile.Y * _xSize + _xSize, tile.X + tile.Y * _xSize + _xSize - 1 };
            foreach (int spot in spots)
            {
                if(tile.LinearDistanceToTarget(new Vector2(spot % _xSize, spot / _xSize)) > 2) { continue; }
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

    public static Tile GetTile(Vector2 pos)
    {
        return GetTile(new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)));
    }
    public static Tile GetTile(Vector2Int pos)
    {
        return Grid[pos.x + pos.y * XSize];
    }
}
