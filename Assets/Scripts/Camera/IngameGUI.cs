using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameGUI : MonoBehaviour
{
    public GameObject DetailedStatsWindow;

    private CharacterStats displayedStats;

    public GameObject DialogWindow;

    private TurnController controller;

    private Camera mainCamera;

    private EventController events;

    // Start is called before the first frame update
    void Start()
    {
        if (DetailedStatsWindow == null) DetailedStatsWindow = transform.Find("Detailed Stats Window").gameObject;
        DetailedStatsWindow.SetActive(false);

        if (DialogWindow == null) DialogWindow = transform.Find("Dialog Window").gameObject;
        DialogWindow.SetActive(false);

        controller = TurnController.GetInstance();

        events = EventController.GetInstance();
        events.CharacterSelected += ShowDetailedCharacter;
        events.NotifyStatsChaned += UpdateDetailesWindow;

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Cancel") > 0)
        {
            EndTurnAttempt();
        }
    }

    

    public void HideDetailedStats()
    {
        Debug.Log("hide");
        DetailedStatsWindow.SetActive(false);
    }

    public void ShowDetailedCharacter(CharacterStats characterStats)
    {
        DetailedStatsWindow.SetActive(true);

        displayedStats = characterStats;
        DetailedStatsWindow.transform.Find("Character display").gameObject.GetComponent<Image>().sprite = characterStats.fullDisplay;

        DetailedStatsWindow.GetComponentInChildren<Text>().text = "Stats:\n - Health: " + characterStats.Health + "/" + characterStats.maxHealth + "\n - Mana: " + characterStats.Mana + "/" + characterStats.maxMana + "\n - Movement: " + characterStats.MovementCount + "/" + characterStats.maxMovementCount; 
    }

    public void UpdateDetailesWindow(CharacterStats subject)
    {
        if(displayedStats != null) DetailedStatsWindow.GetComponentInChildren<Text>().text = "Stats:\n - Health: " + displayedStats.Health + "/" + displayedStats.maxHealth + "\n - Mana: " + displayedStats.Mana + "/" + displayedStats.maxMana + "\n - Movement: " + displayedStats.MovementCount + "/" + displayedStats.maxMovementCount;
    }

    public void EndTurnAttempt()
    {
        DialogWindow.SetActive(true);
    }

    public void EndTurn()
    {
        DialogWindow.SetActive(false);
        controller.CurrentCharacterBehavior.ForceEndTurn();
    }

    public void CancelDialog()
    {
        DialogWindow.SetActive(false);
    }
}
