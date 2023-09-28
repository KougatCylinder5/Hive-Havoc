using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class FlowFieldGenerator : MonoBehaviour
{
    FlowFieldJob.PathNode[,] flowTiles;
    Vector3 startPoint;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = GameObject.FindGameObjectWithTag("CommandCenter").transform.position;
        NativeList<FlowFieldJob.PathNode> positionsToCheck = new(Allocator.TempJob) { new() { position = new((int)startPoint.x, (int)startPoint.z), direction = Vector2.zero, gCost = 0, fCost = 0, hCost = 0, isWalkable = true } };
        NativeList<FlowFieldJob.PathNode> closedList = new(Allocator.TempJob);

        NativeList<JobHandle> handles = new NativeList<JobHandle>(Allocator.TempJob);
        while (positionsToCheck.Length > 0)
        {
            NativeList<FlowFieldJob.PathNode> newPositionsToCheck = new(positionsToCheck.Length * 8, Allocator.TempJob);
            FlowFieldJob job = new()
            {
                startPosition = new((int)startPoint.x, (int)startPoint.z),
                positionsToCheck = positionsToCheck.AsParallelReader(),
                newPositionsToCheck = newPositionsToCheck.AsParallelWriter(),
                closedList = closedList.AsParallelReader(),
                gridSize = PathingManager.GridSize
            };

            handles.Add(job.Schedule(positionsToCheck.Length, 32));
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
        }

        closedList.Dispose();
        handles.Dispose();
        positionsToCheck.Dispose();
    }

    //[BurstCompile]
    public struct FlowFieldJob : IJobParallelFor
    {
        [ReadOnly]
        public Vector2Int startPosition;
        public NativeArray<PathNode>.ReadOnly positionsToCheck;
        public NativeArray<PathNode>.ReadOnly closedList;
        public Vector2Int gridSize;
        [WriteOnly]
        public NativeList<PathNode>.ParallelWriter newPositionsToCheck;

        public void Execute(int index)
        {


            NativeArray<Vector2Int> neighbourOffsetArray = new(8, Allocator.Temp);
            neighbourOffsetArray[0] = new Vector2Int(-1, 0); // Left
            neighbourOffsetArray[1] = new Vector2Int(+1, 0); // Right
            neighbourOffsetArray[2] = new Vector2Int(0, +1); // Up
            neighbourOffsetArray[3] = new Vector2Int(0, -1); // Down
            neighbourOffsetArray[4] = new Vector2Int(-1, -1); // Left
            neighbourOffsetArray[5] = new Vector2Int(+1, -1); // Right
            neighbourOffsetArray[6] = new Vector2Int(-1, +1); // Up
            neighbourOffsetArray[7] = new Vector2Int(-1, -1); // Down


            foreach (Vector2Int neighbour in neighbourOffsetArray)
            {
                PathNode node = new()
                {
                    position = positionsToCheck[index].position + neighbour,
                    direction = -neighbour,
                    hCost = Vector2.Distance(startPosition, positionsToCheck[index].position + neighbour),
                    gCost = positionsToCheck[index].gCost + 1,
                    isWalkable = true

                };
                node.CalculateFCost();
                if (!node.IsPositionInsideGrid(node.position, gridSize) || closedList.Contains(node))
                {
                    continue;
                }


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

    }

}
