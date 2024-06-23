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
    [SerializeField] private List<EventDialogueListEntry> dialogue;
    [SerializeField] private int currLine;
    [SerializeField] private List<DialogueEntry> currDialog;
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
        currDialog = dialogue[0].dialogueList; // start with firtst dialogue list (no event needed)

        // change current dialog based on dialog events
        dialogue.ForEach(eventDialog => {
            GlobalEventManager.Instance.AddListener(eventDialog.globalEventName, () => {
                currDialog = eventDialog.dialogueList;
                currLine = 0;
                end = false;
                OnNext(null);
            });
        });
    }
    void OnNext(InputValue value)
    {
        if (end) {
            dialogueBox.visible = false;
            return;
        } 
        if (value != null && !value.isPressed) return;
        if (this.typewriter.typing) {
            this.typewriter.ShowText();
        } else {
            if (!dialogueBox.visible)
            {
                dialogueBox.visible = true;
            }
            if (!string.IsNullOrEmpty(currDialog[currLine].startGame)) startGameEvent.RaiseEvent(currDialog[currLine].startGame);
            this.dialogueSoundController.PitchShift(currDialog[currLine].character.characterPitch);
            this.typewriter.StartTyping(currDialog[currLine].phrase);
            this.speakerRenderer.DisplaySpeaker(currDialog[currLine].character, currDialog[currLine].emotion);
            currLine++;
            end = currLine >= currDialog.Count;
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

    [System.Serializable]
    public class EventDialogueListEntry 
    {
        public String globalEventName;
        public List<DialogueEntry> dialogueList;
    }
}
