using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;



public class FlowFieldGenerator : MonoBehaviour
{
    public static FlowFieldJob.PathNode[,] FlowTiles { get; private set; }
    public List<bool> NaturalObstructedTiles { get; private set; }
    [SerializeField]
    public static Vector3 _startPoint { private set; get; }
    public static bool Finished { private set; get; }

    private NativeList<FlowFieldJob.PathNode> positionsToCheck;
    private NativeList<FlowFieldJob.PathNode> closedList;
    private NativeList<JobHandle> handles;
    private NativeArray<bool> obstructedTiles;
    private Coroutine gridGenerator;
    [BurstCompile]
    public void Awake()
    {
        Finished = false;
        NaturalObstructedTiles = new();
        for (int i = 0; i < PathingManager.GridSize.x * PathingManager.GridSize.y; i++)
        {
            NaturalObstructedTiles.Add(PathingManager.ObstructedTiles[i]);
        }
        FlowTiles = new FlowFieldJob.PathNode[PathingManager.GridSize.x, PathingManager.GridSize.y];
        
    }
    // Start is called before the first frame update

    private void Start()
    {
        _startPoint = GameObject.FindGameObjectWithTag("CommandCenter").transform.position;
        positionsToCheck = new(Allocator.Persistent) { new() { position = new((int)_startPoint.x, (int)_startPoint.z), direction = Vector2.zero, gCost = 0, fCost = 0, hCost = 0, isWalkable = true } };
        closedList = new(Allocator.Persistent);
        handles = new NativeList<JobHandle>(Allocator.Persistent);

        obstructedTiles = new(PathingManager.GridSize.x * PathingManager.GridSize.y, Allocator.Persistent);
        for (int j = 0; j < NaturalObstructedTiles.Count; j++)
        {
            obstructedTiles[j] = NaturalObstructedTiles[j];
        }
        gridGenerator =  StartCoroutine(nameof(NodeIterate));
        
    }
    private IEnumerator NodeIterate()
    {
        while (positionsToCheck.Length > 0)
        {
            NativeList<FlowFieldJob.PathNode> newPositionsToCheck = new(positionsToCheck.Length * 8, Allocator.Persistent);
            FlowFieldJob job = new()
            {
                startPosition = new((int)_startPoint.x, (int)_startPoint.z),
                positionsToCheck = positionsToCheck.AsParallelReader(),
                newPositionsToCheck = newPositionsToCheck.AsParallelWriter(),
                obstructedTiles = obstructedTiles.AsReadOnly(),
                closedList = closedList.AsParallelReader(),
                gridSize = PathingManager.GridSize
            };

            handles.Add(job.Schedule(positionsToCheck.Length, 1));
            JobHandle.CompleteAll(handles);
            foreach (FlowFieldJob.PathNode oldNode in positionsToCheck)
            {
                closedList.Add(oldNode);
            }
            positionsToCheck.Clear();
            foreach (FlowFieldJob.PathNode position in newPositionsToCheck)
            {
                if (!positionsToCheck.Contains(position) && !closedList.Contains(position))
                {
                    positionsToCheck.Add(position);
                }

            }
            newPositionsToCheck.Dispose();
            yield return new WaitForEndOfFrame();
        }
        foreach (FlowFieldJob.PathNode node in closedList)
        {
            FlowTiles[node.position.x, node.position.y] = node;
        }
        Finished = true;
        obstructedTiles.Dispose();
        closedList.Dispose();
        handles.Dispose();
        positionsToCheck.Dispose();
        StopCoroutine(gridGenerator);
    }
    [BurstCompile]
    public struct FlowFieldJob : IJobParallelFor
    {
        [ReadOnly]
        public Vector2Int startPosition;
        public NativeArray<PathNode>.ReadOnly positionsToCheck;
        public NativeArray<PathNode>.ReadOnly closedList;
        public NativeArray<bool>.ReadOnly obstructedTiles;
        public Vector2Int gridSize;
        public NativeList<PathNode>.ParallelWriter newPositionsToCheck;

        public void Execute(int index)
        {


            NativeArray<Vector2Int> neighbourOffsetArray = new(8, Allocator.Temp);
            neighbourOffsetArray[0] = new Vector2Int(-1, 0); // Left
            neighbourOffsetArray[1] = new Vector2Int(+1, 0); // Right
            neighbourOffsetArray[2] = new Vector2Int(0, +1); // Up
            neighbourOffsetArray[3] = new Vector2Int(0, -1); // Down


            foreach (Vector2Int neighbour in neighbourOffsetArray)
            {
                PathNode node = new()
                {
                    position = positionsToCheck[index].position + neighbour,
                    direction = -neighbour * 2,
                    hCost = Vector2.Distance(startPosition, positionsToCheck[index].position + neighbour),
                    gCost = positionsToCheck[index].gCost + 1,

                };
                node.CalculateFCost();
                if (!node.IsPositionInsideGrid(node.position, gridSize) || closedList.Contains(node))
                {
                    continue;
                }
                node.isWalkable = obstructedTiles[CalculateIndex(positionsToCheck[index].position + neighbour, gridSize.x)];


                if (node.isWalkable)
                {
                    newPositionsToCheck.AddNoResize(node);
                }

            }
            neighbourOffsetArray.Dispose();
        }

        public struct PathNode : IComparer<PathNode>, IEquatable<PathNode>
        {
            public Vector2Int position;
            public Vector2 direction;
            public float hCost, gCost, fCost;
            public bool isWalkable;

            public float CalculateFCost()
            {
                fCost = hCost + gCost;
                return fCost;
            }

            public int Compare(PathNode x, PathNode y)
            {
                return (x.position - y.position).sqrMagnitude;
            }

            public bool Equals(PathNode other)
            {
                return position == other.position;
            }

            public bool IsPositionInsideGrid(Vector2Int position, Vector2Int GridSize)
            {
                return
                    position.x >= 0 &&
                    position.y >= 0 &&
                    position.x < GridSize.x &&
                    position.y < GridSize.y;
            }
            public override string ToString()
            {
                return position.ToString();
            }
        }

        private int CalculateIndex(int x, int y, int gridWidth)
        {
            return x + y * gridWidth;
        }
        private int CalculateIndex(Vector2Int pos, int gridWidth)
        {
            return pos.x + pos.y * gridWidth;
        }
    }
    private void OnApplicationQuit()
    {
        obstructedTiles.Dispose();
        closedList.Dispose();
        handles.Dispose();
        positionsToCheck.Dispose();
    }
}
