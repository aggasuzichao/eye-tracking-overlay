# Eye Tracking Overlay

A real-time eye tracking system built in Unity with OpenCV, detecting the left and right eye independently so each can carry its own overlay sprite. University project, 2025.

## Why separate detectors

Most eye tracking treats the pair as one object. This uses two Haar cascades, one trained for each eye, so the two are identified as left and right rather than as two anonymous rectangles. That is what makes per-eye overlays possible: a different sprite on each eye, correctly placed, rather than the same sprite mirrored.

## How it works

A webcam manager owns the capture and exposes the current frame as an OpenCV Mat, so every other script reads from one source rather than each opening its own camera.

Each detection manager converts that frame from RGB to grayscale, because Haar cascades operate on intensity and passing colour through wastes work. It then runs detectMultiScale with a minimum size of 30 by 30 pixels, which discards the small false positives that otherwise appear in hair and shadow.

Each overlay manager takes the resulting rectangle, converts its position from camera pixel coordinates into Unity UI space, and places its sprite there, scaled to the detected eye.

## Files

Assets/Scripts/ScriptWebcamManager.cs, the shared capture source.

Assets/Scripts/ScriptLeftEyeDetectionManager.cs and ScriptRightEyeDetectionManager.cs, one Haar cascade each.

Assets/Scripts/ScriptLeftEyeOverlayManager.cs and ScriptRightEyeOverlayManager.cs, sprite placement per eye.

## What is not here

The Unity project itself, which is large and mostly the OpenCVForUnity package. The cascade XML files ship with OpenCV. Only the scripts written for this project are in this repository.

## Notes

Prototyped in Python first, where the detection loop is faster to iterate on, then ported to C# and Unity. The port is where most of the work went. Coordinate systems differ between the camera frame and the Unity canvas, and the colour conversion has to be explicit because the webcam texture arrives in a different channel order than OpenCV expects.

Agga Thu, Bangkok. agga.suzichao@gmail.com
