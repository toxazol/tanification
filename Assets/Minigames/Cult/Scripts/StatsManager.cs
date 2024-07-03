using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static ActionsManager;

public class StatsManager : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string followerCounterID;
    private Label followerCountElement;
    [SerializeField] private string faithBarID;
    private VisualElement faithBar;
    [Header("Environment")]
    [SerializeField] SpriteRenderer cultBGD;
    [SerializeField] Sprite bgd_1;
    [SerializeField] Sprite bgd_2;
    [SerializeField] Sprite bgd_3;
    [SerializeField] int followerThreshold_1;
    [SerializeField] int followerThreshold_2;
    private CultStats cultStats = new CultStats();
    public bool isVictory = false;

    // Start is called before the first frame update
    void Start()
    {
        if (MainMenu.GAME_LANGUAGE == "ua") {
            uiDocument.rootVisualElement.Q<Label>("actionMenuTitle").text =  "Дії";
            uiDocument.rootVisualElement.Q<Label>("followersLabel").text =  "Посіпаки: ";
            uiDocument.rootVisualElement.Q<Label>("faithLabel").text =  "Віра ";
        }
        
        followerCountElement = uiDocument.rootVisualElement.Q<Label>(followerCounterID);
        faithBar = uiDocument.rootVisualElement.Q<VisualElement>(faithBarID);
        cultStats.faith = 0;
        cultStats.followers = 0;
        UpdateUI();
    }

    public bool RequrementCheck(CultStats required)
    {
        return cultStats.followers >= required.followers && cultStats.faith >= required.faith;
    }

    public void UpdateStats(CultStats inc)
    {
        cultStats.faith += inc.faith;
        if (cultStats.faith < 0) cultStats.faith = 0;
        else if (cultStats.faith > 100) cultStats.faith = 100;
        int oldFollowers = cultStats.followers;
        cultStats.followers += inc.followers;
        if (cultStats.followers < 0) cultStats.followers = 0;
        UpdateUI();
        if (oldFollowers < followerThreshold_1 && cultStats.followers >= followerThreshold_1) {
            cultBGD.sprite = cultStats.followers >= followerThreshold_2 ? bgd_3 : bgd_2;
        } else if (oldFollowers < followerThreshold_2 && cultStats.followers >= followerThreshold_2)
        {
            cultBGD.sprite = bgd_3;
        } else if (oldFollowers >= followerThreshold_2 && cultStats.followers <= followerThreshold_2)
        {
            cultBGD.sprite = cultStats.followers < followerThreshold_1 ? bgd_1 : bgd_2;
        } else if (oldFollowers >= followerThreshold_1 && cultStats.followers < followerThreshold_1)
        {
            cultBGD.sprite = bgd_1;
        }
        if (cultStats.followers == 0 || cultStats.faith == 0) {
            GlobalEventManager.Instance.TriggerEvent("Cult:Defeat");
        }
        if (!string.IsNullOrEmpty(inc.globalEvent)) {
            GlobalEventManager.Instance.TriggerEvent(inc.globalEvent);
            if(inc.globalEvent == "Cult:Victory") {
                isVictory = true;
            }
        }
    }

    private void UpdateUI()
    {
        followerCountElement.text = "" + cultStats.followers;
        faithBar.style.width = new Length(Math.Max(1, cultStats.faith), LengthUnit.Percent);
    }
}
