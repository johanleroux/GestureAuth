# GestureAuth
Gesture Auth is a attempt to provide an interface for contactless authentication through biometrical hand gestures. Written in C# with the EmguCV wrapper around OpenCV.

## Pipeline
### Pre-processing
 - Blur image for a better range (unsharpen image)
 - Convert image to a HSV colour spectrum
 - Limit the colour range to match the hand
 - Erode image to reduce noise
 - Dilate image to fill open areas
 - (Optional) Threshold to a binary black or white
### Feature Extraction
 - Segment image to only include ROI area
 - Fetch contours of hand
 - Determine bounding box of hand

## Usage
 - Run program and follow instructions on screen
 - F10 for debug
 - F11 for fullscreen
 - R for resetting

## License
The MIT License (MIT). Please see [License File](LICENSE) for more information.
