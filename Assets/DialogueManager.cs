using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Typewriter typewriter;
    [SerializeField] private List<DialogueEntry> dialogue;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        yield return new WaitForSeconds(1);
        foreach (DialogueEntry e in dialogue)
        {
            typewriter.StartTyping(e.phrase);
            yield return new WaitUntil(() => { return typewriter.typing == false; });
        }
        yield return null;
    }

    [System.Serializable]
    public class DialogueEntry 
    {
        
        public string phrase;
        public Boolean excited;
    }
}
