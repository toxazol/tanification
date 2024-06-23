using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string dialogueBoxId;
    private VisualElement dialogueBox;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Typewriter typewriter;
    [SerializeField] private SpeakerRenderer speakerRenderer;
    [SerializeField] private DialogueSoundController dialogueSoundController;
    [SerializeField] private StringEventChannelSO startGameEvent;
    [SerializeField] private StringEventChannelSO endGameEvent;
    [SerializeField] private List<DialogueEntry> dialogue;
    [SerializeField] private int currLine;
    private bool end;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        endGameEvent.OnEventRaised += continueDialogue;
        end = false;
        currLine = 0;
        dialogueBox = uiDocument.rootVisualElement.Q<VisualElement>(dialogueBoxId);
    }

    void OnNext(InputValue value)
    {
        if (!value.isPressed || end) return;
        if (this.typewriter.typing) {
            this.typewriter.ShowText();
        } else {
            if (!dialogueBox.visible)
            {
                dialogueBox.visible = true;
            }
            if (!string.IsNullOrEmpty(dialogue[currLine].startGame)) startGameEvent.RaiseEvent(dialogue[currLine].startGame);
            this.dialogueSoundController.PitchShift(dialogue[currLine].character.characterPitch);
            this.typewriter.StartTyping(dialogue[currLine].phrase);
            this.speakerRenderer.DisplaySpeaker(dialogue[currLine].character, dialogue[currLine].emotion);
            currLine++;
            end = currLine >= dialogue.Count;
        }
    }

    void continueDialogue(string text)
    {
        return;
    }

    [System.Serializable]
    public class DialogueEntry 
    {
        public string phrase;
        public CharacterSO character;
        public string emotion;
        public string startGame;
    }
}
