using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using RestSharp;
using Newtonsoft.Json;

public interface IResponseProvider
{
    public Task<string> GetResponse(string text_input = null);
}

internal static class StaticSuite
{
    public static readonly string LANGUAGE = "English";
    public static readonly string ANTI_LANGUAGE = "French";
    public static readonly StructuredRequest EMPTY_INIT_STRUCTURE = null;
    public static readonly List<string> EMPTY_CHECKPOINT_LIST = null;
    public static readonly string CHAT_API_URL = "https://api.together.xyz/v1/chat/completions";
    public static readonly string BEARER_STRING = "Bearer fbcc63b8b8ae920691fec7535e95d51c0d2a8296c1dc48d0789f5346a4fc5b1a";
    public static readonly string FINISH_TOKEN = "[FINISHED]";

    public static bool InitiateConversation(string input)
    {
        return input == null;
    }

    public static string CleanModelInput(string input)
    {
        Regex TMR = new(@"\s\s+");
        string local_content = input;
        local_content = TMR.Replace(local_content, " ").Replace("\"", "\\\"");
        local_content = local_content.Replace("\n", "").Replace("\t", "");

        // Debug.Log("Input String to API:");
        // Debug.Log(local_content);

        return local_content;
    }

    public static string RespToString(RestResponse response)
    {
        var responseObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content);
        var choices = responseObject["choices"] as Newtonsoft.Json.Linq.JArray;
        var message = choices[0]["message"] as Newtonsoft.Json.Linq.JObject;
        Debug.Assert(message["role"].ToString() == "assistant", "Role is not assistant");
        var content = message["content"].ToString().Trim();

        // Debug.Log("Response from API:");
        // Debug.Log(content);
        return content;
    }
}

public class DummyResponseProvider : IResponseProvider
{
    private static readonly string[] greetings = {
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
    public async Task<string> GetResponse(string input = null)
    {
        await Task.Delay(UnityEngine.Random.Range(100, 1000)); // Simulate latency
        string output = greetings[UnityEngine.Random.Range(0, greetings.Length)];
        return output;
    }
}

public class LLM_ResponseProvider : IResponseProvider
{
    private readonly LLM_API_Client _client;
    private readonly bool _client_initialized = false;

    private readonly ActorInfo _actor_info;
    public ActorInfo ActorInfo => _actor_info;

    // new client inside the constructor
    // TODO: Actor history
    public LLM_ResponseProvider(ActorInfo actor_info,
                                StructuredParameter parameters = null)
    {
        _client = new LLM_API_Client(parameters);
        _actor_info = actor_info;
        _client_initialized = true;
    }

    public static MetaModelInput CreateResponseGuidance(ActorInfo actor_info)
    {
        // Setup instruction for the model based on actor_info
        const ModelInputState init_state = ModelInputState.USER;
        string instruction_1 = $"Your name is {actor_info.FirstName} {actor_info.LastName}. {actor_info.Biography}";


        List<string> instruction_2 = actor_info.CheckpointList;
        string instruction_3 = $@"Now for some very important guidelines for the
        roleplay coming up, please follow carefully. Remember you're having an
        interactive conversation with me, so make sure you sound natural and
        realistic. Don't be too sophisticated, you should sound organic. You
        must only respond in {StaticSuite.LANGUAGE}. You are NOT allowed to respond in
        {StaticSuite.ANTI_LANGUAGE}, nor should it be part of your language at all. The end
        of each {StaticSuite.LANGUAGE} should end with this special sequence of characters:
        '{StaticSuite.FINISH_TOKEN}'. Each of your responses should be short.
        Only output your response to my statement. That is,
        you should only output your dialogue, nothing else. Do not include any
        special characters or emojis in your output. Do not output the
        {StaticSuite.ANTI_LANGUAGE} translation. Each response you provide
        should have exactly one question integrated at the end of your
        short response. Alright, let's get started!";
        StructuredRequest init_instructions = new(
            arg_sceneInstruction: instruction_1,
            arg_checkpointList: instruction_2,
            arg_constraintInstruction: instruction_3
        );
        string init_request = "Start with the first checkpoint. You begin by asking me a question.";
        // Get MetaModelInput object
        MetaModelInput init_input = new(
            arg_state: init_state,
            arg_initRequest: init_instructions,
            arg_responseString: init_request
        );

        return init_input;
    }

    public async Task<string> GetResponse(string text_input = null)
    {
        Debug.Assert(_client_initialized, "LLM API client is not initialized");
        MetaModelInput input_obj;
        if (StaticSuite.InitiateConversation(text_input))
        {
            // This means that that we're starting a conversation for the first
            // and should provide the initial guidance
            Debug.Log("[Conversation LLM] Starting a new conversation");
            input_obj = CreateResponseGuidance(_actor_info);
        }
        else
        {
            // This means that we're continuing a conversation that has already
            // started
            Debug.Log("[Conversation LLM] Continuing the conversation");
            input_obj = new MetaModelInput(ModelInputState.USER,
                                        arg_initRequest: StaticSuite.EMPTY_INIT_STRUCTURE,
                                        arg_responseString: text_input);
        }
        Debug.Log($"Sending request to LLM API with input: {input_obj.JointInstruction}");
        return await _client.SendRequest(input_obj);
    }
}

public class LLM_SuggestionProvider : IResponseProvider
{
    private readonly LLM_API_Client _client;
    private readonly bool _client_initialized = false;

