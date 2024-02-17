using System.Collections.Generic;
using System.Diagnostics;
using Unity.Burst.CompilerServices;

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
                          StructuredRequest arg_initRequest = null,
                          string arg_responseString = null)
    {
        if (arg_initRequest == null && arg_responseString == null)
            Debug.Assert(false, "Both initRequest and responseString are null");
        else if (arg_initRequest != null && arg_responseString != null)
            Debug.Assert(false, "Both initRequest and responseString are not null");
        else if (arg_initRequest != null)
        {
            Debug.Assert(arg_state == ModelInputState.SYSTEM, "initRequest is not null but state is not SYSTEM");
            _initRequest = arg_initRequest;
        }
        else if (arg_responseString != null)
            _responseString = arg_responseString;
    }

    // Joint instruction method (payload string for the LLM API request?)
    public string JointInstruction
    {
        get
        {
            var output = (State == ModelInputState.SYSTEM) ? _initRequest.JointInstruction : _responseString;
            Debug.Assert(output != null, "JointInstruction output is incorrectly null");
            return output;
        }
    }

    // Role method (feeding instruction or actual conversation persona?)
    public string Role
    {
        get
        {
            var output = (State == ModelInputState.SYSTEM) ? "system" : "user";
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
        string _local = "You are in a scene where: ";
        _local += _sceneInstruction;
        return _local;
    }

    private string checkpointFormat()
    {
        string _local = "Your objective is to fulfill these checkpoints: ";
        for (int ckpt_num = 0; ckpt_num < _checkpointList.Count; ckpt_num++)
        {
            _local += ((ckpt_num == 0) ? "" : " + ") + _checkpointList[ckpt_num];
        }
        return _local;
    }

    private string constraintFormat()
    {
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
    private readonly string ç = "mistralai/Mixtral-8x7B-Instruct-v0.1";

    public string Model { get; set; }
    public int MaxTokens { get; set; }
    public List<string> Stop { get; set; }
    public double Temperature { get; set; }
    public double TopP { get; set; }
    public int TopK { get; set; }
    public double RepetitionPenalty { get; set; }
    public int N { get; set; }

    public StructuredParameter(double arg_temperature = 0.7,
                               double arg_topP = 0.7,
                               int arg_topK = 50,
                               double arg_repetitionPenalty = 1,
                               int arg_n = 1,
                               string arg_model = "mistralai/Mixtral-8x7B-Instruct-v0.1",
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