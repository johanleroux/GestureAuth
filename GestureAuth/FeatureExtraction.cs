using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace GestureAuth
{
    class FeatureExtraction
    {
        private Mat _frame;
        private VectorOfVectorOfPoint _contours;

        private VectorOfPoint _handContour = new VectorOfPoint();
        private double _handArea;

        public FeatureExtraction(Mat frame)
        {
            _frame = frame;
        }

        public Mat process()
        {
            Mat processFrame = _frame.Clone();

            // Segment frame to only include ROI area
            CvInvoke.Rectangle(processFrame, new Rectangle(new Point(0, 0), new Size(160, 480)), new MCvScalar(0, 0, 0), -1);

            // Fetch contours of the hand
            processFrame = fetchContours(processFrame);

            // Determine the bounding box of the hand
            processFrame = determineHandLocation(processFrame);

            return processFrame;
        }

        public Mat fetchContours(Mat processFrame)
        {
            _contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(processFrame, _contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            if (_contours.Size == 0) return processFrame;

            _handArea = 0;
            for (int i = 0; i < _contours.Size; i++)
            {
                VectorOfPoint contour = _contours[i];

                double area = CvInvoke.ContourArea(contour);
                if (area > _handArea)
                {
                    _handArea = area;
                    _handContour = contour;
                }
            }

            return processFrame;
        }

        private Mat determineHandLocation(Mat processFrame)
        {
            Config.handBoundingBox = CvInvoke.BoundingRectangle(_handContour);

            if(Config.debug)
                CvInvoke.Rectangle(processFrame, Config.handBoundingBox, Config.drawingColor);

            Config.handPoint = new Point(Config.handBoundingBox.X + Config.handBoundingBox.Width / 2, Config.handBoundingBox.Y + Config.handBoundingBox.Height / 2);

            return processFrame;
        }
    }
}
