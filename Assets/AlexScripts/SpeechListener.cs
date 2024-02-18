using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class SpeechListener : MonoBehaviour
{
    // I have another script file that has the SavWav class. I want to access that class's static variables here. How do i do it.
    private AudioClip microphoneClip;
    private string microphoneName;

    public AudioSource audio;
    
    public string filename = "Treehacks2024SavedAudio.wav";
    // Use the savewav class to save the audio to a file
    // Start is called before the first frame update

    void Start()
    {
        microphoneName = Microphone.devices[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartRecording()
    {
        microphoneClip = Microphone.Start(microphoneName, true, 60, 44100);
        Debug.Log ("Recording started");
        if (microphoneClip == null)
        {
            Debug.Log("Microphone clip is null");
        }
    }

    public void StopRecordingAndSave()
    {
        Microphone.End(microphoneName); // stop the recording
        SavWav.TrimSilence(microphoneClip, 0.001f);
        SavWav.Save(filename, microphoneClip);
        Debug.Log("Recording stopped... Saving audio to file.");
    }

    public string ConvertSpeechToText()
    {
        string response = "";
        
        StartCoroutine(SpeechToText.PostRequest((jsonResponse) =>
        {
            if (jsonResponse != null)
            {
                // Handle the JSON response
                Debug.Log("Received JSON response: " + jsonResponse);
            }
            else
            {
                // Handle the error
                Debug.LogError("Failed to receive JSON response");
            }
        }, filename));

        return response;
    }

    public void PlayAudioForUser() {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio == null) {
            Debug.Log("Audio source is null");
        }
        audio.PlayOneShot(microphoneClip);
    }

}

