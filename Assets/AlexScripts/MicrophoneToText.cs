using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Net;
using System.IO;

public class MicrophoneToText : MonoBehaviour
{
    public SpeechListener detector;
    public ReadInput readInput;


    private InputDevice targetDevice;

    private bool recordingStarted = false;


    // Start is called before the first frame update
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

            // Add component of type Readinput to the game object
            // GameObject readInputObj = new GameObject("ReadInput");
            // readInputObj.AddComponent<ReadInput>();
            // readInput = readInputObj.GetComponent<ReadInput>();
        }
        else {
            Debug.Log("No devices found");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        // if (primaryButtonValue && !recordingStarted) {
        //     recordingStarted = true;
        //     detector.StartRecording();
        //     Debug.Log("Primary button is pressed");
        // }

        // if (!primaryButtonValue && recordingStarted) {
        //     detector.StopRecordingAndSave();
        //     detector.PlayAudioForUser();
        //     string text = detector.ConvertSpeechToText();
        //     Debug.Log("Primary button is released");
        //     recordingStarted = false;
        // }

        // change the object related to this script to blue color 
        // if the primary button is pressed down


        //check if primay button is pressed down
        // if (readInput) 
        // {
        //     if (readInput.ButtonDown()) 
        //     {
        //         detector.StartRecording();
        //         Debug.Log("Recording started");
        //         Debug.Log("Primary button pressed");
        //         transform.localScale = Vector3.one * 1.5f;
        //     } 
    
        //     {
        //         detector.StopRecordingAndPlayAudio();
        //         Debug.Log("Recording stopped");
        //         Debug.Log("Primary button released");
        //     }
        // }

        // if (Input.GetKeyDown("space")) {
        //     if (detector == null) {
        //         Debug.Log("Detector is null");
        //     }
        //     else {
        //         detector.StartRecording();
        //         Debug.Log("Recording started");
        //     }
        //     transform.localScale = Vector3.one * 1.5f;
        // }

        // if (Input.GetKeyUp("space")) {
        //     if (detector == null) {
        //         Debug.Log("Detector is null");
        //     }
        //     else {
        //         detector.StopRecordingAndPlayAudio();
        //         Debug.Log("Recording stopped");
        //     }
        // }
    }

}



    public class ReadInput : MonoBehaviour
    {
        bool wasDown = false;
        bool isDown = false;
       
        //Assigned to the RightHand Controller
        public InputDevice device;

        public ReadInput(InputDevice device) {
            this.device = device;
        }
 
        void Start ()
        {
        }
 
        void Update() {
            wasDown = isDown;
            device.TryGetFeatureValue(CommonUsages.primaryButton, out isDown);
        }
       
        public bool ButtonDown() {
            return isDown && !wasDown;
        }
        public bool Button() {
            return isDown;
        }
        public bool ButtonUp() {
            return wasDown && !isDown;
        }
    }