using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestureAuth
{
    public static class Util
    {
        public static bool isWithin(Point biggerCircle, int biggerRadius, Point smallerCircle, int smallerRadius)
        {
            int dx = biggerCircle.X - smallerCircle.X;
            int dy = biggerCircle.Y - smallerCircle.Y;

            int Dsqr = dx * dx + dy * dy;
            int rdiff = Math.Abs(biggerRadius - smallerRadius);
            int rsum = smallerRadius + biggerRadius;

            return rdiff * rdiff < Dsqr && Dsqr < rsum * rsum;
        }

        public static int center(int point, int width)
        {
            return point + width / 2;
        }
    }
}
