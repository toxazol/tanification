using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class MainMenu : MonoBehaviour
{
    public static string GAME_LANGUAGE = "en";
    public static bool IS_ACTIVE = true;

    // Start is called before the first frame update
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;

        RadioButton radioButtonUa = root.Q<RadioButton>("uaRadio"); // Assuming the name of the radio button is "radioButton1"
        RadioButton radioButtonEn = root.Q<RadioButton>("enRadio");

        // Add event listeners
        radioButtonUa.RegisterValueChangedCallback(evt => OnRadioButtonChanged(evt.newValue, "ua"));
        radioButtonEn.RegisterValueChangedCallback(evt => OnRadioButtonChanged(evt.newValue, "en"));

        Button playButton = root.Q<Button>("playBtn");
        playButton.clicked += OnPlayButtonClicked;
    }

    private void OnRadioButtonChanged(bool isSelected, string selectedValue)
    {
        if (isSelected)
        {
            GAME_LANGUAGE = selectedValue;
            Debug.Log("Selected value: " + selectedValue);
        }
    }

    private void OnPlayButtonClicked()
    {
        // Handle button click
        Debug.Log("Play button clicked!");

        IS_ACTIVE = false;

        gameObject.SetActive(false);
    }

}
