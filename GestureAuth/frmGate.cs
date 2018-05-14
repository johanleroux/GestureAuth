using System;
using Emgu.CV;
using System.Windows.Forms;
using System.Drawing;
using Emgu.CV.CvEnum;
using System.Timers;
using System.Collections.Generic;
using Emgu.CV.Structure;

namespace GestureAuth
{
    public partial class frmGate : Form
    {
        private VideoCapture _camera = null;
        private Mat _frame;

        private Preprocess _preprocess;
        private FeatureExtraction _featureExtraction;

        frmDebug _debugForm;

        private int _frameCounter;
        private int _frameRate;

        private List<Node> _inputNodes = new List<Node>();

        Random _r = new Random();

        public frmGate()
        {
            InitializeComponent();

            initFrameCounter();

            initInputNodes();

            try
            {
                _camera = new VideoCapture();

                _camera.ImageGrabbed += ProcessFrame;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }

            _frame = new Mat();
            _camera.FlipHorizontal = true;
            _camera.Start();

            captureFrame.Enabled = false;

            switchFullscreen();
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (_camera == null) return;
            if (_camera.Ptr == IntPtr.Zero) return;

            _frameCounter++;

            _camera.Retrieve(_frame, 0);

            _preprocess = new Preprocess(_frame);
            _frame = _preprocess.process();

            _featureExtraction = new FeatureExtraction(_frame);
            _frame = _featureExtraction.process();

            Image<Rgb, Byte> tmp = _frame.ToImage<Rgb, Byte>();
            _frame = tmp.Mat;

            drawUI();

            captureFrame.Image = _frame;
        }

        private void initInputNodes()
        {
            List<int> inputs = new List<int>();
            for (int i = 0; i < 10; i++)
                inputs.Add(i);

            List<Point> points = new List<Point>();
            points.Add(new Point(230, 110));
            points.Add(new Point(330, 30));
            points.Add(new Point(470, 30));
            points.Add(new Point(570, 110));
            points.Add(new Point(190, 240));
            points.Add(new Point(610, 240));
            points.Add(new Point(230, 370));
            points.Add(new Point(330, 450));
            points.Add(new Point(470, 450));
            points.Add(new Point(570, 370));

            while (inputs.Count > 0)
            {
                int index = _r.Next(0, inputs.Count - 1);
                int tmp = inputs[index];
                _inputNodes.Add(new Node(tmp, points[0]));
                inputs.Remove(tmp);
                points.RemoveAt(0);
            }
        }

        private void drawUI()
        {
            drawDiagnostics();

            foreach (Node node in _inputNodes)
            {
                CvInvoke.Circle(_frame, node.currentPoint(), 20, node.getColour(), -1, LineType.AntiAlias);
                CvInvoke.PutText(_frame, node.id.ToString(), node.textPoint(), Config.fontFace, 2, Config.drawingColor, 1, LineType.AntiAlias);
            }

            CvInvoke.Circle(_frame, new Point(400, 240), 50, Config.nodeDeactive, 3, LineType.AntiAlias);

            Point handCenter = new Point(
                _featureExtraction.handPoint.X,
                _featureExtraction.handPoint.Y
            );

            CvInvoke.Circle(_frame, handCenter, 50, Config.nodeActive, 3);
        }

        private void drawDiagnostics()
        {
            CvInvoke.PutText(_frame, _frameRate + "FPS", new Point(0, 10), Config.fontFace, 0.8, Config.drawingColor);

            CvInvoke.Rectangle(_frame, new Rectangle(new Point(160, 0), new Size(480, 480)), Config.drawingColor, 1);
        }

        private void frmGate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10) switchDebug();
            if (e.KeyCode == Keys.F11) switchFullscreen();
        }

        private void switchDebug()
        {
            Config.debug = !Config.debug;

            if (Config.debug)
            {
                _debugForm = new frmDebug();
                _debugForm.Show();
                this.Focus();
            } else
            {
                _debugForm.Hide();
                _debugForm.Dispose();
                _debugForm = null;
            }
        }

        private void switchFullscreen()
        {
            if (Config.fullscreen)
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Bounds = Screen.PrimaryScreen.Bounds;
                captureFrame.Dock = DockStyle.Fill;
            }
            else
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;

                int height = 500;
                int width = Convert.ToInt32(Math.Floor(height * 640f / 480f)) - 15;
                this.Size = new Size(width, height);
            }

            Config.fullscreen = !Config.fullscreen;
        }

        public void initFrameCounter()
        {
            System.Timers.Timer frameTimer = new System.Timers.Timer();
            frameTimer.Elapsed += new ElapsedEventHandler(frameCounter);
            frameTimer.Interval = 1000;
            frameTimer.Enabled = true;
        }

        private void frameCounter(object source, ElapsedEventArgs e)
        {
            _frameRate = _frameCounter;
            _frameCounter = 0;
        }
    }
}