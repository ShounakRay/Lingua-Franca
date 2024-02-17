using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class SpeechListener : MonoBehaviour
{
    // I have another script file that has the SavWav class. I want to access that class's static variables here. How do i do it.
    private AudioClip microphoneClip;
    private string microphoneName;
    
    public string savePath = "/Users/katie/Documents/UnityProjects/Treehacks2024VR/Assets/SavedAudio.wav";
    // Use the savewav class to save the audio to a file
    // Start is called before the first frame update
    void Start()
    {
        microphoneName = Microphone.devices[0];        
        Debug.Log("Microphone name: " + microphoneName);

    }

    // Update is called once per frame
    void Update()
    {
        /* TODO: make the activation for the microphone
            an in game button press */
        // Debug.Log("How about here?");
    }

    public void StartRecording()
    {
        microphoneClip = Microphone.Start(microphoneName, true, 60, 44100);
        if (microphoneClip == null)
        {
            Debug.Log("Microphone clip is null");
        }
    }

    public void StopRecordingAndPlayAudio()
    {
        Microphone.End(microphoneName); // stop the recording
        SavWav.TrimSilence(microphoneClip, 0.001f);
        SavWav.Save("/Users/katie/Documents/UnityProjects/Treehacks2024VR/Assets/SavedAudio.wav", microphoneClip);
        // StartCoroutine(PlayAudioForUser());
    }

    IEnumerator PlayAudioForUser() {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = microphoneClip;
        audio.Play();
    }

}

