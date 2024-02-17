using System.Threading.Tasks;
using UnityEngine;

public interface IResponseProvider
{
    public Task<string> GetResponse(string input = null);
}

public class DummyResponseProvider : IResponseProvider
{
    private static string[] greetings = {
        "Hey there!",
        "Good morning!",
        "Hi, how are you?",
        "Hello, friend!",
        "What's up?",
        "Hey, how's it going?",
        "Morning!",
        "Hiya!",
        "Yo!",
        "Hi, stranger!",
        "Howdy!",
        "Hey, good to see you!",
        "Greetings!",
        "Hey, what's happening?",
        "Well, hello there!",
        "Hey, long time no see!",
        "Hey, buddy!",
        "Hi, nice to meet you!",
        "Hello, how have you been?",
        "Hey, how's your day?",
        "Good day!",
        "Hi, how's everything?",
        "Hello, stranger!",
        "Hey, how've you been?",
        "Hi, how's life treating you?"
    };

    public async Task<string> GetResponse(string input = null)
    {
        await Task.Delay(Random.Range(100, 1000)); // Simulate latency
        return greetings[Random.Range(0, greetings.Length)];
    }
}