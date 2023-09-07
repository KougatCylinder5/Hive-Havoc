using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Tile : IComparable<Tile>, IEquatable<Tile>
{
    public int X { private set; get; }
    public int Y { private set; get; }
    public float GCost { private set { _gCost = value; } get { return _gCost; } }
    public float HCost { private set { _hCost = value; } get { return _hCost; } }
    public List<Tile> Neighbors { private set { _neighbors = value; } get { return _neighbors; } }
    public bool IsOccupied { private set { _isOccupied = value; } get { return _isOccupied; } }

    [SerializeField] private float _gCost = 0;
    [SerializeField] private float _hCost = 0;
    [NonSerialized] private List<Tile> _neighbors = new();
    [SerializeField] private bool _isOccupied = false;

    [NonSerialized] public List<Tile> PathPerTile = new();
    public Tile(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }
    private Tile(Tile tile)
    {
        X = tile.X;
        Y = tile.Y;
        _isOccupied = tile.IsOccupied;
        _neighbors = tile.Neighbors;
    }

    public float LinearDistanceToTarget(Vector2Int target)
    {
        return Mathf.Sqrt(Mathf.Pow(target.x - this.X, 2) + MathF.Pow(target.y - this.Y, 2));
    }
    public void AddNeighbor(Tile tile)
    {
        _neighbors.Add(tile);
    }
    public bool Equals(Tile other)
    {
        return this.X == other.X && this.Y == other.Y;
    }
    public int CompareTo(Tile other)
    {
        return this.X == other.X && this.Y == other.Y ? 0 : 1;
    }
    public Tile GetInstance()
    {
        return new Tile(this);
    }

    public Tile SetGCost(float cost)
    {
        _gCost = cost;
        return this;
    }
    public Tile SetHCost(float cost)
    {
        _hCost = cost;
        return this;
    }

    public override string ToString()
    {
        return string.Format("X : {0}, Y : {1}", X, Y);
    }
}