using System.Collections;
using UnityEngine;
using static PathingManager;

[RequireComponent(typeof(LineRenderer))]
public class UnitAI : AIController, IAIBasics
{
    public Vector2 Target { get => _target; set => _target = value; }
    private PathInfo _lastPathGenerated = null;

    private Vector2 _movedLastFrame, _lastPosition2D;

    private LineRenderer _pathRenderer;

    public new void Awake()
    {
        base.Awake();
        _pathRenderer = GetComponent<LineRenderer>();
        _pathRenderer.endWidth = 0.3f;
        _pathRenderer.startWidth = 0.1f;
        InvokeRepeating(nameof(UpdatePath), 0, _updateFrequency);
        _lastPosition2D = _position2D;
        StartCoroutine(nameof(DistanceMoved));
    }
    
    public void ExecutePath()
    {
        if (Vector2.Distance(_position2D, Target) < 0.1f)
        {
            Target = _position2D;
            return;
        }



        if (_pathInfo != null && _pathInfo.cleanedPath.Count != 0)
        {
            Vector2 direction2D = (_pathInfo.cleanedPath.Peek() - _position2D).normalized;
            Vector3 direction = new(direction2D.x, -1f, direction2D.y);
            _characterController.Move(Mathf.Clamp(speed,0.25f,Vector3.Distance(_position2D, Target) / Time.deltaTime) * Time.deltaTime * direction);
            if ((_pathInfo.cleanedPath.Peek() - _position2D).sqrMagnitude < 0.02f)
            {
                _pathInfo.cleanedPath.TryDequeue(out Vector2 _);
            }
        }
    }

    private new void Update()
    {
        base.Update();
        DisplayLine();
        ExecutePath();
    }

    public void UpdatePath()
    {

        float distanceToTarget = Vector2.Distance(_position2D, Target);
        if (distanceToTarget < 0.1f)
        {
            _pathInfo.cleanedPath.Clear();
            return;
        }
        Debug.Log(_movedLastFrame.magnitude);
        if (distanceToTarget < 0.5f && _movedLastFrame.magnitude < 0.02f)
        {
            Target = _position2D;
            return;
        }
        if (_lastPathGenerated is null)
        {
            _lastPathGenerated = new PathInfo() { Start = _position2D, End = Target };

            Instance.QueuePath(_lastPathGenerated);
        }
        if (Instance.RetrievePath(_lastPathGenerated) != null)
        {
            _pathInfo = Instance.RetrievePath(_lastPathGenerated);
            _lastPathGenerated = null;
        }
        Debug.Log(_pathInfo);
    }


    private void DisplayLine()
    {
        _pathRenderer.positionCount = 2;
        _pathRenderer.SetPosition(0, _position);
        _pathRenderer.SetPosition(1, PathInfo.ConvertToVector3(_target, _position.y));
        _pathRenderer.Simplify(0.5f);
    }

    private IEnumerator DistanceMoved()
    {
        while (true)
        {
            _movedLastFrame = _position2D - _lastPosition2D;
            _lastPosition2D = _position2D;
            yield return new WaitForSeconds(_updateFrequency * 2);
        }
    }
}

