using System.Collections.Generic;
using UnityEngine;
using static PathingManager;

public class NpcAI : AIController
{
    [SerializeField]
    private Vector2 _origin;
    [SerializeField]
    private Vector2 _pickUpTarget;
    [SerializeField]
    public PathInfo _pathToGen;
    [SerializeField]
    public PathInfo _movingPath;
    public Stack<Vector2> _returnPath = new();


    private bool _metDesination = false;

    // Start is called before the first frame update
    void Start()
    {

        _origin = _position2D;
        _returnPath.Push(_origin);
        _pathToGen = new()
        {
            Start = _origin,
            End = _pickUpTarget
        };

        Instance.QueuePath(_pathToGen);
        _movingPath = null;
        InvokeRepeating(nameof(TestIfNear), 10, 1);
    }

    new void Update()
    {
        base.Update();

        if ((_position2D - _pickUpTarget).sqrMagnitude < 0.2f && !_metDesination)
        {
            _metDesination = true;
            foreach (var path in _returnPath)
            {
                _movingPath.cleanedPath.Enqueue(path);
            }
        }


        _movingPath ??= Instance.RetrievePath(_pathToGen);

        if (_movingPath != null && _movingPath.cleanedPath.Count != 0)
        {
            Vector2 direction2D = (_movingPath.cleanedPath.Peek() - _position2D).normalized;
            Vector3 direction = new(direction2D.x, -1f, direction2D.y);
            _characterController.Move(direction * speed * Time.deltaTime);
            if ((_movingPath.cleanedPath.Peek() - _position2D).sqrMagnitude < 0.2f){
                _movingPath.cleanedPath.TryDequeue(out Vector2 result);
                if(result != null) _returnPath.Push(result);
            }
        }


    }

    public void TestIfNear()
    {
        if ((_origin - _position2D).sqrMagnitude < 0.2f)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        ResourceStruct.Wood++;
    }
}
