using System.Collections.Generic;
using System.Threading.Tasks;

using RestSharp;

public interface IResponseProvider
{
    // FIXME: `input` should be different type
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
public class LLM_ResponseProvider : IResponseProvider
{
    private readonly LLM_API_Client _client;
    private readonly bool _client_initialized = false;

    public string response = "";

    // new client inside the constructor
    // TODO: Actor history
    public LLM_ResponseProvider()
    {
        _client = new LLM_API_Client();
        _client_initialized = true;
    }

    // FIXME: `input` should be different type
    public Task<string> GetResponse(string input = null)
    {
        return response = _client.SendRequest(input);
    }
}

internal class StructuredRequest
{
    private readonly string _sceneInstruction;
    private readonly List<string> _checkpointList;
    private readonly string _constraintInstruction;
    public string jointInstruction
    {
        get
        {
            return sceneFormat() + "\n" + checkpointFormat() + "\n" + constraintFormat();
        }
    }

    private string sceneFormat()
    {
        string _local = "You are in a scene where: ";
        _local += _sceneInstruction;
        return _local;
    }

    private string checkpointFormat()
    {
        string _local = "Your objective is to fulfill these checkpoints: ";
        for (int ckpt_num = 0; ckpt_num < checkpointList.Count; ckpt_num++)
        {
            _local += ((ckpt_num == 0) ? "" : " + ") + checkpointList[ckpt_num];
        }
        return _local;
    }

    private string constraintFormat()
    {
        string _local = "You must also adhere to the following constraints: ";
        _local += constraintInstruction;
        return _local;
    }


    public StructuredRequest(string arg_sceneInstruction,
                             List<string> arg_checkpointList,
                             string arg_constraintInstruction)
    {
        _sceneInstruction = arg_sceneInstruction;
        _checkpointList = arg_checkpointList;
        _constraintInstruction = arg_constraintInstruction;
    }
}

internal class LLM_API_Client
{
    private RestClient _client;

    private readonly string _model;
    private readonly int _max_tokens;
    private readonly double _temperature;
    private readonly double _top_p = 0.7;
    private readonly int _top_k = 50;
    private readonly double _repetition_penalty = 1;

    public LLM_API_Client(string arg_model = "mistralai/Mixtral-8x7B-Instruct-v0.1",
                        int arg_max_tokens = 512, double arg_temperature = 0.7,
                        double arg_top_p = 0.7, int arg_top_k = 50,
                        double arg_repetition_penalty = 1)
    {
        _model = arg_model;
        _max_tokens = arg_max_tokens;
        _temperature = arg_temperature;
        _top_p = arg_top_p;
        _top_k = arg_top_k;
        _repetition_penalty = arg_repetition_penalty;
        init_client(null, null);
    }

    public async Task<string> SendRequest(string input)
    {
        // FIXME: `input` should be different type
        var payload = new StructuredRequest(RequestMode.USER_MODE, input, null, null);

        var request = new RestRequest("");
        request.AddHeader("accept", "application/json");
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
                new { role = payload.role, content = payload.jointInstruction }
            }
        };

        request.AddJsonBody(jsonBody, false);
        var response = await client.PostAsync(request);
    }

    // Initialize the API client with StructuredRequest and StructuredParameter
    private void init_client(StructuredRequest request,
                             StructuredParameter parameter)
    {
        _client = new RestClient(new RestClientOptions(API_URL));
    }
}

internal class StructuredParameter
{
    private const string API_URL = "https://api.together.xyz/v1/chat/completions";

    public string Model { get; set; }
    public int MaxTokens { get; set; }
    public List<string> Stop { get; set; }
    public double Temperature { get; set; }
    public double TopP { get; set; }
    public int TopK { get; set; }
    public double RepetitionPenalty { get; set; }
    public int N { get; set; }

    public StructuredParameter(double arg_temperature,
                               double arg_topP,
                               int arg_topK,
                               double arg_repetitionPenalty,
                               int arg_n,
                               string arg_model = API_URL,
                               int arg_maxTokens = 512,
                               List<string> arg_stop = null)
    {
        Model = arg_model;
        MaxTokens = arg_maxTokens;
        Stop = arg_stop ?? new List<string> { "</s>", "[/INST]" };
        Temperature = arg_temperature;
        TopP = arg_topP;
        TopK = arg_topK;
        RepetitionPenalty = arg_repetitionPenalty;
        N = arg_n;
    }
}

internal class Message
{
    public string Role { get; set; }
    public string Content { get; set; }
}

// using RestSharp;


// var options = new RestClientOptions("https://api.together.xyz/v1/chat/completions");
// var client = new RestClient(options);
// // var request = new RestRequest("");
// // request.AddHeader("accept", "application/json");
// // request.AddHeader("Authorization", "Bearer fbcc63b8b8ae920691fec7535e95d51c0d2a8296c1dc48d0789f5346a4fc5b1a");
// request.AddJsonBody("{\"model\":\"mistralai/Mixtral-8x7B-Instruct-v0.1\",\"max_tokens\":512,\"stop\":[\"</s>\",\"[/INST]\"],\"temperature\":0.7,\"top_p\":0.7,\"top_k\":50,\"repetition_penalty\":1,\"n\":1,\"messages\":[{\"role\":\"user\",\"content\":\"Example string\"}]}", false);
// var response = await client.PostAsync(request);

// Console.WriteLine("{0}", response.Content);