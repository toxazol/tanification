using UnityEngine;
using System.Collections.Generic;
using System;

public class GlobalEventManager : MonoBehaviour
{
    private static GlobalEventManager _instance;
    public static GlobalEventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GlobalEventManager");
                _instance = go.AddComponent<GlobalEventManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
    // Singleton
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
    }

    private Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();

    public void AddListener(string eventName, Action listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = listener;
        }
        else
        {
            eventDictionary[eventName] += listener;
        }
    }

    public void RemoveListener(string eventName, Action listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= listener;
        }
    }

    public void TriggerEvent(string eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName]?.Invoke();
        }
    }
}