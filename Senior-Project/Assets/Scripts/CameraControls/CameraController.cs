using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed;
    public float movementTime;
    public Vector3 newPosition;

    public Vector3 zoomAmount;
    private Vector3 newZoom;
    public Transform cameraTransform;
    public int minZoomBound;
    public int maxZoomBound;

    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;

    public float speedConstant;
    public CameraBounds boundChecker;

    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
        HandleMouseInput();
        SpeedChange(speedConstant);
        newPosition = boundChecker.CheckBounds(newPosition);
    }

    void HandleMovementInput()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += transform.forward * movementSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += transform.forward * -movementSpeed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += transform.right * -movementSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += transform.right * movementSpeed;
        }
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }

    void HandleMouseInput()
    {
        if((Input.mouseScrollDelta.y > 0 && newZoom.y + zoomAmount.y > minZoomBound) || (Input.mouseScrollDelta.y < 0 && newZoom.y - zoomAmount.y < maxZoomBound))
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }
        if(Input.GetMouseButtonDown(2))
        {
            dragStartPosition = CameraRaycast();
        }
        if (Input.GetMouseButton(2))
        {
            dragCurrentPosition = CameraRaycast();
            newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            
        }
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

    void SpeedChange(float speedConstant)
    {
        movementSpeed = newZoom.y * speedConstant;
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