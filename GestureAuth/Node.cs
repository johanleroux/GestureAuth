using System;
using System.Drawing;
using Emgu.CV.Structure;

namespace GestureAuth
{
    public class Node
    {
        public int id;
        public Point point;
        private bool _active = false;
        private long _attachTime = long.MaxValue;
        private long _answerTime = long.MaxValue;

        public Node(int id, Point point)
        {
            this.id = id;
            this.point = point;
        }

        public MCvScalar getColour()
        {
            if (_active)
                return Config.nodeActive;

            return Config.nodeDeactive;
        }

        public bool isActive()
        {
            return _active;
        }

        public void activate()
        {
            _active = true;
        }

        public void deactivate()
        {
            _active = false;
        }

        public void attach()
        {
            if (_attachTime == long.MaxValue)
                _attachTime = (long)(DateTime.UtcNow - Config.UnixEpoch).TotalMilliseconds;

            if (_attachTime + 1000 < (long)(DateTime.UtcNow - Config.UnixEpoch).TotalMilliseconds)
                activate();
        }

        public void detach()
        {
            _attachTime = long.MaxValue;
            deactivate();
        }

        public Point currentPoint()
        {
            if (isActive())
            {
                // check if outside bounding box
                if (
                    Config.handPoint.X < 160 || 
                    Config.handPoint.X > 640 || 
                    Config.handPoint.Y < 0 || 
                    Config.handPoint.Y > 480
                    )
                {
                    detach();
                    return currentPoint();
                }

                return Config.handPoint;
            }

            return new Point(point.X, point.Y);
        }

        public Point textPoint()
        {
            return new Point(currentPoint().X - 10, currentPoint().Y + 10);
        }

        public bool isWithin(Point handPoint)
        {
            return Util.isWithin(handPoint, Config.handRadius, currentPoint(), Config.nodeRadius);
        }

        public bool isAnswer()
        {
            if (_answerTime == long.MaxValue)
                _answerTime = (long)(DateTime.UtcNow - Config.UnixEpoch).TotalMilliseconds;

            if (_answerTime + 500 < (long)(DateTime.UtcNow - Config.UnixEpoch).TotalMilliseconds)
            {
                _answerTime = long.MaxValue;
                return true;
            }

            return false;
        }
    }
}
