using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Typewriter typewriter;
    [SerializeField] private List<DialogueEntry> dialogue;
    [SerializeField] private int currLine;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        currLine = 0;
    }

    void OnNext(InputValue value)
    {
        if (!value.isPressed) return;
        if (this.typewriter.typing) {
            this.typewriter.ShowText();
        } else {
            this.typewriter.StartTyping(dialogue[currLine].phrase);
            currLine++;
        }
    }

    [System.Serializable]
    public class DialogueEntry 
    {
        
        public string phrase;
        public bool excited;
    }
}
