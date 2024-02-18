using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
public class SpeechToText {
    /* Takes WAV file and returns the text of the speech in the file */
    private static string WIT_API_KEY = "BWD7ZXX4TIXQJ2PANV3QFR44OJBK4SWE";

    public static IEnumerator PostRequest (System.Action<string> callback, string filename) {
        string contentType = filename;
        // List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        // string auth = "Authorization=Bearer " + WIT_API_KEY;
        // formData.Add(new MultipartFormDataSection(auth));
        // formData.Add(new MultipartFormFileSection("audio/wav", filename));
        // UnityWebRequest request = UnityWebRequest.Post("https://api.wit.ai/dictation?v=20230215", 
        //                                                        formData);

        UnityWebRequest request = new UnityWebRequest("https://api.wit.ai/speech?v=20230215", "POST");

        request.SetRequestHeader("Authorization", "Bearer " + WIT_API_KEY);
        request.SetRequestHeader("Content-Type", "audio/wav");

        byte[] fileData = System.IO.File.ReadAllBytes(filename);

        request.uploadHandler = new UploadHandlerRaw(fileData);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Request was successful, invoke the callback with the JSON response
            Debug.Log("Received response: " + request.downloadHandler.text);
            Debug.Log("Received response: " + request.result);
            callback?.Invoke(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("POST request failed. Error: " + request.error);
            Debug.Log("POST request failed. Error: " + request.result);
            // There was an error, invoke the callback with null
            callback?.Invoke(null);
        }                                                       
        
    }


}