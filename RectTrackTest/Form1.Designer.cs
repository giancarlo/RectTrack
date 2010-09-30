namespace RectTrackTest
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tracker1 = new RectTrack.Tracker();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::RectTrackTest.Properties.Resources.me;
            this.pictureBox1.Location = new System.Drawing.Point(174, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(275, 260);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tracker1
            // 
            this.tracker1.BorderWidth = 3;
            this.tracker1.Control = this.pictureBox1;
            this.tracker1.GridSize = new System.Drawing.Size(8, 8);
            this.tracker1.HandleSize = 7;
            this.tracker1.Location = new System.Drawing.Point(171, 27);
            this.tracker1.Name = "tracker1";
            this.tracker1.OffSet = new System.Drawing.Point(0, 0);
            this.tracker1.Size = new System.Drawing.Size(281, 266);
            this.tracker1.SnapToGrid = false;
            this.tracker1.TabIndex = 1;
            this.tracker1.Text = "tracker1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 336);
            this.Controls.Add(this.tracker1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private RectTrack.Tracker tracker1;
    }
}