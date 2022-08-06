using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private PlayerControlls playerController;
    [SerializeField] public float movement_speed = 10f;
    [SerializeField] public float movement_time = 10f;
    [SerializeField] public float zoom_min_height = 10f;
    [SerializeField] public float zoom_max_height = 50f;
    [SerializeField] public float zoom_dampening = 50f;
    [SerializeField] public float zoom_speed = 50f;
    [SerializeField] public float zoom_step = 1f;
    [SerializeField] public Vector3 move_input;
    [SerializeField] int inputSensitivity = 100;
    [SerializeField] public float mouse_sensitivity = 10f;
    //static float movementIncrementOverTime = 0;

    [SerializeField] public Vector2 mouse_move_input;
    private Vector3 new_position;
    private float x_rotation;

    private float zoom_height;
    private Camera main_camera;

    private void Awake()
    {
        playerController = new PlayerControlls();
        main_camera = gameObject.GetComponentInChildren<Camera>();
    }

    private void OnEnable()
    {
        playerController.Enable();
        playerController.Player.Move.performed += PlayerMovePerformed;
        playerController.Player.Move.canceled += PlayerMoveCanceled;

        playerController.Player.Look.performed += PlayerLookPerformed;
        playerController.Player.Look.canceled += PlayerLookCanceled;

        playerController.Player.MouseZoom.performed += PlayerMouseZoomPerformed;
        playerController.Player.MouseZoom.canceled += PlayerMouseZoomCanceled;


    }
    private void OnDisable()
    {
        playerController.Disable();
        playerController.Player.Move.performed -= PlayerMovePerformed;
        playerController.Player.Move.canceled -= PlayerMoveCanceled;

        playerController.Player.Look.performed -= PlayerLookPerformed;
        playerController.Player.Look.canceled -= PlayerLookCanceled;

        playerController.Player.MouseZoom.performed -= PlayerMouseZoomPerformed;
        playerController.Player.MouseZoom.canceled -= PlayerMouseZoomCanceled;
    }
    private void Start()
    {
        new_position = transform.position;

    }

    public void PlayerMovePerformed(InputAction.CallbackContext context)
    {
        move_input = new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, move_input.z);
    }
    public void PlayerMoveCanceled(InputAction.CallbackContext context)
    {
        move_input = new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, move_input.z);
    }

    public void PlayerLookPerformed(InputAction.CallbackContext context)
    {
        mouse_move_input = context.ReadValue<Vector2>();
    }
    public void PlayerLookCanceled(InputAction.CallbackContext context)
    {
        mouse_move_input = context.ReadValue<Vector2>();

    }
    public void PlayerMouseZoomPerformed(InputAction.CallbackContext context)
    {
        float scroll_value = -context.ReadValue<Vector2>().y;
        MouseZoomLogic(scroll_value);
        /*if (zoom_height == zoom_min_height)
        {
            main_camera.fieldOfView -= 1;
        }
        else
        {
            main_camera.fieldOfView += 1;
        }*/


    }
    public void PlayerMouseZoomCanceled(InputAction.CallbackContext context)
    {
        //mouse_move_input = context.ReadValue<Vector2>();
        float scroll_value = context.ReadValue<Vector2>().y;
        MouseZoomLogic(scroll_value);

    }


    void FixedUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        MovePlayerRelativeToCamera();
        Look();
        UpdateMouseZoom();
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        //Robotic arm mouvement feeling
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (movement_speed * time) / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
    public void HandleMouvementInput()
    {
        new_position += Vector2ToVector3((move_input));
        transform.position = Vector3.Lerp(transform.position, new_position, (inputSensitivity * movement_speed * Time.deltaTime) / movement_time);
    }
    public void Look()
    {
        if (!Mouse.current.middleButton.IsPressed())
        {
            return;
        }
        Vector3 normalized_vector = mouse_move_input * .5f * .1f;
        x_rotation += mouse_move_input.x * mouse_sensitivity;
        transform.Rotate(0f, normalized_vector.x * mouse_sensitivity, 0f);
    }
    public Vector3 Vector2ToVector3(Vector2 vector2)
    {
        return new Vector3(vector2.x, 0, vector2.y);
    }


    public void MovePlayerRelativeToCamera()
    {

        // Get Player Input
        Vector2 moveInput = playerController.Player.Move.ReadValue<Vector2>();
        Vector3 verticalDirection = Vector3.zero;
        Vector3 horizontalDirection = Vector3.zero;

        // Get Camera Normalized Directional Vectors
        Vector3 cameraForward = main_camera.transform.forward;
        Vector3 cameraRight = main_camera.transform.right;

        // Set Y to 0
        cameraForward.y = 0;
        cameraRight.y = 0;

        // Calculate Movement Direction
        verticalDirection = cameraRight * moveInput.x;
        horizontalDirection = cameraForward * moveInput.y;

        Vector3 movementDirection = verticalDirection + horizontalDirection;

        // Move Player
        //transform.position += movementDirection * movement_speed * Time.deltaTime;
        this.transform.Translate(movementDirection * movement_speed * Time.deltaTime, Space.World);

    }

    public void MouseZoomLogic(float scroll_value)
    {
        if (Mathf.Abs(scroll_value) <= 0.1f)
        {
            return;
        }
        zoom_height = main_camera.transform.localPosition.y + scroll_value * zoom_step;
        if (zoom_height < zoom_min_height)
        {
            zoom_height = zoom_min_height;
        }
        else if (zoom_height > zoom_max_height)
        {
            zoom_height = zoom_max_height;
        }
    }

    public void UpdateMouseZoom()
    {
        Vector3 zoom_target = new Vector3(main_camera.transform.localPosition.x, zoom_height, main_camera.transform.localPosition.z);
        zoom_target -= zoom_speed * (zoom_height - main_camera.transform.localPosition.y) * main_camera.transform.forward;
        main_camera.transform.localPosition = Vector3.Lerp(main_camera.transform.localPosition, zoom_target, Time.deltaTime * zoom_dampening);
    }


}
