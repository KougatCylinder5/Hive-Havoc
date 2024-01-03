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
    public GameBounds boundChecker;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the camera's position and zoom
        _newPosition = transform.position;
        _newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
        HandleMouseInput();
        SpeedChange(speedConstant);
        _newPosition = boundChecker.StayInBounds(_newPosition);
    }

    void HandleMovementInput()
    {
        //WASD moves camera
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
        //Scrolling zooms camera
        if((Input.mouseScrollDelta.y > 0 && _newZoom.y + zoomAmount.y > minZoomBound) || (Input.mouseScrollDelta.y < 0 && _newZoom.y - zoomAmount.y < maxZoomBound))
        {
            _newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }
        //Dragging middle mouse button moves camera
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
    //When the camera is zoomed in, it moves slower, and vice versa.
    void SpeedChange(float speedConstant)
    {
        movementSpeed = _newZoom.y * speedConstant;
    }

    //Used for detecting how far the mouse is dragged for moving the camera
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