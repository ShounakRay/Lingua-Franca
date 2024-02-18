using System.Threading.Tasks;
using UnityEngine;

// async void ResponseSuggestionJoint_Test(int cycles)
// {
//     string input = null;
//     for (int i = 0; i < cycles; i++)
//     {
//         input = await ResponseLLM_Test(responseProvider as LLM_ResponseProvider, input);
//         input = await SuggestionsLLM_Test(input);
//         Debug.Log(i);
//     }
// }

// async void SOLE_ResponseLLM_Test()
// {
//     LLM_ResponseProvider provider = new(info, null);
//     string response_1 = await provider.GetResponse(null);
//     Debug.Log("[Actor.Awake.ResponseLLM_Test] Response from LLM_ResponseProvider.GetResponse(null):");
//     Debug.Log(response_1);
// }

// async Task<string> SuggestionsLLM_Test(string prompt_input)
// {
//     LLM_SuggestionProvider provider = new(parameters: null);
//     string output = await provider.GetResponse(prompt_input);
//     return output;
// }

// async Task<string> ResponseLLM_Test(LLM_ResponseProvider provider, string prompt_input)
// {
//     string output = await provider.GetResponse(prompt_input);
//     return output;
// }