using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;

namespace GestureAuth
{
    class Preprocess
    {
        private Mat _frame;

        public Preprocess(Mat frame)
        {
            _frame = frame;
        }

        public Mat process()
        {
            Mat processFrame = _frame.Clone();

            // Blur image for a better range
            processFrame = blur(processFrame);

            // Convert frame colour to HSV spectrum
            processFrame = convertToHsv(processFrame);

            // Limit colour range to match hand
            processFrame = limitRange(processFrame);

            // Erode image to reduce noise
            processFrame = dilate(processFrame);

            // Dilate image to fill open areas
            processFrame = dilate(processFrame);

            // Threshold the image; either pure white or black
            processFrame = threshold(processFrame);

            return processFrame;            
        }

        public Mat convertToHsv(Mat processFrame)
        {
            Mat tmpFrame = new Mat();

            CvInvoke.CvtColor(processFrame, tmpFrame, ColorConversion.Bgr2Hsv);

            return tmpFrame;
        }

        public Mat limitRange(Mat processFrame)
        {
            Mat tmpFrame = new Mat();
            Hsv skinColorLowerLimit = new Hsv(Config.skinLowerLimit_1, Config.skinLowerLimit_2, Config.skinLowerLimit_3);
            Hsv skinColorUpperLimit = new Hsv(Config.skinUpperLimit_1, Config.skinUpperLimit_2, Config.skinUpperLimit_3);

            CvInvoke.InRange(processFrame, new ScalarArray(skinColorLowerLimit.MCvScalar), new ScalarArray(skinColorUpperLimit.MCvScalar), tmpFrame);

            return tmpFrame;
        }

        public Mat erode(Mat processFrame)
        {
            Mat tmpFrame = new Mat();
            Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(10, 10), new Point(0, 0));

            CvInvoke.Erode(processFrame, processFrame, kernel, new Point(0, 0), 5, BorderType.Constant, new MCvScalar(0));

            return tmpFrame;
        }

        public Mat dilate(Mat processFrame)
        {
            Mat tmpFrame = new Mat();
            Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(3, 3), new Point(0, 0));

            CvInvoke.Dilate(processFrame, tmpFrame, kernel, new Point(0, 0), 3, BorderType.Constant, new MCvScalar(0));

            return tmpFrame;
        }

        public Mat blur(Mat processFrame)
        {
            Mat tmpFrame = new Mat();

            CvInvoke.GaussianBlur(processFrame, tmpFrame, new Size(5, 5), 100);

            return tmpFrame;
        }

        public Mat threshold(Mat processFrame)
        {
            Mat tmpFrame = new Mat();

            CvInvoke.Threshold(processFrame, tmpFrame, Config.threshold_1, Config.threshold_2, ThresholdType.Binary);

            return tmpFrame;
        }
    }
}
