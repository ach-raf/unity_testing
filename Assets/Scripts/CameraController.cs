using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    private PlayerControlls playerController;
    [SerializeField] public float movement_speed = 10f;

    private void Awake()
    {
        playerController = new PlayerControlls();
    }

    private void OnEnable()
    {
        playerController.Enable();

    }
    private void OnDisable()
    {
        playerController.Disable();
    }

    void FixedUpdate()
    {
        playerController.Player.Move.performed += ctx => MoveCamera(ctx.ReadValue<Vector2>());
    }
    private void MoveCamera(Vector2 direction)
    {

        Vector3 new_position = transform.position + new Vector3(direction.x, 0, direction.y) * movement_speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, new_position, Time.deltaTime);

    }
}
