using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine;
using UnityEngine.UI;

public class ScriptLeftEyeDetectionManager : MonoBehaviour
{
    public ScriptWebcamManager webcamManagerScript;
    public TextAsset haarcascade;
    public CascadeClassifier leftEyeDetector;
    public RawImage leftEyeDetectionDisplay;
    public Mat grayMat;

    private Texture2D leftEyeDetectedTexture;
    private Mat leftEyeDetectedMat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftEyeDetector = new CascadeClassifier(Application.dataPath + "/Resources/haarcascade_lefteye_2splits.xml");

        if (leftEyeDetector.empty())
        {
            Debug.LogError("Failed to load Haar Cascade XML file.");
            return;
        }

        grayMat = new Mat();
        leftEyeDetectedMat = new Mat();

        leftEyeDetectedTexture = new Texture2D(webcamManagerScript.requestedWidth, webcamManagerScript.requestedHeight, TextureFormat.RGBA32, false);

        leftEyeDetectionDisplay.texture = leftEyeDetectedTexture;
    }

    // Update is called once per frame
    void Update()
    {
        if (webcamManagerScript.rgbMat.empty())
        {
            return;
        }

        Imgproc.cvtColor(webcamManagerScript.rgbMat, grayMat, Imgproc.COLOR_RGB2GRAY);

        MatOfRect lefteye = new MatOfRect();

        leftEyeDetector.detectMultiScale(grayMat, lefteye, 1.1, 3, 0, new Size(30, 30), new Size());

        webcamManagerScript.rgbMat.copyTo(leftEyeDetectedMat);

        foreach (OpenCVForUnity.CoreModule.Rect eye in lefteye.toArray())
        {
            Imgproc.rectangle(leftEyeDetectedMat, eye.tl(), eye.br(), new Scalar(0, 255, 0), 2);

        }

        Utils.matToTexture2D(leftEyeDetectedMat, leftEyeDetectedTexture);
        leftEyeDetectionDisplay.texture = leftEyeDetectedTexture;
    }
}
