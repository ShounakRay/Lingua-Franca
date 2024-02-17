using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MicrophoneToText : MonoBehaviour
{
    public SpeechListener detector;
    private InputDevice targetDevice;
    public ReadInput readInput;

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
        else {
            Debug.Log("No devices found");
            // Add component of type Readinput to the game object
            GameObject readInputObj = new GameObject("ReadInput");
            readInputObj.AddComponent<ReadInput>();
            readInput = readInputObj.GetComponent<ReadInput>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        //check if primay button is pressed down
        if (readInput) 
        {
            if (readInput.ButtonDown()) 
            {
                detector.StartRecording();
                Debug.Log("Recording started");
                Debug.Log("Primary button pressed");
                transform.localScale = Vector3.one * 1.5f;
            } 
            else if (readInput.ButtonUp()) 
            {
                detector.StopRecordingAndPlayAudio();
                Debug.Log("Recording stopped");
                Debug.Log("Primary button released");
            }
        }

        if (Input.GetKeyDown("space")) {
            if (detector == null) {
                Debug.Log("Detector is null");
            }
            else {
                detector.StartRecording();
                Debug.Log("Recording started");
            }
            transform.localScale = Vector3.one * 1.5f;
        }

        if (Input.GetKeyUp("space")) {
            if (detector == null) {
                Debug.Log("Detector is null");
            }
            else {
                detector.StopRecordingAndPlayAudio();
                Debug.Log("Recording stopped");
            }
        }
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