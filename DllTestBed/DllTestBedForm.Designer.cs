namespace DllTestBed
{
    partial class DllTestBedForm
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
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonRun2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonRun
            // 
            this.buttonRun.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonRun.Location = new System.Drawing.Point(12, 12);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(127, 113);
            this.buttonRun.TabIndex = 3;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = false;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // buttonRun2
            // 
            this.buttonRun2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonRun2.Location = new System.Drawing.Point(155, 12);
            this.buttonRun2.Name = "buttonRun2";
            this.buttonRun2.Size = new System.Drawing.Size(127, 113);
            this.buttonRun2.TabIndex = 4;
            this.buttonRun2.Text = "Run2";
            this.buttonRun2.UseVisualStyleBackColor = false;
            this.buttonRun2.Click += new System.EventHandler(this.buttonRun2_Click);
            // 
            // DllTestBedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 137);
            this.Controls.Add(this.buttonRun2);
            this.Controls.Add(this.buttonRun);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DllTestBedForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DllTestBed";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonRun2;
    }
}

