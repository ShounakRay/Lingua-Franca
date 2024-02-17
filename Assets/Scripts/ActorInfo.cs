using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "ScriptableObjects/ActorInfo", order = 1)]
public class ActorInfo : ScriptableObject
{
    [SerializeField] private string firstName;
    [SerializeField] private string lastName;
    [SerializeField] private string biography;

    public string FirstName => firstName;
    public string LastName => lastName;
    public string Biography => biography;
}