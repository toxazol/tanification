using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Blobot : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private InputActions gameInputs;
    private InputAction moveAction;
    private bool isLookLeft = false;
    private Rigidbody2D rBody;

    private void Start() 
    {
        rBody = GetComponent<Rigidbody2D>();
        // pause
        GlobalEventManager.Instance.AddListener("BulletHell:Death", ()=>{
            Time.timeScale = 0f;
        });
        GlobalEventManager.Instance.AddListener("BulletHell:Win", ()=>{
            Time.timeScale = 0f;
        });
        GlobalEventManager.Instance.AddListener("BulletHell:Restart", ()=>{
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1f;
        });
    }
    private void Awake()
    {
        // Initialize the auto-generated class
        gameInputs = new InputActions();
    }

    private void OnEnable()
    {
        // Enable the entire action map
        gameInputs.BulletHell.Enable();
        // Cache the move action for efficiency
        moveAction = gameInputs.BulletHell.Move;
        // Subscribe to the interact action
        gameInputs.BulletHell.Fire.performed += OnFire;
    }

    private void OnDisable()
    {
        // Disable the entire action map
        gameInputs.BulletHell.Disable();

        // Unsubscribe from the interact action
        gameInputs.BulletHell.Fire.performed -= OnFire;
    }

    private void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Move(moveInput);
    }

    private void Move(Vector2 movement)
    {
        if(movement == Vector2.zero) {
            // GetComponent<Animator>().SetBool("isWalk", false);
            return;
        }

        // GetComponent<Animator>().SetBool("isWalk", true);

        isLookLeft = movement.x < 0f; // flip sprites if move left
        GetComponent<SpriteRenderer>().flipX = isLookLeft;

        var translate = speed * Time.deltaTime * movement;
        // transform.Translate(translate);
        rBody.AddForce(translate * 100);
        // rBody.velocity = Vector2.zero;
        // rBody.totalForce = Vector2.zero;
        // rBody.MovePosition(rBody.position + translate);
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        // shoot
        
    }
}
