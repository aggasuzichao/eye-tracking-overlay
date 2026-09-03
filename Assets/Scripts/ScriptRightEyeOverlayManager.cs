using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.ObjdetectModule;
using UnityEngine;
using UnityEngine.UI;


public class ScriptRightEyeOverlayManager : MonoBehaviour
{

    public ScriptRightEyeDetectionManager rightEyeDetectionManagerScript;
    public Image pirateEyepatchImage;
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
        if (rightEyeDetectionManagerScript == null || rightEyeDetectionManagerScript.webcamManagerScript.rgbMat.empty())
        {
            return;
        }

        MatOfRect righteye = new MatOfRect();
        rightEyeDetectionManagerScript.rightEyeDetector.detectMultiScale(rightEyeDetectionManagerScript.grayMat, righteye, 1.1, 3, 0, new Size(30, 30), new Size());

        if (righteye.toArray().Length > 0)
        {
            OpenCVForUnity.CoreModule.Rect eye = righteye.toArray()[0];

            Vector2 uiPosition = ConvertFaceToUIPosition(eye, rawImageTransform);

            pirateEyepatchImage.rectTransform.anchoredPosition = uiPosition;

            pirateEyepatchImage.rectTransform.sizeDelta = new Vector2(eye.width * adjustSize, eye.height * adjustSize);

        }
    }

    private Vector2 ConvertFaceToUIPosition(OpenCVForUnity.CoreModule.Rect face, RectTransform rawImage)
    {
        float normalizedX = (float)face.x / rightEyeDetectionManagerScript.webcamManagerScript.requestedWidth;
        float normalizedY = 1f - (float)face.y / rightEyeDetectionManagerScript.webcamManagerScript.requestedHeight;

        float posX = (normalizedX - 0.5f) * rawImage.rect.width;
        float posY = (normalizedY - 0.5f) * rawImage.rect.height;

        return new Vector2(posX + adjustX, posY + adjustY);

    }
}
