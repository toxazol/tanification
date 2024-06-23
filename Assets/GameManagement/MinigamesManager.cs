using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamesManager : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] StringEventChannelSO gameStart;
    [SerializeField] StringEventChannelSO gameEnd;

    // Singleton
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

    void startMinigame(string gameName)
    {
        SceneManager.LoadSceneAsync(gameName);
    }

    void unloadMinigame(string game)
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }
}
