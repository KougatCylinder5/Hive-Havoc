

using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class AIController : MonoBehaviour
{
    public Vector2 target;
    
    public float speed;
    public bool IsStale { get; private set; }

    private Queue<Vector2> path = new();

    public int _pathQueuePosition = -1;

    bool _turnForPath = false;

    public void Start()
    {
        Time.fixedDeltaTime = 1;
    }

    public void Update()
    {
        if (_pathQueuePosition == -1)
        {
            try
            {
                _pathQueuePosition = PathingManager.Instance.QueuePath(target, new(transform.position.x, transform.position.z));
                _turnForPath = true;
            }
            catch
            {
                Debug.Log("First Frame");
            }
        }
        else
        {
            if (_turnForPath)
            {
                path = PathingManager.Instance.Paths[_pathQueuePosition];
                _turnForPath = false;
                _pathQueuePosition = -1;
            }
            //foreach (var pathNode in path)
            //{
            //    Debug.Log(pathNode);
            //}
        }
    }

}
