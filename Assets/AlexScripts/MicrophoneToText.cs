using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MicrophoneToText : MonoBehaviour
{
    public SpeechListener detector;
    private InputDevice targetDevice;

    // Start is called before the first frame update
    void Start()
    {
        List <InputDevice> inputDevices = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, inputDevices);

        foreach (var device in inputDevices) {
            Debug.Log("Device found with name: " + device.name + " and characteristics: " + device.characteristics);
        }

        if (inputDevices.Count > 0) {
            targetDevice = inputDevices[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        if (primaryButtonValue) {
            detector.StartRecording();
            Debug.Log("Recording started");
            transform.localScale = Vector3.one * 1.5f;
        } 

        Debug.Log("Calling update?");
        // else {
        //     detector.StopRecordingAndPlayAudio();
        //     Debug.Log("Recording stopped");
        // }

        // if (Input.GetKeyDown("space")) {
        //     detector.StartRecording();
        //     Debug.Log("Recording started");
        //     transform.localScale = Vector3.one * 1.5f;
        // }

        // if (Input.GetKeyUp("space")) {
        //     detector.StopRecordingAndPlayAudio();
        //     Debug.Log("Recording stopped");
        // }
    }

}
