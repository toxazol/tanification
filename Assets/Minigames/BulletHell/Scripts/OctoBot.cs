using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OctoBot : MonoBehaviour
{
    [SerializeField] private float bodyRotationSpeed = 32f;
    // [SerializeField] private float armsRotationSpeed = 32f;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject arms;
    [SerializeField] private AudioClip deathSound;
    private AudioSource sound;
    private bool isDead = false;
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        transform.Rotate(0, 0, bodyRotationSpeed * Time.fixedDeltaTime);
        // arms.transform.Rotate(0, 0, armsRotationSpeed * Time.fixedDeltaTime);
        // rBody.rotation += rotationSpeed;
    }

    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "Player") {
            Die();
        }
    }

    private void Die() {
        isDead = true;
        sound.PlayOneShot(deathSound);
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
