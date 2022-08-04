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
    [SerializeField] public float mouse_sensitivity = 10f;
    [SerializeField] public Vector2 mouse_move_input;
    private Vector3 new_position;
    private float x_rotation;

    private void Awake()
    {
        playerController = new PlayerControlls();
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
        HandleMouvementInput();
        Look();
        //playerController.Player.Move.performed += ctx => MoveCamera(ctx.ReadValue<Vector2>());
    }
    private void MoveCamera(Vector2 direction)
    {

        Vector3 new_position = Vector2ToVector3((direction * movement_speed));
        transform.position = Vector3.Lerp(transform.position, new_position, Time.deltaTime);

    }
    public void HandleMouvementInput()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //Debug.Log("Move");
        new_position += Vector2ToVector3((move_input * movement_speed));

        transform.position = Vector3.Lerp(transform.position, new_position, Time.deltaTime);
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
        //transform.localRotation = Quaternion.Euler(x_rotation, 0f, 0f);


        //transform.rotation = Quaternion.Euler(0, x_rotation, 0);
    }
    public Vector3 Vector2ToVector3(Vector2 vector2)
    {
        return new Vector3(vector2.x, 0, vector2.y);
    }

    //public void MovmentsChanged(InputAction.CallbackContext ctx)

}
