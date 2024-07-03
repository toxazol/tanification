using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ActionsManager : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string buttonsContainerID;
    private VisualElement buttonsContainer;
    [SerializeField] private StatsManager statsManager;
    [SerializeField] private List<CultAction> actions;
    [SerializeField] private List<int> usedButtons = new List<int>();
    [SerializeField] private AudioClip clickSound;
    private int btnClickCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        buttonsContainer = uiDocument.rootVisualElement.Q<VisualElement>(buttonsContainerID);
        usedButtons.Clear();
        AddButton(actions[0], 0);
        AddButton(actions[1], 1);
        AddButton(actions[4], 4);
    }

    private void AddButton(CultAction action, int index)
    {
        Debug.Log("Add button: " + index);
        Button actionButton = new Button();
        actionButton.name = "action_" + index;
        actionButton.AddToClassList("actionButton");
        actionButton.text = MainMenu.GAME_LANGUAGE == "en" ? action.name : action.name_ua;
        buttonsContainer.Add(actionButton);
        actionButton.clickable.clickedWithEventInfo += ActionButtonClick;
        usedButtons.Add(index);
    }

    private void ActionButtonClick(EventBase eventBase)
    {
        // Allow only mouse clicks to avoid conflicts with main dialog skip inputs
        if (!(eventBase is MouseUpEvent || eventBase is PointerUpEvent))
        {
            return;
        }
        GetComponent<AudioSource>().PlayOneShot(clickSound);
        Button btn = (Button)eventBase.target;
        int index = int.Parse(btn.name.Substring(btn.name.IndexOf("_") + 1));
        CultAction action = actions[index];
        CultStats inc = statsManager.RequrementCheck(action.requred) ? action.goodOutcome : action.badOutcome;
        statsManager.UpdateStats(inc);
        buttonsContainer.Remove(btn);
        btnClickCount++;
        if (usedButtons.Count < actions.Count) {
            AddRandomButton();
        } 
        if (btnClickCount >= actions.Count && !statsManager.isVictory){
            GlobalEventManager.Instance.TriggerEvent("Cult:Defeat");
        }
    }

    private void AddRandomButton()
    {
        int newBtn = UnityEngine.Random.Range(0, actions.Count);
        while (usedButtons.Contains(newBtn)) {
            newBtn = UnityEngine.Random.Range(0, actions.Count);
        }
        AddButton(actions[newBtn], newBtn);
    }

    [Serializable]
    public class CultAction {
        public string name;
        public string name_ua;
        public CultStats requred;
        public CultStats goodOutcome;
        public CultStats badOutcome;
    }

    [Serializable]
    public class CultStats
    {
        public int followers;
        public int faith;
        public string globalEvent;
    }
}
