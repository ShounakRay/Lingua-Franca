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
        // ClickFrench(true);
        // ClickSpanish(false);
        // ClickStart();
    }

    private void ClickSpanish(bool toggled)
    {
        if (toggled)
        {
            Game.Instance.targetLanguage = Language.Spanish;
            Debug.Log("Spanish toggled");
        }
    }

    private void ClickFrench(bool toggled)
    {
        if (toggled)
        {
            Game.Instance.targetLanguage = Language.French;
            Debug.Log("French toggled");
        }
    }

    // Update is called once per frame
    void Update()
    {   
    }

    private void ClickStart()
    {
        if (Game.Instance.targetLanguage == Language.English)
        {
            selectionWarning.SetActive(true);
            Debug.Log("English selected");
        }
        else
        {
            Debug.Log("Starting game with language: " + Game.Instance.targetLanguage);
            gameObject.SetActive(false);
            if (Game.Instance.Player)
                Game.Instance.Player.SetMovementState(true);
            Game.Instance.ObjectiveManager.AddObjective("eat", "eat a sandwhich");
            Game.Instance.ObjectiveManager.AddObjective("drink", "drink a glass of water");
        }
    }
}
