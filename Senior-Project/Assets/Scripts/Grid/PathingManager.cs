using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class PathingManager : MonoBehaviour
{
    // Start is called before the first frame update
    Queue<AIController.Node> path = new Queue<AIController.Node>();
    IEnumerable<JobHandle> pathingJob;
    NativeList<AIController.Node> rawPath = new NativeList<AIController.Node>(Allocator.TempJob);
    public Vector2 start;
    public Vector2 end;
    void Start()
    {
        InvokeRepeating(nameof(GetPath), 0, 0.1f);
        IEnumerable<JobHandle> pathingJob = AIController.Instance.GeneratePath(rawPath, end, start);
    }

    // Update is called once per frame
    void Update()
    {
        if (path.Count > 0)
        {
            if (transform.position.Equals(path.Peek().position))
            {
                path.Dequeue();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(path.Peek().position.x, 0, path.Peek().position.y), Time.deltaTime * 1);
            }
        }
    }

    private void GetPath()
    {
        if (pathingJob.Cast<JobHandle>().ElementAt(0).IsCompleted)
        {
            foreach(AIController.Node node in rawPath)
            {
                path.Enqueue(node);
            }
            rawPath.Dispose();
            rawPath = new(Allocator.TempJob);
        }
        //yield return new WaitForSecondsRealtime(0.1f);
    }
}
