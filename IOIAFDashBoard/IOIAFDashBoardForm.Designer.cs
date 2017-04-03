namespace IOIAFDashBoard
{
    partial class IOIAFDashBoard
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
            this.AcquireOIMSData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AcquireOIMSData
            // 
            this.AcquireOIMSData.Location = new System.Drawing.Point(33, 217);
            this.AcquireOIMSData.Name = "AcquireOIMSData";
            this.AcquireOIMSData.Size = new System.Drawing.Size(157, 23);
            this.AcquireOIMSData.TabIndex = 0;
            this.AcquireOIMSData.Text = "AcquireOIMSData";
            this.AcquireOIMSData.UseVisualStyleBackColor = true;
            this.AcquireOIMSData.Click += new System.EventHandler(this.AcquireOIMSData_Click);
            // 
            // IOIAFDashBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 261);
            this.Controls.Add(this.AcquireOIMSData);
            this.Name = "IOIAFDashBoard";
            this.Text = "IOIAFDashBoard";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AcquireOIMSData;
    }
}

