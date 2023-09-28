/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEditor;
using System.Linq;
using System;
using Unity.Collections.NotBurstCompatible;

public class PathingManager : MonoBehaviour
{
    [SerializeField]
    public static Vector2Int GridSize { get; private set; }
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public List<PathInfo> Paths { get; private set; }

    public static List<bool> ObstructedTiles = new List<bool>();

    private Queue<PathInfo> _pathsToGenerate;

    public static PathingManager Instance;

    public void Awake()
    {
        GridSize = new(15, 15);

        Instance = this;
        _pathsToGenerate = new();
        Paths = new();
        InvokeRepeating(nameof(DestroyAllPaths), 0, 60);

        for(int i = 0; i < GridSize.x * GridSize.y; i++)
        {
            ObstructedTiles.Add(true);
        }
    }

    public void LateUpdate()
    {

        List<PathInfo> tempGeneratedPath = ReturnPaths(_pathsToGenerate);

        foreach (PathInfo path in tempGeneratedPath)
        {
            if(Paths.IndexOf(path) == -1)
            {
                Paths.Add(path);
            }
        }
        
        
        _pathsToGenerate.Clear();
        
    }
    public void QueuePath(PathInfo pathToQueue)
    {
        _pathsToGenerate.Enqueue(pathToQueue);
    }

    public void DestroyAllPaths()
    {
        Paths.Clear();
    }

    private List<PathInfo> ReturnPaths(Queue<PathInfo> pathsToGen)
    {
        NativeList<JobHandle> jobs = new(pathsToGen.Count, Allocator.Temp);
        List<NativeList<float2>> rawPath = new();
        List<PathInfo> paths = new();

        NativeArray<bool> obstructedTiles = new(GridSize.x * GridSize.y, Allocator.TempJob);
        for( int j = 0; j < ObstructedTiles.Count; j++)
        {
            obstructedTiles[j] = ObstructedTiles[j];
        }

        while (pathsToGen.Count > 0)
        {
            
            rawPath.Add(new NativeList<float2>(Allocator.Persistent));
            FindPathJob findPathJob = new()
            {
                exactEndPosition = pathsToGen.Peek().End,
                startPosition = new int2(Mathf.RoundToInt(pathsToGen.Peek().Start.x), Mathf.RoundToInt(pathsToGen.Peek().Start.y)),
                endPosition = new int2(Mathf.RoundToInt(pathsToGen.Peek().End.x), Mathf.RoundToInt(pathsToGen.Peek().End.y)),
                path = rawPath[^1],
                obstructedGrid = obstructedTiles.AsReadOnly(),
                GridSize = new int2(GridSize.x,GridSize.y)
                
                
            };
            jobs.Add(findPathJob.Schedule());
            paths.Add(pathsToGen.Dequeue());
        }
        
        JobHandle.CompleteAll(jobs);

        obstructedTiles.Dispose();
        int i = 0;
        foreach(NativeList<float2> iPath in rawPath)
        {

            Queue<Vector2> path = new Queue<Vector2>();
            for (int j = iPath.Length-1; j >= 0; j--) 
            {
                
                float2 node = iPath[j];

                path.Enqueue(new Vector2(node.x, node.y));
            }

            path.TryDequeue(out Vector2 _);
            paths[i].path = path;
            paths[i++].CleanPath();

            iPath.Dispose();
        }


        jobs.Dispose();
        return paths;
    }



    [BurstCompile]
    private struct FindPathJob : IJob
    {
        public float2 exactEndPosition;
        [ReadOnly]
        public NativeArray<bool>.ReadOnly obstructedGrid;

        public int2 startPosition;
        public int2 endPosition;
        public NativeList<float2> path;

        public int2 GridSize;
        public void Execute()
        {

            NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(GridSize.x * GridSize.y, Allocator.Temp);

            for (int x = 0; x < GridSize.x; x++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    PathNode pathNode = new()
                    {
                        x = x,
                        y = y,
                        index = CalculateIndex(x, y, GridSize.x),

                        gCost = int.MaxValue,
                        hCost = CalculateDistanceCost(new int2(x, y), endPosition),

                        isWalkable = obstructedGrid[CalculateIndex(x, y, GridSize.x)],
                        cameFromNodeIndex = -1
                    };
                    pathNode.CalculateFCost();

                    pathNodeArray[pathNode.index] = pathNode;
                }
            }


