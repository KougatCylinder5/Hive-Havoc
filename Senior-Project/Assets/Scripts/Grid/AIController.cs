using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Burst;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;

public class AIController : MonoBehaviour
{
    public static Vector2Int sizeOfGrid = new(1000, 1000);
    public static AIController Instance;
    public void Awake()
    {
        Instance = this;
    }
    public IEnumerable<JobHandle> GeneratePath(NativeList<Node> path, Vector2 target, Vector2 start)
    {
        NativeArray<Node> nodes = new(sizeOfGrid.x * sizeOfGrid.y, Allocator.TempJob);

        
        for (int x = 0; x < sizeOfGrid.x; x++)
        {
            for (int y = 0; y < sizeOfGrid.y; y++)
            {
                nodes[x + y * sizeOfGrid.x] = new Node()
                {
                    position = new Vector2Int(x, y),
                    index = x + y * sizeOfGrid.x,
                    isWalkable = true,
                    cameFromIndex = -1,
                    gCost = int.MaxValue
                };
            }
        }
        JobHandle task = new GeneratedPath()
        {
            start = start,
            end = target,
            path = path,
            nodes = nodes
        }.Schedule();
        yield return task;
        task.Complete();
        

    }

    public struct Node
    {
        public Vector2Int position;
        public int index;

        public int gCost;
        public int hCost;
        public int fCost;//x:g y:h z:f

        public bool isWalkable;

        public int cameFromIndex;


        public void FCost()
        {
            fCost = hCost + gCost;
        }

        public Node GetInstance()
        {
            Node copy = new() {position = position,index = index, isWalkable = isWalkable};
            return copy;
        }
        public override string ToString()
        {
            return string.Format("Pos: {0}", position);
        }
    }
    
    private struct GeneratedPath : IJob
    {
        public Vector2 start; public Vector2 end;

        public NativeList<Node> path;

        public NativeArray<Node> nodes;


        //[BurstCompile]
        public void Execute()
        {
            NativeList<int> openList = new(Allocator.Temp), closedList = new(Allocator.Temp);

            for (int i = 0; i < nodes.Length; i++)
            {
                Node node = nodes[i];
                node.hCost = CalculateDistanceCost(node.position, end);
                node.FCost();
                nodes[i] = node;
            }

            NativeArray<Vector2Int> neighborOffsetArray = new(8, Allocator.Temp);
            neighborOffsetArray[0] = new(-1, 0);
            neighborOffsetArray[1] = new(+1, 0);
            neighborOffsetArray[2] = new(0, -1);
            neighborOffsetArray[3] = new(0, +1);
            neighborOffsetArray[4] = new(-1, -1);
            neighborOffsetArray[5] = new(+1, +1);
            neighborOffsetArray[6] = new(-1, +1);
            neighborOffsetArray[7] = new(+1, -1);

            int endNodeIndex = CalcuateIndex(end);

            Node startNode = nodes[CalcuateIndex(start)];
            startNode.gCost = 0;
            nodes[CalcuateIndex(start)] = startNode;

            openList.Add( startNode.index );

            while (openList.Length > 0)
            {
                int currentNodeIndex = GetLosestCostFNodeIndex(openList, nodes);
                Node currentNode = nodes[currentNodeIndex];
                if (currentNodeIndex == CalcuateIndex(end))
                {
                    endNodeIndex = currentNodeIndex;
                    break;
                }
                for (int i = 0; i < openList.Length; i++)
                {
                    if (openList[i] == currentNodeIndex)
                    {
                        openList.RemoveAt(i);
                        break;
                    }
                }
                closedList.Add(currentNodeIndex);

                for (int i = 0; i < neighborOffsetArray.Length; i++)
                {
                    Vector2Int offset = neighborOffsetArray[i];
                    Vector2Int neighborPos = currentNode.position + offset;
                    int neighbourIndex = -1;
                    if (!IsInsideBounds(neighborPos)) {
                        neighbourIndex = CalcuateIndex(neighborPos);
                    }
                    
                    if (closedList.Contains(neighbourIndex))
                    {
                        continue;
                    }
                    Node neighborNode = new();
                    try
                    {
                        neighborNode = nodes[neighbourIndex];
                    }
                    catch
                    {
                        continue;
                    }

                    if (!neighborNode.isWalkable)
                    {
                        continue;
                    }

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode.position, neighborPos);
                    if (tentativeGCost < neighborNode.gCost)
                    {
                        neighborNode.cameFromIndex = currentNodeIndex;
                        neighborNode.gCost = tentativeGCost;
                        neighborNode.FCost();
                        nodes[neighbourIndex] = neighborNode;

                        if (!openList.Contains(neighborNode.index))
                        {
                            openList.Add(neighbourIndex);
                        }
                    }
                }
            }

            Node endNode = nodes[endNodeIndex];
            path.Add(endNode);
            if (endNode.cameFromIndex != -1)
            {
                Node nextNode = nodes[endNode.cameFromIndex];
                while (nextNode.cameFromIndex != -1)
                {
                    path.Add(nextNode);
                    nextNode = nodes[nextNode.cameFromIndex];
                }
            }
            neighborOffsetArray.Dispose();

        }
        public int CalculateDistanceCost(Vector2 cur, Vector2 end)
        {
            return Mathf.RoundToInt(Vector2.Distance(cur, end) * 10);
        }
        private int GetLosestCostFNodeIndex(NativeList<int> openList, NativeArray<Node> nodes)
        {
            Node lowestCostPathNode = nodes[openList[0]];
            for (int i = 0; i < openList.Length; i++)
            {
                Node testNode = nodes[openList[i]];
                if (testNode.fCost < lowestCostPathNode.fCost)
                {
                    lowestCostPathNode = testNode;
                }
            }
            return lowestCostPathNode.index;
        }
        public int CalcuateIndex(Vector2 position)
        {
            return Mathf.RoundToInt(position.x) + Mathf.RoundToInt(position.y) * sizeOfGrid.x;
        }
        private bool IsInsideBounds(Vector2 pos)
        {
            return pos.x > 0 &&
                pos.y > 0 &&
                pos.x > sizeOfGrid.x &&
                pos.y > sizeOfGrid.y;
        }
       
    }
}
