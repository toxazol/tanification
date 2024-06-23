using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class Typewriter : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] string dialogueTextElementID;
    [SerializeField] float typingDelay;
    [SerializeField] DialogueSoundController soundController;
    private Label dialogueTextElement;
    private StringBuilder dialogueTextBuilder = new StringBuilder();
    private WaitForSecondsRealtime delayTimer;
    public bool typing = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogueTextElement = uiDocument.rootVisualElement.Q<Label>(dialogueTextElementID);
        delayTimer = new WaitForSecondsRealtime(typingDelay);
        dialogueTextBuilder = new StringBuilder();
    }
    public void StartTyping(string text)
    {
        dialogueTextBuilder.Clear();
        dialogueTextBuilder.Capacity = text.Length;
        StartCoroutine(Type(text));
    }

    IEnumerator Type(string text)
    {
        typing = true;
        foreach (char c in text)
        {
            if (!typing)
            {
                dialogueTextBuilder.Clear();
                dialogueTextBuilder.Append(text);
                soundController.PlaySound();
                dialogueTextElement.text = dialogueTextBuilder.ToString();
                soundController.PlaySound();
                soundController.PlaySound();
                yield break;
            }
            dialogueTextBuilder.Append(c);
            if (char.IsLetter(c)) soundController.PlaySound();
            dialogueTextElement.text = dialogueTextBuilder.ToString();
            yield return delayTimer;
        }
        typing = false;
        yield return null;
    }
    
    public void ShowText()
    {
        typing = false;
    }
}
