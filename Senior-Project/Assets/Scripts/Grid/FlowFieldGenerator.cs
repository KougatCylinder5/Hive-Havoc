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
    Vector2[,] flowTiles;
    Vector3 startPoint;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = GameObject.FindGameObjectWithTag("CommandCenter").transform.position;
        NativeList<FlowFieldJob.PathNode> positionsToCheck = new(Allocator.TempJob);
        NativeList<FlowFieldJob.PathNode> closedList = new(Allocator.TempJob);

        NativeList<JobHandle> handles = new NativeList<JobHandle>(Allocator.TempJob);
        while (positionsToCheck.Length > 0)
        {
            NativeList<FlowFieldJob.PathNode> newPositionsToCheck = new(positionsToCheck.Length * 4,Allocator.TempJob);
            FlowFieldJob job = new()
            {
                positionsToCheck = positionsToCheck.AsParallelReader(),
                newPositionsToCheck = newPositionsToCheck.AsParallelWriter(),
                closedList = closedList.AsParallelReader(),
                gridSize = PathingManager.GridSize
            };

            handles.Add(job.Schedule(positionsToCheck.Length, 32));
            JobHandle.CompleteAll(handles);
            foreach(FlowFieldJob.PathNode position in newPositionsToCheck)
            {
                positionsToCheck.Add(position);
                closedList.Add(position);
            }
                
            
            positionsToCheck.Clear();
            newPositionsToCheck.Dispose();
        }


        handles.Dispose();
        positionsToCheck.Dispose();
    }

    [BurstCompile]
    public struct FlowFieldJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<PathNode>.ReadOnly positionsToCheck;
        public NativeArray<PathNode>.ReadOnly closedList;
        public Vector2Int gridSize;
        [WriteOnly]
        public NativeList<PathNode>.ParallelWriter newPositionsToCheck;

        public void Execute(int index)
        {

            
            NativeArray<Vector2Int> neighbourOffsetArray = new(4, Allocator.Temp);
            neighbourOffsetArray[0] = new Vector2Int(-1,  0); // Left
            neighbourOffsetArray[1] = new Vector2Int(+1,  0); // Right
            neighbourOffsetArray[2] = new Vector2Int( 0, +1); // Up
            neighbourOffsetArray[3] = new Vector2Int( 0, -1); // Down


            foreach(Vector2Int neighbour in neighbourOffsetArray)
            {
                PathNode node = new()
                {
                    position = positionsToCheck[index].position + neighbour,
                    direction = positionsToCheck[index].position + neighbour
                    hCost = float.MaxValue,
                    gCost = positionsToCheck[index].gCost + 1,
                    isWalkable = true

                };
                node.CalculateFCost();
                
                if(node.IsPositionInsideGrid(gridSize) || closedList.Contains(node))
                {
                    continue;
                }
                

                if (node.isWalkable)
                {
                    node.direction -= neighbour;
                    node.direction = node.direction.normalized;
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

            public bool IsPositionInsideGrid(Vector2Int GridSize)
            {
                return
                    position.x >= 0 &&
                    position.y >= 0 &&
                    GridSize.x < GridSize.x &&
                    GridSize.y < GridSize.y;
            }
        }
        
        private int CalculateIndex(int x, int y, int gridWidth)
        {
            return x + y * gridWidth;
        }
        private int CalculateIndex(Vector2Int position, int gridWidth)
        {
            return position.x + position.y * gridWidth;
        }

    }

}
