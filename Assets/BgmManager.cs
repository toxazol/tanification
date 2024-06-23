using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioClip bulletHellDeath;
    [SerializeField] AudioClip bulletHellWin;

    void Start()
    {
        GlobalEventManager.Instance.AddListener("BulletHell:Death", bhDeath);
        GlobalEventManager.Instance.AddListener("BulletHell:Win", bhWin);
    }

    void bhDeath() {
        GetComponent<AudioSource>().clip = bulletHellDeath;
        GetComponent<AudioSource>().Play();
    }
    void bhWin() {
        GetComponent<AudioSource>().clip = bulletHellWin;
        GetComponent<AudioSource>().Play();
    }

    void OnDisable() 
    {
        GlobalEventManager.Instance.RemoveListener("BulletHell:Death", bhDeath);
        GlobalEventManager.Instance.RemoveListener("BulletHell:Win", bhWin);
    }

}
