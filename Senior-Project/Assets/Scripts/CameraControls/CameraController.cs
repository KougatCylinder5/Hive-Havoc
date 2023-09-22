using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed;
    public float movementTime;
    private Vector3 _newPosition;

    public Vector3 zoomAmount;
    private Vector3 _newZoom;
    public Transform cameraTransform;
    public int minZoomBound;
    public int maxZoomBound;

    private Vector3 _dragStartPosition;
    private Vector3 _dragCurrentPosition;

    public float speedConstant;
    public CameraBounds boundChecker;

    // Start is called before the first frame update
    void Start()
    {
        _newPosition = transform.position;
        _newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
        HandleMouseInput();
        SpeedChange(speedConstant);
        _newPosition = boundChecker.CheckBounds(_newPosition);
    }

    void HandleMovementInput()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _newPosition += transform.forward * movementSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _newPosition += transform.forward * -movementSpeed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _newPosition += transform.right * -movementSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _newPosition += transform.right * movementSpeed;
        }
        transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * movementTime);
    }

    void HandleMouseInput()
    {
        if((Input.mouseScrollDelta.y > 0 && _newZoom.y + zoomAmount.y > minZoomBound) || (Input.mouseScrollDelta.y < 0 && _newZoom.y - zoomAmount.y < maxZoomBound))
        {
            _newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }
        if(Input.GetMouseButtonDown(2))
        {
            _dragStartPosition = CameraRaycast();
        }
        if (Input.GetMouseButton(2))
        {
            _dragCurrentPosition = CameraRaycast();
            _newPosition = transform.position + _dragStartPosition - _dragCurrentPosition;
            
        }
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, _newZoom, Time.deltaTime * movementTime);
    }

    void SpeedChange(float speedConstant)
    {
        movementSpeed = _newZoom.y * speedConstant;
    }

    Vector3 CameraRaycast()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float entry;
        if (plane.Raycast(ray, out entry))
        {
            return ray.GetPoint(entry);
        }
        else
        {
            return Vector3.zero;
        }
    }
}