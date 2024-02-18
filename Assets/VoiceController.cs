using Oculus.Voice;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class VoiceController : MonoBehaviour 
{
    [Header("Voice")]
    [SerializeField]
    private AppVoiceExperience appVoiceExperience;

    [SerializeField]
    private TextMeshPro fullTranscriptText;

    [SerializeField]
    private TextMeshPro partialTranscriptText;

    private bool appVoiceActive;

    private InputDevice targetDevice;

    private bool recordingStarted = false;

    private void Awake()
    {
        fullTranscriptText.text = string.Empty;
        partialTranscriptText.text = string.Empty;

        appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener ((transcription) => {
            fullTranscriptText.text = transcription;
            Debug.Log("Full transcription: " + transcription);
        });

        appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener ((transcription) => {
            partialTranscriptText.text = transcription;
            Debug.Log("Partial transcription: " + transcription);
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

    void Update()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        if (primaryButtonValue && !appVoiceActive) {
            recordingStarted = true;
            appVoiceExperience.Activate();
            Debug.Log("Primary button is pressed");
        }

        // if (!primaryButtonValue && recordingStarted) {
        //     Debug.Log("Primary button is released");
        //     recordingStarted = false;
        // }
    }

    void Start()
    {
        List <InputDevice> inputDevices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, inputDevices);

        foreach (var device in inputDevices) {
            Debug.Log("Device found with name: " + device.name + " and characteristics: " + device.characteristics);
        }

        if (inputDevices.Count > 0) {
            targetDevice = inputDevices[0];
            Debug.Log("Target device name: " + targetDevice.name);
        }
        else {
            Debug.Log("No devices found");
        }

    }

}
