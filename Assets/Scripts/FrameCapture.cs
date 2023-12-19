using UnityEngine;
using System.IO;

public class FrameCapture : MonoBehaviour
{
    private bool isRecording = false;
    private string recordingPath;
    private float startTime;
    private int frameCount = 0;

    void Start()
    {
        // Set the recording path to the project's Assets folder
        recordingPath = Application.dataPath + "/Recording/";
    }

    void Update()
    {
        // Start/stop recording on key press
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isRecording)
                StopRecording();
            else
                StartRecording();
        }
    }

    void StartRecording()
    {
        // Create a unique folder for each recording session
        string folderName = "Recording_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        recordingPath += folderName + "/";

        // Create the folder if it doesn't exist
        if (!Directory.Exists(recordingPath))
            Directory.CreateDirectory(recordingPath);

        // Start recording
        isRecording = true;
        startTime = Time.time;
        frameCount = 0;
    }

    void StopRecording()
    {
        // Stop recording
        isRecording = false;

        // Reset the recording path
        recordingPath = Application.dataPath + "/Recording/";

        Debug.Log("Recording saved at " + recordingPath);
    }

    void LateUpdate()
    {
        // Capture frames while recording
        if (isRecording)
        {
            // Capture the current frame as a screenshot
            string screenshotPath = recordingPath + frameCount.ToString("0000") + ".png";
            ScreenCapture.CaptureScreenshot(screenshotPath);
            frameCount++;
        }
    }
}
