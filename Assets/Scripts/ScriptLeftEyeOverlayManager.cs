using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.ObjdetectModule;
using UnityEngine;
using UnityEngine.UI;


public class ScriptFaceOverlayManager : MonoBehaviour
{

    public ScriptLeftEyeDetectionManager leftEyeDetectionManagerScript;
    public Image animeEyeImage;
    public RectTransform rawImageTransform;

    public float adjustSize = 2.0f;
    public float adjustX;
    public float adjustY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (leftEyeDetectionManagerScript == null || leftEyeDetectionManagerScript.webcamManagerScript.rgbMat.empty())
        {
            return;
        }

        MatOfRect lefteye = new MatOfRect();
        leftEyeDetectionManagerScript.leftEyeDetector.detectMultiScale(leftEyeDetectionManagerScript.grayMat, lefteye, 1.1, 3, 0, new Size(30, 30), new Size());

        if (lefteye.toArray().Length > 0)
        {
            OpenCVForUnity.CoreModule.Rect eye = lefteye.toArray()[0];

            Vector2 uiPosition = ConvertFaceToUIPosition(eye, rawImageTransform);

            animeEyeImage.rectTransform.anchoredPosition = uiPosition;

            animeEyeImage.rectTransform.sizeDelta = new Vector2(eye.width * adjustSize, eye.height * adjustSize);

        }
    }

    private Vector2 ConvertFaceToUIPosition(OpenCVForUnity.CoreModule.Rect eye, RectTransform rawImage)
    {
        float normalizedX = (float)eye.x / leftEyeDetectionManagerScript.webcamManagerScript.requestedWidth;
        float normalizedY = 1f - (float)eye.y / leftEyeDetectionManagerScript.webcamManagerScript.requestedHeight;

        float posX = (normalizedX - 0.5f) * rawImage.rect.width;
        float posY = (normalizedY - 0.5f) * rawImage.rect.height;

        return new Vector2(posX + adjustX, posY + adjustY);

    }
}
