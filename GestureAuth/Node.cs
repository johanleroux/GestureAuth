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
            return this._active;
        }

        public void activate()
        {
            _active = true;
        }

        public void deactivate()
        {
            _active = false;
        }

        public Point currentPoint()
        {
            return new Point(point.X, point.Y);
        }

        public Point textPoint()
        {
            return new Point(currentPoint().X - 10, currentPoint().Y + 10);
        }
    }
}
