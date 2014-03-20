namespace SpielNaoKinect.Kinect
{
    partial class WindowAngles
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
            this.XElbowRight = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // XElbowRight
            // 
            this.XElbowRight.AutoSize = true;
            this.XElbowRight.Location = new System.Drawing.Point(27, 20);
            this.XElbowRight.Name = "XElbowRight";
            this.XElbowRight.Size = new System.Drawing.Size(35, 13);
            this.XElbowRight.TabIndex = 0;
            this.XElbowRight.Text = "label1";
            // 
            // WindowAngles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 227);
            this.Controls.Add(this.XElbowRight);
            this.Name = "WindowAngles";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label XElbowRight;



    }
}