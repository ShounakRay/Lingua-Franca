using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events; 
using Oculus.Voice;
using System.Linq;
using UnityEngine.XR;

public class RecordingProvider : MonoBehaviour, IRecordingProvider
{
    [Header("Voice")]
    [SerializeField]
    private AppVoiceExperience appVoiceExperience;

    [SerializeField]
    private string partialTranscriptText;

    private bool appVoiceActive;

    public event Action<string> OnRecordingUpdated;


    public void RaiseRecordingUpdated(string transcription)
    {
        OnRecordingUpdated?.Invoke(transcription);
    }

    private void Awake ()
    {
        partialTranscriptText = string.Empty;

        appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener ((transcription) => {
            partialTranscriptText = transcription;
            Debug.Log("Partial transcription: " + transcription);

            /* Invoked on each update to the partial transcription */
            RaiseRecordingUpdated(transcription);
        });

        appVoiceExperience.VoiceEvents.OnSend.AddListener ((request) => {
            appVoiceActive = true;
            Debug.Log("App voice is active");
        });

        appVoiceExperience.VoiceEvents.OnRequestCompleted.AddListener (() => {
            appVoiceActive = false;
            Debug.Log("App voice is inactive");
        });
    }

    /* Starts a recording. Will not start another 
        recording if one is already active */
    public void StartRecording()
    {   
        if (!appVoiceActive)
        {
            appVoiceExperience.Activate();
        }
    }

    public async Task<string> StopRecording()
    {
        /* Does not pause recording atm, will fix this on next commit */
        while (appVoiceActive && partialTranscriptText != string.Empty)
        {
            await Task.Delay(100);
        }

        return partialTranscriptText;
    }
}
