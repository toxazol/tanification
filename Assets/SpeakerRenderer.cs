using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpeakerRenderer : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string spriteElementID;
    [SerializeField] private string nameElementID;
    private VisualElement spriteElement;
    private Label nameElement;

    // Start is called before the first frame update
    void Start()
    {
        spriteElement = uiDocument.rootVisualElement.Q<VisualElement>(spriteElementID);
        nameElement = uiDocument.rootVisualElement.Q<Label>(nameElementID);
    }

    public void DisplaySpeaker(CharacterSO character, string emotion)
    {
        spriteElement.style.backgroundImage = character.GetSprite(emotion);
        nameElement.text = character.characterName;
        nameElement.style.color = character.characterColor;
    }
}
