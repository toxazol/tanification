using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamesManager : MonoBehaviour
{
    [SerializeField] UnityEditor.SceneAsset mainScene;

    [Header("Minigame Scenes")]
    [SerializeField] UnityEditor.SceneAsset hermitCrabs;
    [SerializeField] UnityEditor.SceneAsset bulletHell;
    [SerializeField] UnityEditor.SceneAsset cult;

    [Header("Events")]
    [SerializeField] StringEventChannelSO gameStart;
    [SerializeField] StringEventChannelSO gameEnd;

    public static MinigamesManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameStart.OnEventRaised += startMinigame;
        gameEnd.OnEventRaised += unloadMinigame;
    }

    // Update is called once per frame
    void OnDestroy()
    {
        gameStart.OnEventRaised -= startMinigame;
        gameEnd.OnEventRaised -= unloadMinigame;
    }

    void startMinigame(string game)
    {
        switch (game)
        {
            case "hermitCrabs":
                SceneManager.LoadSceneAsync(hermitCrabs.name);
                break;
            case "bulletHell":
                SceneManager.LoadSceneAsync(bulletHell.name);
                break;
            case "cult":
                SceneManager.LoadSceneAsync(cult.name);
                break;
            default: break;
        }
    }

    void unloadMinigame(string game)
    {
        SceneManager.LoadSceneAsync(mainScene.name);
    }
}
