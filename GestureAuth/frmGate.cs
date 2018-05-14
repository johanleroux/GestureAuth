using System;
using Emgu.CV;
using System.Windows.Forms;
using System.Drawing;
using Emgu.CV.CvEnum;
using System.Timers;
using System.Collections.Generic;
using Emgu.CV.Structure;
using System.Globalization;
using System.Threading;

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

        private String _hiddenPassword;
        private String _enteredPassword;

        private List<Node> _inputNodes = new List<Node>();

        Random _r = new Random();

        public frmGate()
        {
            InitializeComponent();

            initFrameCounter();

            refreshAuthentication();

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

            determineAction();

            drawUI();

            captureFrame.Image = _frame;
        }

        private void initInputNodes()
        {
            _inputNodes.Clear();

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

        private void determineAction()
        {
            if (_enteredPassword.Length == _hiddenPassword.Length) return;

            Point handPoint = new Point
                (
                    Util.center(Config.handPoint.X, Config.handRadius),
                    Util.center(Config.handPoint.Y, Config.handRadius)
                );

            Point ansPoint = new Point
                (
                    Util.center(Config.ansPoint.X, Config.ansRadius),
                    Util.center(Config.ansPoint.Y, Config.ansRadius)
                );

            bool active = false;
            foreach (Node node in _inputNodes)
                if (node.isActive())
                    active = true;


            if (!active)
            {
                foreach (Node node in _inputNodes)
                {
                    if (!node.isActive())
                        if (node.isWithin(handPoint))
                            node.attach();
                        else
                            node.detach();
                }
            }

            if (Util.isWithin(handPoint, Config.handRadius, ansPoint, Config.ansRadius))
            {
                foreach (Node node in _inputNodes)
                    if (node.isActive())
                    {
                        if (node.isAnswer())
                        {
                            _enteredPassword += node.id.ToString();
                            node.detach();
                        }
                    }
            } else if (Util.isWithin(handPoint, Config.handRadius, new Point(190, 30), Config.nodeRadius))
            {
                foreach (Node node in _inputNodes)
                    if (node.isActive())
                        if (node.isAnswer())
                            node.detach();
            }
            else if (Util.isWithin(handPoint, Config.handRadius, new Point(610, 30), Config.nodeRadius))
            {
                initInputNodes();
            }
        }

        private void drawUI()
        {
            if(_hiddenPassword == _enteredPassword)
            {
                CvInvoke.Rectangle(_frame, new Rectangle(new Point(0, 0), new Size(640, 480)), new MCvScalar(0, 0, 0), -1);
                CvInvoke.PutText(_frame, "ACCESS GRANTED", new Point(160, 230), Config.fontFace, 2, Config.nodeSuccess, 2, LineType.AntiAlias);
                
                return;
            } else if (_hiddenPassword.Length == _enteredPassword.Length)
            {
                CvInvoke.Rectangle(_frame, new Rectangle(new Point(0, 0), new Size(640, 480)), new MCvScalar(0, 0, 0), -1);
                CvInvoke.PutText(_frame, "ACCESS DENIED", new Point(180, 230), Config.fontFace, 2, Config.nodeActive, 2, LineType.AntiAlias);

                return;
            }
       
            drawDiagnostics();

            CvInvoke.PutText(_frame, "Please enter", new Point(0, 30), Config.fontFace, 1, Config.drawingColor, 1, LineType.AntiAlias);
            CvInvoke.PutText(_frame, "hidden password", new Point(0, 45), Config.fontFace, 1, Config.drawingColor, 1, LineType.AntiAlias);
            CvInvoke.PutText(_frame, _hiddenPassword + " ;-)", new Point(0, 60), Config.fontFace, 1, Config.drawingColor, 1, LineType.AntiAlias);

            MCvScalar ansNodeColour = new MCvScalar();
            if (_enteredPassword.Length != _hiddenPassword.Length)
                ansNodeColour = Config.nodeDeactive;
            else if (_enteredPassword == _hiddenPassword)
                ansNodeColour = Config.nodeSuccess;
            else
                ansNodeColour = Config.nodeActive;

            for (int i = 0; i < _enteredPassword.Length; i++)
            {
                CvInvoke.Circle(_frame, new Point(i * 40 + 15, 100), Config.nodeRadius - 5, ansNodeColour, -1, LineType.AntiAlias);
                CvInvoke.PutText(_frame, _enteredPassword[i].ToString(), new Point(i * 40 + 5, 110), Config.fontFace, 2, Config.drawingColor, 1, LineType.AntiAlias);

            }

            CvInvoke.Circle(_frame, new Point(190, 30), Config.nodeRadius + 5, Config.nodeActive, -1, LineType.AntiAlias);
            CvInvoke.PutText(_frame, "T", new Point(180, 40), Config.fontFace, 2, Config.drawingColor, 1, LineType.AntiAlias);

            CvInvoke.Circle(_frame, new Point(610, 30), Config.nodeRadius + 5, Config.nodeSuccess, -1, LineType.AntiAlias);
            CvInvoke.PutText(_frame, "R", new Point(600, 40), Config.fontFace, 2, Config.drawingColor, 1, LineType.AntiAlias);

            foreach (Node node in _inputNodes)
            {
                CvInvoke.Circle(_frame, node.currentPoint(), Config.nodeRadius, node.getColour(), -1, LineType.AntiAlias);
                CvInvoke.PutText(_frame, node.id.ToString(), node.textPoint(), Config.fontFace, 2, Config.drawingColor, 1, LineType.AntiAlias);
            }

            CvInvoke.Circle(_frame, Config.ansPoint, Config.ansRadius, Config.nodeDeactive, 3, LineType.AntiAlias);

            Point handCenter = new Point(
                Config.handPoint.X,
                Config.handPoint.Y
            );

            CvInvoke.Circle(_frame, handCenter, Config.handRadius, Config.nodeActive, 3, LineType.AntiAlias);
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
            if (e.KeyCode == Keys.R) refreshAuthentication();
        }

        public void refreshAuthentication()
        {
            initInputNodes();

            _enteredPassword = "";

            _hiddenPassword = "";
            for (int i = 0; i < 4; i++)
                _hiddenPassword += _r.Next(0, 9);
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