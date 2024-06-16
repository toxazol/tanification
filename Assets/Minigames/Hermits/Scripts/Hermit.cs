using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hermit : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Vector2 holdOffset = new Vector2(2f, 2.5f);
    [SerializeField] private GameObject myShell;
    private InputActions gameInputs;
    private InputAction moveAction;
    private GameObject heldItem;
    private float heldItemInitialY;
    private bool isLookLeft = false;

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
        if(movement.x == 0f) return;

        isLookLeft = movement.x < 0f; // flip sprites if move left
        gameObject.GetComponent<SpriteRenderer>().flipX = isLookLeft;
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).GetComponent<SpriteRenderer>().flipX = isLookLeft;
        }

        var translate = speed * Time.deltaTime * movement;
        transform.Translate(translate);
        HoldUp(heldItem);
    }

    private void HoldUp(GameObject item) {
        if(item == null) return;
        item.GetComponent<SpriteRenderer>().flipX = isLookLeft;
        item.transform.position = new Vector3(
            transform.position.x + holdOffset.x * (isLookLeft ? -1f : 1f),
            transform.position.y + holdOffset.y, 0f);
    }

    private void OnAction(InputAction.CallbackContext context)
    {
        if(heldItem != null) { // try wear or exchange or drop shell
            if(Wear(heldItem)) return;
            if(ExchangeShell()) return;
            var here = new Vector3(
                heldItem.transform.position.x, 
                heldItemInitialY, 0f);
            Drop(here);
        } else {
            Grab(GetCollidingObjectWithTag("EmptyShell"));
        }
        
    }

    private bool Wear(GameObject shell) {
        int diff = shell.GetComponent<Shell>().size - myShell.GetComponent<Shell>().size;
        if(diff != 1) return false;
        SwapShells(shell, myShell);
        var here = new Vector3(shell.transform.position.x, heldItemInitialY, 0f);
        Drop(here);
        Debug.Log("Congratulations, you found your new home!");
        return true;
    }

    private bool ExchangeShell() {
        var shell = GetCollidingObjectWithTag("CrabShell");
        if(shell == null) return false;
        int newSize = shell.GetComponent<Shell>().size;
        int diff =  heldItem.GetComponent<Shell>().size - newSize;
        if(diff == 1) {
            // SwapShell(shell);
            SwapShells(shell, heldItem);
            return true;
        } else {
            Debug.Log("Your shell is too " + (diff > 0 ? "big" : "small") + " for this crab.");
        }
        return false;
    }

    private void SwapShells(GameObject shell1, GameObject shell2) {
        // swap spites
        (shell1.GetComponent<SpriteRenderer>().sprite, shell2.GetComponent<SpriteRenderer>().sprite) = 
            (shell2.GetComponent<SpriteRenderer>().sprite, shell1.GetComponent<SpriteRenderer>().sprite);
        // swap sizes
        (shell1.GetComponent<Shell>().size, shell2.GetComponent<Shell>().size) =
            (shell2.GetComponent<Shell>().size, shell1.GetComponent<Shell>().size);
    }

    // private void SwapShell(GameObject otherCrabShell) {
    //     otherCrabShell.transform.GetChild(0).gameObject.SetActive(false);
    //     heldItem.transform.GetChild(0).gameObject.SetActive(true);
    //     otherCrabShell.tag = "EmptyShell";
    //     heldItem.tag = "CrabShell";
    //     Drop(otherCrabShell.transform.position);
    //     Grab(otherCrabShell);
    // }

    private void Grab(GameObject item) {
        if(item == null) return;
        heldItem = item;
        heldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Held";
        heldItemInitialY = heldItem.transform.position.y; // remember initail height for dropping later
        HoldUp(heldItem);
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
