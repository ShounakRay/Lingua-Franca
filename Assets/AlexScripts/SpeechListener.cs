using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechListener : MonoBehaviour
{
    private AudioClip microphoneClip;
    private string microphoneName;
    public string savePath = "Assets/SavedAudio.wav";
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
        Debug.Log("How about here?");
    }

    public void StartRecording()
    {
        microphoneClip = Microphone.Start(microphoneName, true, 60, 44100);
    }

    public void StopRecordingAndPlayAudio()
    {
        Microphone.End(microphoneName); // stop the recording
        // SavWav.Save(savePath, microphoneClip);
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
