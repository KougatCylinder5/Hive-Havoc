using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class AIController : MonoBehaviour
{
    
    public void Start()
    {
        List<Tile> path = GeneratePath(new Vector2Int(10, 10), new Vector2Int(440, 490), true); GeneratePath(new Vector2Int(10, 10), new Vector2Int(440, 490), true); GeneratePath(new Vector2Int(10, 10), new Vector2Int(440, 490), true);
        
        foreach (Tile tile in path)
        {
            UnityEngine.Debug.Log(tile);
        }
    }

    public Vector2Int Size { private set; get; }
    public List<Tile> Grid { private set; get; }
    /**
     * 
     * 
     */
    public static List<Tile> GeneratePath(Vector2 start, Vector2 end, bool ignoreOccupied = false)
    {
        Stopwatch watch = new Stopwatch();
        if (GridManager.GetTile(start).Equals(GridManager.GetTile(end)))
        {
            return new();
        }

        List<Tile> checkedTiles = new()
        {
            GridManager.GetTile(start).GetInstance().SetGCost(GridManager.GetTile(start).LinearDistanceToTarget(end))
        };
        SortedList<float, Tile> tilesToCheck = new();
        SortedList<float, Tile> holdingQueue = new();
        //set up for both lists
        foreach (Tile neighbor in GridManager.GetTile(start).Neighbors)
        {
            Tile copy = neighbor.GetInstance();
            copy.SetGCost(copy.LinearDistanceToTarget(end));
            copy.SetHCost(copy.HCost + 1);
            copy.parentTile = GridManager.GetTile(start);
            tilesToCheck.Add((copy.HCost + copy.GCost) * 1000 + Random.Range(-0.5f, 0.5f), copy);

        }
        int iterations = 0;

        Tile lastTouched = new(-1, -1);

        while (true)
        {
            
            watch.Start();
            //just a break so it doesn't get stuck if it can't find a path
            if (iterations++ == 5000)
            {
                break;
            }
            foreach (Tile neighbor in GridManager.GetTile((tilesToCheck.Values[0]).Position).Neighbors)
            {
                if (tilesToCheck.ContainsValue(neighbor) || checkedTiles.Contains(neighbor) || (neighbor.IsOccupied && ignoreOccupied))
                {
                    continue;
                }
                //create a copy so we don't modify the original
                Tile copy = neighbor.GetInstance();
                copy.SetGCost(copy.LinearDistanceToTarget(end));
                copy.SetHCost(copy.HCost + 0.5f);
                copy.parentTile = tilesToCheck.Values[0];
                //UnityEngine.Random is used to prevent collisions on prexisting keys
                while (true) 
                {
                    if (holdingQueue.TryAdd((copy.HCost + copy.GCost) * 1000 + Random.Range(-0.75f, 0.75f), copy))
                    {
                        break;
                    }
                }
                if (neighbor.Equals(GridManager.GetTile(end)))
                {
                    lastTouched = copy;
                    //to break out of both loops at once however a bad practice goto is
                    goto outter;
                }

            }
            checkedTiles.Add(tilesToCheck.Values[0]);
            tilesToCheck.RemoveAt(0);
            foreach (KeyValuePair<float,Tile> tile in holdingQueue)
            {
                while (true)
                {
                    if (tilesToCheck.TryAdd(tile.Key + Random.Range(-0.75f, 0.75f), tile.Value))
                    {
                        break;
                    }
                }
            }
            holdingQueue.Clear();
        }
        outter:

        List<Tile> completePath = new();
        completePath.Add(lastTouched);
        while (lastTouched.parentTile != null)
        {
            completePath.Add(lastTouched.parentTile);
            lastTouched = lastTouched.parentTile;
        }
        completePath.Reverse();
        watch.Stop();
        UnityEngine.Debug.Log(watch.ElapsedMilliseconds);
        return completePath;
    }

    public static List<Tile> GeneratePath(Vector2Int start, Vector2Int end, bool ignoreOccupied = false)
    {
        return GeneratePath(new Vector2(start.x, start.y), new Vector2(end.x, end.y), ignoreOccupied);
    }
}
