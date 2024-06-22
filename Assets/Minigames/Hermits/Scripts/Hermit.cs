using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hermit : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Vector2 holdOffset = new Vector2(2f, 2.5f);
    [SerializeField] private GameObject myShell;
    [SerializeField] private GameObject myClaws;
    [SerializeField] private GameObject me;
    [SerializeField] private List<AudioClip> steps;
    private InputActions gameInputs;
    private InputAction moveAction;
    private AudioSource sound;
    private GameObject heldItem;
    private float heldItemInitialY;
    private bool isLookLeft = false;

    private void Awake()
    {
        // Initialize the auto-generated class
        gameInputs = new InputActions();

        sound = GetComponent<AudioSource>();
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
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Move(moveInput);
    }

    private void Move(Vector2 movement)
    {
        if(movement.x == 0f) {
            me.GetComponent<Animator>().SetBool("isWalk", false);
            return;
        }

        if(!sound.isPlaying) {
            sound.PlayOneShot(steps[Random.Range(0, steps.Count)]);
        }

        me.GetComponent<Animator>().SetBool("isWalk", true);

        isLookLeft = movement.x < 0f; // flip sprites if move left
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
            transform.position.x + holdOffset.x * (isLookLeft ? -1f : 1f) , // * item.transform.localScale.x
            transform.position.y + holdOffset.y, 0f);
    }

    private void OnAction(InputAction.CallbackContext context)
    {
        if(heldItem != null) { // try wear or exchange or drop shell
            if(Wear(heldItem) || ExchangeShell()) {
                return;
            }
            var here = new Vector3(
                heldItem.transform.position.x, 
                heldItemInitialY, 0f);
            Drop(here);
        } else {
            // claws up or down
            me.GetComponent<Animator>().SetBool("isGrab", !myClaws.activeSelf);
            myClaws.SetActive(!myClaws.activeSelf);
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
            SwapShells(heldItem, shell);
            return true;
        } else {
            Debug.Log("Your shell is too " + (diff > 0 ? "big" : "small") + " for this crab.");
        }
        return false;
    }

    private void SwapShells(GameObject shell1, GameObject shell2) {
        Transform temp = shell1.transform.parent;
        shell1.transform.SetParent(shell2.transform.parent);
        shell2.transform.SetParent(temp);

        shell1.transform.GetPositionAndRotation(out Vector3 tempPosition, out Quaternion tempRotation);
        shell1.transform.SetPositionAndRotation(shell2.transform.position, shell2.transform.rotation);
        shell2.transform.SetPositionAndRotation(tempPosition, tempRotation);
        
        //swap sprite x flips
        (shell1.GetComponent<SpriteRenderer>().flipX, shell2.GetComponent<SpriteRenderer>().flipX) =
            (shell2.GetComponent<SpriteRenderer>().flipX, shell1.GetComponent<SpriteRenderer>().flipX);
        // swap sorting layers
        (shell1.GetComponent<SpriteRenderer>().sortingLayerID, shell2.GetComponent<SpriteRenderer>().sortingLayerID) =
            (shell2.GetComponent<SpriteRenderer>().sortingLayerID, shell1.GetComponent<SpriteRenderer>().sortingLayerID);
        //swap tags
        (shell1.tag, shell2.tag) = (shell2.tag, shell1.tag);
        //play swap animation on given out object
        shell1.GetComponentInChildren<ParticleSystem>(true).gameObject.SetActive(true);
        // update held item reference
        heldItem = shell2;
    }
    private void Grab(GameObject item) {
        if(item == null) return;
        me.GetComponent<Animator>().SetBool("isGrab", true);
        myClaws.SetActive(true);
        heldItem = item;
        heldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Held";
        heldItemInitialY = heldItem.transform.position.y; // remember initail height for dropping later
        HoldUp(heldItem);
    }

    private void Drop(Vector3 pos) {
        me.GetComponent<Animator>().SetBool("isGrab", false);
        myClaws.SetActive(false);
        heldItem.transform.position = pos;
        heldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Drop";
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
