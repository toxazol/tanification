using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamesManager : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] StringEventChannelSO gameStart;
    [SerializeField] StringEventChannelSO gameEnd;
    [SerializeField] MainMenu mainMenu;

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
        mainMenu.gameObject.SetActive(true);
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
