using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile :IComparable, IComparable<Tile>, IEquatable<Tile>
{
    public int X { private set; get; }
    public int Y { private set; get; }
    public Vector2Int Position { private set; get; }
    public float GCost { private set { _gCost = value; } get { return _gCost; } }
    public float HCost { private set { _hCost = value; } get { return _hCost; } }
    public List<Tile> Neighbors { private set { _neighbors = value; } get { return _neighbors; } }
    public bool IsOccupied { private set { _isOccupied = value; } get { return _isOccupied; } }

    [SerializeField] private float _gCost = 0;
    [SerializeField] private float _hCost = 0;
    private List<Tile> _neighbors = new();
    [SerializeField] private bool _isOccupied = false;

    public Tile parentTile;
    
    public Tile(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
        Position = new Vector2Int(X, Y);
    }
    private Tile(Tile tile)
    {
        X = tile.X;
        Y = tile.Y;
        _isOccupied = tile.IsOccupied;
        _neighbors = tile.Neighbors;
        this.Position = tile.Position;
        this._gCost = tile.GCost;
        this.parentTile = tile.parentTile;
    }
    public float LinearDistanceToTarget(Vector2 target)
    {
        return Mathf.Sqrt(Mathf.Pow(Mathf.Abs(target.x - this.X), 2) + Mathf.Pow(Mathf.Abs(target.y - this.Y), 2));
    }
    public float LinearDistanceToTarget(Vector2Int target)
    {
        return LinearDistanceToTarget(new Vector2(target.x, target.y));
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
        if (this._hCost + this._gCost == other._hCost + other._gCost) return 0;
        if (this._hCost + this._gCost < other._hCost + other._gCost) return -1;
        if (this._hCost + this._gCost > other._hCost + other._gCost) return 1;
        return int.MinValue;
    }
    public int CompareTo(object obj)
    {
        return this.CompareTo(obj);
    }
    public override int GetHashCode()
    {
        return (int)((_gCost + _hCost) * 1000);
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
