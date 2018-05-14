using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Drawing;

namespace GestureAuth
{
    public static class Config
    {
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static int skinLowerLimit_1 = 0;
        public static int skinLowerLimit_2 = 10;
        public static int skinLowerLimit_3 = 60;

        public static int skinUpperLimit_1 = 20;
        public static int skinUpperLimit_2 = 150;
        public static int skinUpperLimit_3 = 255;

        public static int threshold_1 = 150;
        public static int threshold_2 = 255;

        public static MCvScalar drawingColor = new MCvScalar(255, 255, 255);

        public static MCvScalar nodeDeactive = new Rgb(Color.Chocolate).MCvScalar;
        public static MCvScalar nodeActive = new Rgb(Color.Blue).MCvScalar;
        public static MCvScalar nodeSuccess = new Rgb(Color.Green).MCvScalar;

        public static int nodeRadius = 20;

        public static int ansRadius = 35;
        public static Point ansPoint = new Point(400, 240);

        public static int handRadius = 50;

        public static FontFace fontFace = FontFace.HersheyPlain;

        public static bool fullscreen = false;
        public static bool debug = false;

        public static Point handPoint;
        public static Rectangle handBoundingBox;


    }
}
