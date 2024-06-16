using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hermit : MonoBehaviour
{
    public float speed = 5f;
    private InputActions gameInputs;
    private InputAction moveAction;
private void Awake()
    {
        // Initialize the auto-generated class
        gameInputs = new InputActions();
    }

    private void OnEnable()
    {
        // Enable the entire action map
        gameInputs.Hermits.Enable();

        // Cache the move action for efficiency
        moveAction = gameInputs.Hermits.Move;

        // Subscribe to the interact action
        gameInputs.Hermits.PickUp.performed += OnGrab;
    }

    private void OnDisable()
    {
        // Disable the entire action map
        gameInputs.Hermits.Disable();

        // Unsubscribe from the interact action
        gameInputs.Hermits.PickUp.performed -= OnGrab;
    }

    private void Update()
    {
        // Read value from the Move action
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        // Use moveInput to move your character
        Move(moveInput);
    }

    private void Move(Vector2 movement)
    {
        if(movement.x != 0) {
            gameObject.GetComponent<SpriteRenderer>().flipX = movement.x < 0;
        }
        
        transform.Translate(movement * Time.deltaTime * speed);
    }

    private void OnGrab(InputAction.CallbackContext context)
    {
        Debug.Log("Grab action performed!");
        // if(GetComponent<Collider2D>().) {

        // }
    }
}
