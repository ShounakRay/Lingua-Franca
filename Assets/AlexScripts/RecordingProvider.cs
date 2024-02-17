using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RecordingProvider: IRecordingProvider
{
    private AudioClip microphoneClip;
    void StartRecording()
    {
        microphoneClip = Microphone.Start(microphoneName, true, 60, 44100);
    }

    Task<string> StopRecording()
    {
        Microphone.End(microphoneName); // stop the recording

        // TODO: speech to text with the microphoneClip
    }
}