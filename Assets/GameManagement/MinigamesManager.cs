using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamesManager : MonoBehaviour
{
    [Header("Minigame Scenes")]
    [SerializeField] UnityEditor.SceneAsset hermitCrabs;
    [SerializeField] UnityEditor.SceneAsset bulletHell;
    [SerializeField] UnityEditor.SceneAsset cult;

    [Header("Events")]
    [SerializeField] StringEventChannelSO gameStart;
    [SerializeField] StringEventChannelSO gameEnd;

    // Start is called before the first frame update
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
                SceneManager.LoadScene(hermitCrabs.name);
                break;
            case "bulletHell":
                SceneManager.LoadScene(bulletHell.name);
                break;
            case "cult":
                SceneManager.LoadScene(cult.name);
                break;
            default: break;
        }
    }

    void unloadMinigame(string game)
    {
        switch (game)
        {
            case "hermitCrabs":
                SceneManager.UnloadSceneAsync(hermitCrabs.name);
                break;
            case "bulletHell":
                SceneManager.UnloadSceneAsync(bulletHell.name);
                break;
            case "cult":
                SceneManager.UnloadSceneAsync(cult.name);
                break;
            default: break;
        }
    }
}