            NativeArray<int2> neighbourOffsetArray = new(4, Allocator.Temp);
            neighbourOffsetArray[0] = new int2(-1, 0); // Left
            neighbourOffsetArray[1] = new int2(+1, 0); // Right
            neighbourOffsetArray[2] = new int2(0, +1); // Up
            neighbourOffsetArray[3] = new int2(0, -1); // Down

            int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, GridSize.x);

            PathNode startNode = pathNodeArray[CalculateIndex(startPosition.x, startPosition.y, GridSize.x)];
            startNode.gCost = 0;
            startNode.CalculateFCost();
            pathNodeArray[startNode.index] = startNode;

            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
            NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

            openList.Add(startNode.index);

            while (openList.Length > 0)
            {
                int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);
                PathNode currentNode = pathNodeArray[currentNodeIndex];

                if (currentNodeIndex == endNodeIndex)
                {
                    // Reached our destination!
                    break;
                }

                // Remove current node from Open List
                for (int i = 0; i < openList.Length; i++)
                {
                    if (openList[i] == currentNodeIndex)
                    {
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }

                closedList.Add(currentNodeIndex);

                for (int i = 0; i < neighbourOffsetArray.Length; i++)
                {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                    if (!IsPositionInsideGrid(neighbourPosition, GridSize))
                    {
                        // Neighbour not valid position
                        continue;
                    }

                    int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, GridSize.x);

                    if (closedList.Contains(neighbourNodeIndex))
                    {
                        // Already searched this node
                        continue;
                    }

                    PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
                    if (!neighbourNode.isWalkable)
                    {
                        // Not walkable
                        continue;
                    }

                    int2 currentNodePosition = new int2(currentNode.x, currentNode.y);

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNodeIndex = currentNodeIndex;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.CalculateFCost();
                        pathNodeArray[neighbourNodeIndex] = neighbourNode;

                        if (!openList.Contains(neighbourNode.index))
                        {
                            openList.Add(neighbourNode.index);
                        }
                    }

                }
            }

            PathNode endNode = pathNodeArray[endNodeIndex];
            if (endNode.cameFromNodeIndex == -1)
            {
                // Didn't find a path!
            }
            else
            {
                path.Add(new int2(endNode.x, endNode.y));


                PathNode currentNode = endNode;
                while (currentNode.cameFromNodeIndex != -1)
                {
                    PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
                    path.Add(new int2(cameFromNode.x, cameFromNode.y));
                    currentNode = cameFromNode;
                }
                path.RemoveAt(path.Length - 1);
                path.Add(exactEndPosition);

            }
            pathNodeArray.Dispose();
            neighbourOffsetArray.Dispose();
            openList.Dispose();
            closedList.Dispose();
        }

        private bool IsPositionInsideGrid(int2 gridPosition, int2 GridSize)
        {
            return
                gridPosition.x >= 0 &&
                gridPosition.y >= 0 &&
                gridPosition.x < GridSize.x &&
                gridPosition.y < GridSize.y;
        }

        private int CalculateIndex(int x, int y, int gridWidth)
        {
            return x + y * gridWidth;
        }

        private int CalculateDistanceCost(int2 aPosition, int2 bPosition)
        {
            int xDistance = math.abs(aPosition.x - bPosition.x);
            int yDistance = math.abs(aPosition.y - bPosition.y);
            int remaining = math.abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }


        private int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray)
        {
            PathNode lowestCostPathNode = pathNodeArray[openList[0]];
            for (int i = 1; i < openList.Length; i++)
            {
                PathNode testPathNode = pathNodeArray[openList[i]];
                if (testPathNode.fCost < lowestCostPathNode.fCost)
                {
                    lowestCostPathNode = testPathNode;
                }
            }
            return lowestCostPathNode.index;
        }

        public struct PathNode
        {
            public int x;
            public int y;

            public int index;

            public int gCost;
            public int hCost;
            public int fCost;

            public bool isWalkable;

            public int cameFromNodeIndex;

            public void CalculateFCost()
            {
                fCost = gCost + hCost;
            }

            public void SetIsWalkable(bool isWalkable)
            {
                this.isWalkable = isWalkable;
            }
        }

    }
    public static int CalculateIndex(int x, int y, int gridWidth)
    {
        return x + y * gridWidth;
    }
}
