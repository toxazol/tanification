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
        gameInputs.Hermits.PickUp.performed += OnAction;
    }

    private void OnDisable()
    {
        // Disable the entire action map
        gameInputs.Hermits.Disable();

        // Unsubscribe from the interact action
        gameInputs.Hermits.PickUp.performed -= OnAction;
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

    private void OnAction(InputAction.CallbackContext context)
    {
        if(heldItem != null) {
            if(ExchangeShell()) return;
            var here = new Vector3(
                heldItem.transform.position.x, 
                heldItemInitialY, 0f);
            Drop(here);
        } else {
            Grab(GetCollidingObjectWithTag("EmptyShell"));
        }
        
    }

    private bool ExchangeShell() {
        var shell = GetCollidingObjectWithTag("CrabShell");
        if(shell == null) return false;
        int newSize = shell.GetComponent<Shell>().size;
        int diff =  heldItem.GetComponent<Shell>().size - newSize;
        if(diff == 1) {
            SwapShell(shell);
            return true;
        } else {
            Debug.Log("Your shell is too " + (diff > 0 ? "big" : "small") + " for this crab.");
        }
        return false;
    }

    private void SwapShell(GameObject otherCrabsShell) {
        otherCrabsShell.transform.GetChild(0).gameObject.SetActive(false);
        heldItem.transform.GetChild(0).gameObject.SetActive(true);
        otherCrabsShell.tag = "EmptyShell";
        heldItem.tag = "CrabShell";
        Drop(otherCrabsShell.transform.position);
        Grab(otherCrabsShell);
    }

    private void Grab(GameObject item) {
        if(item == null) return;
        heldItem = item;
        heldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Held";
        heldItemInitialY = heldItem.transform.position.y; // remember initail height for dropping later
        heldItem.transform.position = new Vector3(
                transform.position.x, 
                transform.position.y + holdOffsetY, 0f);
    }

    private void Drop(Vector3 pos) {
        heldItem.transform.position = pos;
        heldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        heldItem = null; // drop
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
