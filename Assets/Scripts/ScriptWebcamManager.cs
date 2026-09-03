using OpenCVForUnity.VideoioModule;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ImgprocModule;
using UnityEngine;
using UnityEngine.UI;

public class ScriptWebcamManager : MonoBehaviour
{
    private VideoCapture videoCapture;
    private Texture2D webcamTexture;
    private Mat bgrMat;

    public RawImage webcamDisplay;
    public int requestedWidth = 960;
    public int requestedHeight = 540;
    public Mat rgbMat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        videoCapture = new VideoCapture(0);

        videoCapture.set(Videoio.CAP_PROP_FRAME_WIDTH, requestedWidth);
        videoCapture.set(Videoio.CAP_PROP_FRAME_HEIGHT, requestedHeight);

        if (!videoCapture.isOpened())
        {
            Debug.LogError("Failed to open webcam.");
            return;
        }

        bgrMat = new Mat();
        rgbMat = new Mat();

        webcamTexture = new Texture2D(requestedWidth, requestedHeight, TextureFormat.RGBA32, false);

        webcamDisplay.texture = webcamTexture;

    }

    // Update is called once per frame
    void Update()
    {
        if (videoCapture.read(bgrMat))
        {
            Imgproc.cvtColor(bgrMat, rgbMat, Imgproc.COLOR_BGR2RGB);

            Utils.matToTexture2D(rgbMat, webcamTexture);

        }
    }
}
