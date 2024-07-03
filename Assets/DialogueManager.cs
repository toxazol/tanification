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

    // Singleton
    public static DialogueManager Instance { get; private set; }
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
        // endGameEvent.OnEventRaised += continueDialogue;
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
    public void OnNext(InputValue value)
    {
        if (MainMenu.IS_ACTIVE) return;

        if (this.typewriter.typing) {
            this.typewriter.ShowText();
            return;
        }
        if (end && dialogueBox.visible) { // for now let's allow firing events from dialogs and ending games only on dialog ends
            dialogueBox.visible = false;
            
            if (currDialog[currLine-1].endsGame) {
                endGameEvent.RaiseEvent("aaaaaaaaaaaaaaaaaaaaaaaaaaa");
            }
            if (!string.IsNullOrEmpty(currDialog[currLine-1].globalEvent)) {
                GlobalEventManager.Instance.TriggerEvent(currDialog[currLine-1].globalEvent);
            }
            return;
        }
        if (currLine >= currDialog.Count) return;
        if (value != null && !value.isPressed) return;
   
        if (!dialogueBox.visible)
        {
            dialogueBox.visible = true;
        }
        
        if (!string.IsNullOrEmpty(currDialog[currLine].startGame)) {
            startGameEvent.RaiseEvent(currDialog[currLine].startGame);
        }

        if(currDialog[currLine].character != null) {
            this.dialogueSoundController.PitchShift(currDialog[currLine].character.characterPitch);
            this.speakerRenderer.DisplaySpeaker(currDialog[currLine].character, currDialog[currLine].emotion);
        }
        
        this.typewriter.StartTyping(MainMenu.GAME_LANGUAGE == "en" ? currDialog[currLine].phrase : currDialog[currLine].phrase_ua);
        
        currLine++;
        end = currLine >= currDialog.Count;
    }

    void continueDialogue(string text)
    {
        return;
    }

    [System.Serializable]
    public class DialogueEntry 
    {
        public string phrase;
        public string phrase_ua;
        public CharacterSO character;
        public string emotion;
        public string startGame;
        public bool endsGame;
        public string globalEvent;
    }

    [System.Serializable]
    public class EventDialogueListEntry 
    {
        public String globalEventName;
        public List<DialogueEntry> dialogueList;
    }
}
