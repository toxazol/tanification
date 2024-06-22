using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OctoBot : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;
    private bool isDead = false;
    // private Rigidbody2D rBody;
    void Start()
    {
        // rBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        transform.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime);
        // rBody.rotation += rotationSpeed;
    }

    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "Player") {
            Die();
        }
    }

    private void Die() {
        isDead = true;
        transform.GetComponentsInChildren<Transform>()
            .Where(t => t.name == "working")
            .ToList()
            .ForEach(t => t.gameObject.SetActive(false));
        transform.GetComponentsInChildren<Transform>(true)
            .Where(t => t.name == "broken")
            .ToList()
            .ForEach(t => t.gameObject.SetActive(true));
        transform.GetComponentsInChildren<Transform>()
            .Where(t => t.name == "Cannon")
            .ToList()
            .ForEach(t => t.gameObject.SetActive(false));
        transform.GetComponentsInChildren<Joint2D>()
            .ToList()
            .ForEach(t => t.enabled = false);
    }
}