    public LLM_SuggestionProvider(StructuredParameter parameters = null)
    {
        _client = new LLM_API_Client(parameters);
        _client_initialized = true;
    }

    public static MetaModelInput CreateSuggestionGuidance(string prompt_input)
    {
        // Setup instruction for the model based on actor_info
        const ModelInputState conv_state = ModelInputState.USER;
        string instruction_1 = $"Here is a prompt: \"{prompt_input}\".";
        List<string> instruction_2 = StaticSuite.EMPTY_CHECKPOINT_LIST;
        string instruction_3 = $"Please provide a suggestion for a possible response in {StaticSuite.LANGUAGE}. Please ensure that the response is natural and organic. Do not include any special characters or emojis in your output. Your suggestion should be fairly short. It should also strictly be a single sentence.";
        StructuredRequest init_instructions = new(
            arg_sceneInstruction: instruction_1,
            arg_checkpointList: instruction_2,
            arg_constraintInstruction: instruction_3
        );
        string init_request = "Please provide a suggestion now according to these constraints.";
        // Get MetaModelInput object
        MetaModelInput init_input = new(
            arg_state: conv_state,
            arg_initRequest: init_instructions,
            arg_responseString: init_request
        );

        return init_input;
    }

    public async Task<string> GetResponse(string prompt_input = null)
    {
        Debug.Assert(_client_initialized, "LLM API client is not initialized");
        Debug.Assert(prompt_input != null, "Prompt input is null. This should never happen for suggestions.");
        MetaModelInput prompt_obj = CreateSuggestionGuidance(prompt_input);
        // Debug.Log($"Sending request to LLM API with input: {prompt_obj.JointInstruction}");
        return await _client.SendRequest(prompt_obj); ;
    }
}

internal class LLM_API_Client
{
    private RestClient _client;
    private readonly StructuredParameter _parameters;

    private void SpawnClient()
    {
        _client = new RestClient(new RestClientOptions(StaticSuite.CHAT_API_URL));
    }

    public LLM_API_Client(StructuredParameter parameters = null)
    {
        // Setup parameters for the model
        _parameters = parameters ?? new StructuredParameter();
        SpawnClient();
    }

    private RestRequest ConstructRequest(string payload_content)
    {
        var request = new RestRequest("");
        request.AddHeader("accept", "application/json");
        request.AddHeader("Authorization", StaticSuite.BEARER_STRING);
        string jsonBody =
        $@"{{
            ""model"": ""{_parameters.Model}"",
            ""max_tokens"": {_parameters.MaxTokens},
            ""stop"": [""</s>"", ""[/INST]"", ""{StaticSuite.FINISH_TOKEN}""],
            ""temperature"": {_parameters.Temperature},
            ""top_p"": {_parameters.TopP},
            ""top_k"": {_parameters.TopK},
            ""repetition_penalty"": {_parameters.RepetitionPenalty},
            ""n"": 1,
            ""messages"": [
                {{
                ""role"": ""user"",
                ""content"": ""{payload_content}""
                }}
            ]
        }}";
        Debug.Log(jsonBody);

        request.AddJsonBody(jsonBody, false);
        return request;
    }

    public async Task<string> SendRequest(MetaModelInput payload)
    {
        string cleaned_payload = StaticSuite.CleanModelInput(payload.JointInstruction);
        RestRequest request = ConstructRequest(cleaned_payload);
        RestResponse delivery = await _client.PostAsync(request);
        // Print the response from the API
        Debug.Log("Response from API:");
        Debug.Log(delivery.Content);
        return await RetryLoop(prompt: StaticSuite.RespToString(delivery), MAX_TIMES: 3, cleaned_pld: cleaned_payload);
    }

    private async Task<string> RetryLoop(string prompt, int MAX_TIMES, string cleaned_pld)
    {
        int repeats = 0;
        string _output = prompt.ToLower();
        List<string> optionList = new List<string> { " Please stay in character. It is really important that you don't break the roleplay. Please don't apologize or say sorry." };
        while (_output.Contains("sorry") || _output.Contains("apologize") || _output.Contains("large language model") || _output.Contains("LLM"))
        {
            if (repeats >= MAX_TIMES)
            {
              Debug.Log("We got really unlucky");
              break;
            }
            Debug.Log("Retrying");
            repeats += 1;

            cleaned_pld += optionList[0];
            RestRequest request = ConstructRequest(cleaned_pld);
            RestResponse delivery = await _client.PostAsync(request);
            prompt = StaticSuite.RespToString(delivery);
            _output = prompt.ToLower();
        }
        return prompt;
    }
}

// TODO: Introduce yourselves
