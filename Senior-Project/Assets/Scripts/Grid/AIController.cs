using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour
{

    [SerializeField]
    private Vector2 target, position;
    
    public float speed;
    public bool IsStale { get; private set; }

    private Queue<Vector2> path = new();
    [SerializeField]
    private int _pathQueuePosition = -1;
    private float _updateFrequency = 0.5f;

    private bool _turnForPath = false;

    public void Start()
    {
        Time.fixedDeltaTime = _updateFrequency;
        target = new Vector2(10, 10);
        InvokeRepeating(nameof(RequestNewPath),0,_updateFrequency);
    }

    public void Update()
    {
        position = transform.position;


        if(path.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, path.Peek(), speed);
            if (Vector2.Distance(position, path.Peek()) < 0.1f)
            {
                path.Dequeue();
            }
        }
            
    }

    private void RequestNewPath()
    {
        Debug.Log("test");
       
        if(Vector2.Distance(position,target) < 0.25f)
        {
            return;
        }
        if (_pathQueuePosition == -1)
        {
            try 
            {
                _pathQueuePosition = PathingManager.Instance.QueuePath(target, new(transform.position.x, transform.position.z));
                _turnForPath = true;
            }
            catch
            {

            }
                    
        }
        else
        {
            if (_turnForPath)
            {
                path = PathingManager.Instance.Paths[_pathQueuePosition];
                _turnForPath = false;
            }
        }


           
    }

    public void SetDestination(Vector2 destination)
    {
        target = destination;
    }

    public void SetTarget(ref Transform target)
    {
        this.target = target.position;
    }




}
