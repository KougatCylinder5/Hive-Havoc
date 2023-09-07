using Unity.Engine;
using System;
using System.Collections;
using System.COllections.Generic;

public class GridManager : Monobehavior
{
    [SerializeField] private int _xSize;
    [SerializeField] private int _ySize;

    public int XSize {private set { _xSize = value; } get { return _xSize; } }
    public int YSize {private set { _ySize = value; } get {return _ySize; } }
    
    public Vector2Int Size { private set; get; }

    public static List<Tile> Grid {private set; get;} 
    
    public GridManager(int xSize, int ySize)
    {
        this._xSize = xSize;
        this._ySize = ySize;
    }
    
    public void Awake()
    {
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
}
