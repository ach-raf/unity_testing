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
    [SerializeField] public Vector3 move_input;
    [SerializeField] int inputSensitivity = 100;
    [SerializeField] public float mouse_sensitivity = 10f;
    static float movementIncrementOverTime = 0;

    [SerializeField] public Vector2 mouse_move_input;
    private Vector3 new_position;
    private float x_rotation;
    private Camera main_camera;

    private void Awake()
    {
        playerController = new PlayerControlls();
        main_camera = gameObject.GetComponentInChildren<Camera>();
    }

    private void OnEnable()
    {
        playerController.Enable();
        //playerController.Player.Move.performed += ctx => MoveCamera(ctx.ReadValue<Vector2>());

    }
    private void OnDisable()
    {
        playerController.Disable();
        //playerController.Player.Move.performed -= ctx => MoveCamera(ctx.ReadValue<Vector2>());
    }
    private void Start()
    {
        new_position = transform.position;
        playerController.Player.Move.performed += ctx =>
        {
            move_input = new Vector3(ctx.ReadValue<Vector2>().x, ctx.ReadValue<Vector2>().y, move_input.z);
        };
        playerController.Player.Move.canceled += ctx =>
        {
            move_input = new Vector3(ctx.ReadValue<Vector2>().x, ctx.ReadValue<Vector2>().y, move_input.z);
        };

        playerController.Player.Look.performed += ctx =>
        {
            mouse_move_input = ctx.ReadValue<Vector2>();
        };
        playerController.Player.Look.canceled += ctx =>
        {
            mouse_move_input = ctx.ReadValue<Vector2>();
        };

    }

    void FixedUpdate()
    {
        MovePlayerRelativeToCamera();
        //HandleMouvementInput();
        Look();
        //playerController.Player.Move.performed += ctx => MoveCamera(ctx.ReadValue<Vector2>());
    }
    private void MoveCamera(Vector2 direction)
    {

        Vector3 new_position = Vector2ToVector3((direction));
        transform.position = Vector3.Lerp(transform.position, new_position, (movement_speed * Time.deltaTime) / movement_time);

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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //Debug.Log("Move");
        new_position += Vector2ToVector3((move_input));
        transform.position = Vector3.Lerp(transform.position, new_position, (movement_speed * Time.deltaTime) / movement_time);
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
        //main_camera.transform.rotation = Quaternion.LookRotation(normalized_vector);
        //transform.localRotation = Quaternion.Euler(x_rotation, 0f, 0f);


        //transform.rotation = Quaternion.Euler(0, x_rotation, 0);
    }
    public Vector3 Vector2ToVector3(Vector2 vector2)
    {
        return new Vector3(vector2.x, 0, vector2.y);
    }

    //public void MovmentsChanged(InputAction.CallbackContext ctx)

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



    private float GetAxis()
    {
        float moveInput = playerController.Player.Move.ReadValue<float>();

        // Get Camera Normalized Directional Vectors
        Vector3 cameraForward = main_camera.transform.forward;
        Vector3 cameraRight = main_camera.transform.right;

        if (moveInput == 1)
        {
            Vector3 verticalDirection = cameraForward * moveInput;
        }
        if (moveInput == -1)
        {
            Vector3 horizontalDirection = cameraRight * moveInput;
        }

        switch (moveInput)
        {
            case 1: // Vertical
                moveInput = Mathf.Lerp(0, moveInput, movementIncrementOverTime);
                movementIncrementOverTime += (0.05f * inputSensitivity) * Time.deltaTime;
                break;

            case -1: // Horizontal
                moveInput = Mathf.Lerp(0, moveInput, movementIncrementOverTime);
                movementIncrementOverTime += (0.05f * inputSensitivity) * Time.deltaTime;
                break;

            default:
                break;
        }

        return moveInput;
    }

    private void Movement_canceled(InputAction.CallbackContext obj)
    {
        movementIncrementOverTime = 0;
    }

}
