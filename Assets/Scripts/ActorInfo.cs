using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "ScriptableObjects/ActorInfo", order = 1)]
public class ActorInfo : ScriptableObject
{
    public struct Conversation
    {
        private Exchange[] exchanges;
        public IReadOnlyList<Exchange> Exchanges => exchanges;

        public Conversation(IEnumerable<Exchange> exchanges)
        {
            this.exchanges = exchanges.ToArray();
        }
    }

    public struct Exchange
    {
        /// <summary>
        /// This actor said this to the player character.
        /// </summary>
        public string Prompt { get; }

        /// <summary>
        /// The player character said this to the actor.
        /// </summary>
        public string Response { get; }

        public Exchange(string prompt, string response)
        {
            Prompt = prompt;
            Response = response;
        }
    }

    [SerializeField] private string firstName;
    [SerializeField] private string lastName;
    [SerializeField] private string biography;

    public string FirstName => firstName;
    public string LastName => lastName;
    public string Biography => biography;

    private List<Conversation> conversations = new List<Conversation>();
    public IReadOnlyList<Conversation> Conversations => conversations;

    public void AppendConversation(Conversation conversation)
    {
        conversations.Add(conversation);
    }
}