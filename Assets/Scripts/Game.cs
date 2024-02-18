using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static Game instance;
    public static Game Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<Game>();
            if (instance == null) instance = new GameObject("[Game]").AddComponent<Game>();
            DontDestroyOnLoad(instance.gameObject);
            return instance;
        }
    }

    private Player player;
    public Player Player
    {
        get
        {
            if (player != null) return player;
            player = FindObjectOfType<Player>();
            return player;
        }
    }

    public ObjectiveManager ObjectiveManager;
    public Language targetLanguage = Language.English; // English by default. changes when game starts

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
            return;
        }

        if (Player == null) Debug.LogWarning("A player could not be found in the scene. Did you remember to add one?");
        
        ObjectiveManager = FindObjectOfType<ObjectiveManager>();
    }
}


public enum Language
{
    English,
    Spanish,
    French
}
    