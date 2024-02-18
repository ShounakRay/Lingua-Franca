using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class MetaConversation
{
    // USer mode or system mode
    // If user, then "what the person said through VR"
    // If system, then "what the system instruction ()"
}

[CreateAssetMenu(fileName = "Actor", menuName = "ScriptableObjects/ActorInfo", order = 1)]
public class ActorInfo : ScriptableObject
{
    [SerializeField] private string firstName;
    [SerializeField] private string lastName;
    [SerializeField] private string biography;
    [SerializeField] private List<string> checkpointList;

    public string FirstName => firstName;
    public string LastName => lastName;
    public string Biography => biography;
    public List<string> CheckpointList => checkpointList;
}