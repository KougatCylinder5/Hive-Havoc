using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Burst;

public class AIController : MonoBehaviour
{
    Queue<PathInfo> pathGeneration = new();

    public static Vector2Int sizeOfGrid = new(100, 100);

    static NativeArray<Node> nodes = new NativeArray<Node>(sizeOfGrid.x * sizeOfGrid.y,Allocator.Persistent);


    public void Awake()
    {
        for (int x = 0; x < sizeOfGrid.x; x++)
        {
            for(int y = 0 ; y < sizeOfGrid.y ; y++)
            {
                nodes[x + y * sizeOfGrid.x] = new Node()
                {
                    position = new Vector2Int(x, y),
                    index = x + y * sizeOfGrid.x,
                    isWalkable = true,
                    cameFromIndex = -1
                };
            }
        }

    }

    private void Start()
    {
        NativeList<JobHandle> AIJobs = new(0,Allocator.Temp);
        NativeList<Node> path = new NativeList<Node>();
        while (pathGeneration.Count > 0)
        {
            AIJobs.Add(new GeneratedPath()
            {
                start = pathGeneration.Peek().start,
                end = pathGeneration.Peek().end,
                path = path
            }.Schedule());
            pathGeneration.Dequeue();
        }
        JobHandle.CompleteAll(AIJobs);
        AIJobs.Dispose();

        
    }
    
    

    
    private struct Node
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
        
    }
    [BurstCompile]
    private struct GeneratedPath : IJob
    {
        public Vector2Int start; public Vector2Int end;

        public NativeList<Node> path;

        public void Execute()
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                Node node = nodes[i];
                node.cameFromIndex = -1;
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

            Node startNode = nodes[CalcuateIndex(start)].GetInstance();
            startNode.gCost = 0;
            List<int> openList = new List<int>();
            List<int> closedList = new List<int>();

            openList.Add(startNode.index);

            while (openList.Count > 0)
            {
                int currentNodeIndex = GetLosestCostFNodeIndex(openList, nodes);
                Node currentNode = nodes[currentNodeIndex];
                if (currentNodeIndex == CalcuateIndex(end))
                {
                    break;
                }
                openList.Remove(currentNodeIndex);
                closedList.Add(currentNodeIndex);

                for (int i = 0; i < neighborOffsetArray.Length; i++)
                {
                    Vector2Int offset = neighborOffsetArray[i];
                    Vector2Int neighborPos = currentNode.position + offset;
                    int neighbourIndex = -1;
                    try
                    {
                        neighbourIndex = CalcuateIndex(neighborPos);
                    }
                    catch
                    {
                        continue;
                    }
                    if (closedList.Contains(neighbourIndex))
                    {
                        continue;
                    }

                    Node neighborNode = nodes[neighbourIndex];

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
                            openList.Add(neighborNode.index);
                        }
                    }
                }
            }

            Node endNode = nodes[endNodeIndex];
            path.Add(endNode);
            Node nextNode = nodes[endNode.cameFromIndex];
            do
            {
                path.Add(nextNode);
                nextNode = nodes[nextNode.cameFromIndex];
            } while (nextNode.index != -1);
            neighborOffsetArray.Dispose();
            path.Dispose();

        }
        public int CalculateDistanceCost(Vector2 cur, Vector2 end)
        {
            return Mathf.RoundToInt(Vector2.Distance(cur, end) * 10);
        }
        private int GetLosestCostFNodeIndex(List<int> openList, NativeArray<Node> nodes)
        {
            Node lowestCostPathNode = nodes[openList[0]];
            for (int i = 0; i < openList.Count; i++)
            {
                Node testNode = nodes[openList[i]];
                if (testNode.fCost < lowestCostPathNode.fCost)
                {
                    lowestCostPathNode = testNode;
                }
            }
            return lowestCostPathNode.index;
        }
        public int CalcuateIndex(Vector2Int position)
        {
            return position.x + position.y * sizeOfGrid.x;
        }
    }
}
