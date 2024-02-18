using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;


// TODO: `dotnet add package RestSharp`
using RestSharp;
using Newtonsoft.Json;

public interface IResponseProvider
{
    // FIXME: `input` should be different type
    public Task<string> GetResponse(MetaModelInput input = null);
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

    // NOTE: UNUSED: string input = null
    public async Task<string> GetResponse(MetaModelInput input = null)
    {
        await Task.Delay(UnityEngine.Random.Range(100, 1000)); // Simulate latency
        return greetings[UnityEngine.Random.Range(0, greetings.Length)];
    }
}


public class LLM_ResponseProvider : IResponseProvider
{
    private readonly LLM_API_Client _client;
    private readonly bool _client_initialized = false;

    public string response = "";

    // new client inside the constructor
    // TODO: Actor history
    public LLM_ResponseProvider(StructuredParameter parameters)
    {
        _client = new LLM_API_Client(parameters);
        _client_initialized = true;
    }

    // FIXME: `input` should be different type
    public async Task<string> GetResponse(MetaModelInput input = null)
    {
        Debug.Assert(_client_initialized, "LLM API client is not initialized");
        Debug.Assert(input != null, "Input cannot be null");
        return await _client.SendRequest(input);
    }
}

internal class LLM_API_Client
{
    private RestClient _client;
    private const string API_URL = "https://api.together.xyz/v1/chat/completions";

    private readonly string _model;
    private readonly int _max_tokens;
    private readonly double _temperature;
    private readonly double _top_p = 0.7;
    private readonly int _top_k = 50;
    private readonly double _repetition_penalty = 1;


    private void SpawnClient()
    {
        _client = new RestClient(new RestClientOptions(API_URL));
    }

    public LLM_API_Client(StructuredParameter parameters)
    {
        Debug.Assert(parameters != null, "Parameters cannot be null");
        _model = parameters.Model;
        _max_tokens = parameters.MaxTokens;
        _temperature = parameters.Temperature;
        SpawnClient();
    }

    public async Task<string> SendRequest(MetaModelInput payload)
    {
        var request = new RestRequest("");
        request.AddHeader("accept", "application/json");
        // FIXME: Need to change bearer?
        request.AddHeader("Authorization", "Bearer fbcc63b8b8ae920691fec7535e95d51c0d2a8296c1dc48d0789f5346a4fc5b1a");
        var jsonBody = new
        {
            model = _model,
            max_tokens = _max_tokens,
            stop = new[] { "</s>", "[/INST]" },
            temperature = _temperature,
            top_p = _top_p,
            top_k = _top_k,
            repetition_penalty = _repetition_penalty,
            n = 1,
            messages = new[]
            {
                new { role = payload.Role, content = payload.JointInstruction }
            }
        };

        request.AddJsonBody(JsonConvert.SerializeObject(jsonBody), false);

        var response = await _client.PostAsync(request);
        // TODO: Data sanitation on response type / error handling

        // NOTE: `response` is in a dictionary format (HTTP output)
        return response.Content;
    }

}