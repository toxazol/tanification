using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameScript : MonoBehaviour
{
    [SerializeField] private Sprite endSprite;
    [SerializeField] private AudioClip endBgm;
    // Start is called before the first frame update
    void Start()
    {
        GlobalEventManager.Instance.AddListener("MainGame:End", End);
    }
    void OnDisable()
    {
        GlobalEventManager.Instance.RemoveListener("MainGame:End", End);
    }

    void End()
    {
        GetComponent<AudioSource>().clip = endBgm;
        GetComponent<AudioSource>().Play();
        GetComponent<SpriteRenderer>().sprite = endSprite;
        foreach( Transform child in transform ) // disable small us
        {
            child.gameObject.SetActive(false);
        }
    }
}
