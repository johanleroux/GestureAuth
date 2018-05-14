namespace GestureAuth
{
    partial class frmGate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.captureFrame = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.captureFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // captureFrame
            // 
            this.captureFrame.BackColor = System.Drawing.SystemColors.Control;
            this.captureFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captureFrame.Location = new System.Drawing.Point(0, 0);
            this.captureFrame.Margin = new System.Windows.Forms.Padding(0);
            this.captureFrame.Name = "captureFrame";
            this.captureFrame.Size = new System.Drawing.Size(640, 480);
            this.captureFrame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.captureFrame.TabIndex = 2;
            this.captureFrame.TabStop = false;
            // 
            // frmGate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.captureFrame);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(656, 519);
            this.Name = "frmGate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gesture Authentication - Gate";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmGate_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.captureFrame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox captureFrame;
    }
}

