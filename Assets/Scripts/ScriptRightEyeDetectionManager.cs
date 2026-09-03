using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine;
using UnityEngine.UI;

public class ScriptRightEyeDetectionManager : MonoBehaviour
{
    public ScriptWebcamManager webcamManagerScript;
    public TextAsset haarcascade;
    public CascadeClassifier rightEyeDetector;
    public RawImage rightEyeDetectionDisplay;
    public Mat grayMat;

    private Texture2D rightEyeDetectedTexture;
    private Mat rightEyeDetectedMat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rightEyeDetector = new CascadeClassifier(Application.dataPath + "/Resources/haarcascade_righteye_2splits.xml");

        if (rightEyeDetector.empty())
        {
            Debug.LogError("Failed to load Haar Cascade XML file.");
            return;
        }

        grayMat = new Mat();
        rightEyeDetectedMat = new Mat();

        rightEyeDetectedTexture = new Texture2D(webcamManagerScript.requestedWidth, webcamManagerScript.requestedHeight, TextureFormat.RGBA32, false);

        rightEyeDetectionDisplay.texture = rightEyeDetectedTexture;
    }

    // Update is called once per frame
    void Update()
    {
        if (webcamManagerScript.rgbMat.empty())
        {
            return;
        }

        Imgproc.cvtColor(webcamManagerScript.rgbMat, grayMat, Imgproc.COLOR_RGB2GRAY);

        MatOfRect righteye = new MatOfRect();

        rightEyeDetector.detectMultiScale(grayMat, righteye, 1.1, 3, 0, new Size(30, 30), new Size());

        webcamManagerScript.rgbMat.copyTo(rightEyeDetectedMat);

        foreach (OpenCVForUnity.CoreModule.Rect eye in righteye.toArray())
        {
            Imgproc.rectangle(rightEyeDetectedMat, eye.tl(), eye.br(), new Scalar(0, 255, 0), 2);

        }

        Utils.matToTexture2D(rightEyeDetectedMat, rightEyeDetectedTexture);
        rightEyeDetectionDisplay.texture = rightEyeDetectedTexture;
    }
}
