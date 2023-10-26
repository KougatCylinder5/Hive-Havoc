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


    private bool _metDesination = false;

    // Start is called before the first frame update
    void Start()
    {

        _origin = _position2D;
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
            _pathToGen = new()
            {
                Start = _pickUpTarget,
                End = _origin
            };
            Instance.QueuePath(_pathToGen);
            _movingPath = null;
        }


        _movingPath ??= Instance.RetrievePath(_pathToGen);

        if (_movingPath != null && _movingPath.cleanedPath.Count != 0)
        {
            Vector2 direction2D = (_movingPath.cleanedPath.Peek() - _position2D).normalized;
            Vector3 direction = new(direction2D.x, -1f, direction2D.y);
            _characterController.Move(direction * speed * Time.deltaTime);
            if ((_movingPath.cleanedPath.Peek() - _position2D).sqrMagnitude < 0.2f){
                _movingPath.cleanedPath.Dequeue();
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
}
