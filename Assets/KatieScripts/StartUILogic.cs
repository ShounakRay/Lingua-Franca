using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUILogic : MonoBehaviour
{
    
    public GameObject spanishToggle;
    public GameObject frenchToggle;
    public GameObject startButton;
    public GameObject selectionWarning;


    // Start is called before the first frame update
    void Start()
    {
        if (Game.Instance.Player)
            Game.Instance.Player.SetMovementState(false);
        spanishToggle.GetComponent<Toggle>().onValueChanged.AddListener(ClickSpanish);
        frenchToggle.GetComponent<Toggle>().onValueChanged.AddListener(ClickFrench);
        startButton.GetComponent<Button>().onClick.AddListener(ClickStart);
    }

    private void ClickSpanish(bool toggled)
    {
        if (toggled)
            Game.Instance.targetLanguage = Language.Spanish;
    }

    private void ClickFrench(bool toggled)
    {
        if (toggled)
            Game.Instance.targetLanguage = Language.French;
    }

    // Update is called once per frame
    void Update()
    {   
    }

    private void ClickStart()
    {
        if (Game.Instance.targetLanguage == Language.English)
            selectionWarning.SetActive(true);
        else
        {
            Debug.Log("Starting game with language: " + Game.Instance.targetLanguage);
            gameObject.SetActive(false);
            if (Game.Instance.Player)
                Game.Instance.Player.SetMovementState(true);

            Game.Instance.ObjectiveManager.AddObjective("eat", "eat a sandwhich");
            Game.Instance.ObjectiveManager.AddObjective("drink", "drink a glass of water");
            Game.Instance.ObjectiveManager.AddObjective("sleep", "sleep for 8 hours");
            Game.Instance.ObjectiveManager.AddObjective("exercise", "exercise for 30 minutes");
            /* Either we can call a funciton here to start the game, or we can put all of the logic here.
            This includes things like spawning the objectives */
        }
    }
}
