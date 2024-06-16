using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hermit : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float holdOffsetY = 1f;
    private InputActions gameInputs;
    private InputAction moveAction;
    private GameObject heldItem;
    private float heldItemInitialY;

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
        if(movement.x != 0f) {
            gameObject.GetComponent<SpriteRenderer>().flipX = movement.x < 0f;
        }
        var translate = speed * Time.deltaTime * movement;
        transform.Translate(translate);
        if(heldItem != null) {
            heldItem.transform.Translate(translate);
        }
    }

    private void OnGrab(InputAction.CallbackContext context)
    {
        if(heldItem != null) {
            heldItem.transform.Translate(0f, -holdOffsetY, 0f);
            // heldItem.transform.position.Set(0f, heldItemInitialY, 0f);
            heldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            heldItem = null; // drop
            return;
        }
        heldItem = GetCollidingObjectWithTag("Draggable");
        if(heldItem == null) return;
        heldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Held";
        // heldItemInitialY = heldItem.transform.position.y; // remember initail height for dropping later
        heldItem.transform.Translate(0f, holdOffsetY, 0f);
    }

    GameObject GetCollidingObjectWithTag(string tag)
    {
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<Collider2D> results = new List<Collider2D>();
        
        int colliderCount = GetComponent<Collider2D>().OverlapCollider(filter, results);

        for (int i = 0; i < colliderCount; i++)
        {
            if (results[i].CompareTag(tag))
            {
                return results[i].gameObject;
            }
        }

        return null;
    }
}
