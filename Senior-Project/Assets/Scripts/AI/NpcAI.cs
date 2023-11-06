using System.Collections.Generic;
using UnityEngine;
using static PathingManager;

public class NpcAI : AIController
{
    [SerializeField]
    private Vector2 _origin;
    public Vector2 Origin { get { return _origin; } set { _origin = value; } }
    [SerializeField]
    private Vector2 _pickUpTarget;
    public Vector2 PickUpTarget { get => _pickUpTarget; set => _pickUpTarget = value; }
    [SerializeField]
    public PathInfo _pathToGen;
    [SerializeField]
    public PathInfo _movingPath;
    public Stack<Vector2> _returnPath = new();

    [SerializeField]
    private bool _metDesination = false;

    // Start is called before the first frame update
    void Start()
    {
        ResetPathing();
        InvokeRepeating(nameof(TestIfNear), 10, 1);
    }

    new void Update()
    {
        base.Update();
        Debug.Log((_position2D - _pickUpTarget).magnitude < 1f);
        if ((_position2D - _pickUpTarget).magnitude < 1f && !_metDesination)
        {
            _metDesination = true;
            foreach (Vector2 path in _returnPath)
            {
                _movingPath.cleanedPath.Enqueue(path);
            }
        }


        _movingPath ??= Instance.RetrievePath(_pathToGen);

        if (_movingPath != null && _movingPath.cleanedPath.Count != 0)
        {
            Vector2 direction2D = (_movingPath.cleanedPath.Peek() - _position2D).normalized;
            Vector3 direction = new(direction2D.x, -1f, direction2D.y);
            _characterController.Move(direction * Speed * Time.deltaTime);
            if ((_movingPath.cleanedPath.Peek() - _position2D).sqrMagnitude < 0.2f)
            {
                _movingPath.cleanedPath.TryDequeue(out Vector2 result);
                if (result != null) _returnPath.Push(result);
            }
        }


    }

    public void TestIfNear()
    {
        if ((_origin - _position2D).sqrMagnitude < 0.2f)
        {
            _metDesination = false;
            ResetPathing();
        }
    }
    private void ResetPathing()
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
    }
}
