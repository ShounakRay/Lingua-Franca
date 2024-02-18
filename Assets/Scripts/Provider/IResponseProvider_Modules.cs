using System.Collections.Generic;
using UnityEngine;

// This is the the nature of text the LLM expects to receive
public enum ModelInputState
{
    SYSTEM,
    USER
}

public class MetaModelInput
{
    public ModelInputState State { get; private set; }
    private readonly StructuredRequest _initRequest;
    private readonly string _responseString;

    public MetaModelInput(ModelInputState arg_state,
                          StructuredRequest arg_initRequest,
                          string arg_responseString)
    {
        State = arg_state;

        Debug.Assert(arg_responseString != null, "responseString is null. Must not be.");
        if (arg_initRequest != null)
        {
            Debug.Assert(arg_state == ModelInputState.USER, "initRequest is not null but state is not USER");
            _initRequest = arg_initRequest;
        }
        _responseString = arg_responseString;
    }

    // Joint instruction method (payload string for the LLM API request?)
    public string JointInstruction
    {
        get
        {
            if (_initRequest == null) return _responseString;
            var output = _initRequest.JointInstruction + '\n' + _responseString;
            Debug.Assert(output != null, "JointInstruction output is incorrectly null");
            return output;
        }
    }

    // Role method (feeding instruction or actual conversation persona?)
    public string Role
    {
        get
        {
            var output = (State == ModelInputState.USER) ? "user" : "user";
            Debug.Assert(output != null, "Role output is incorrectly null");
            return output;
        }
    }
}

public class StructuredRequest
{
    private readonly string _sceneInstruction;
    private readonly List<string> _checkpointList;
    private readonly string _constraintInstruction;

    public string JointInstruction
    {
        get
        {
            return sceneFormat() + "\n" + checkpointFormat() + "\n" + constraintFormat();
        }
    }

    private string sceneFormat()
    {
        if (_sceneInstruction == null) return "";
        string _local = "You are in a scene where: ";
        _local += _sceneInstruction;
        return _local;
    }

    private string checkpointFormat()
    {
        if (_checkpointList == null) return "";
        string _local = "Your objective is to fulfill these checkpoints: ";
        for (int ckpt_num = 0; ckpt_num < _checkpointList.Count; ckpt_num++)
        {
            _local += ((ckpt_num == 0) ? "" : " + ") + _checkpointList[ckpt_num];
        }
        return _local;
    }

    private string constraintFormat()
    {
        if (_constraintInstruction == null) return "";
        string _local = "You must also adhere to the following constraints: ";
        _local += _constraintInstruction;
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

public class StructuredParameter
{
    public string Model { get; set; }
    public int MaxTokens { get; set; }
    public List<string> Stop { get; set; }
    public double Temperature { get; set; }
    public double TopP { get; set; }
    public int TopK { get; set; }
    public int RepetitionPenalty { get; set; }
    public int N { get; set; }

    public StructuredParameter(double arg_temperature = 0.7,
                               double arg_topP = 0.7,
                               int arg_topK = 50,
                               int arg_repetitionPenalty = 1,
                               int arg_n = 1,
                               string arg_model = "meta-llama/Llama-2-70b-chat-hf",
                               int arg_maxTokens = 128,
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