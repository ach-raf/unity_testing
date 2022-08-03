using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    private PlayerControlls playerController;
    [SerializeField] public float movement_speed = 10f;
    [SerializeField] public float movement_time = 10f;
    [SerializeField] public Vector3 move_input;
    private Vector3 new_position;

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

    }

    void FixedUpdate()
    {
        HandleMouvementInput();
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

        Debug.Log("Move");
        new_position += Vector2ToVector3((move_input * movement_speed));

        transform.position = Vector3.Lerp(transform.position, new_position, Time.deltaTime);
    }
    public Vector3 Vector2ToVector3(Vector2 vector2)
    {
        return new Vector3(vector2.x, 0, vector2.y);
    }

    //public void MovmentsChanged(InputAction.CallbackContext ctx)

}
