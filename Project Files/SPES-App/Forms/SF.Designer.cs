namespace SPES_App.Forms
{
    partial class SelectSystemFunction
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CSF = new System.Windows.Forms.RadioButton();
            this.PSF = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CSF);
            this.groupBox1.Controls.Add(this.PSF);
            this.groupBox1.Location = new System.Drawing.Point(32, 31);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(388, 186);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // CSF
            // 
            this.CSF.AutoSize = true;
            this.CSF.Location = new System.Drawing.Point(10, 66);
            this.CSF.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CSF.Name = "CSF";
            this.CSF.Size = new System.Drawing.Size(248, 24);
            this.CSF.TabIndex = 1;
            this.CSF.TabStop = true;
            this.CSF.Text = "Collaborative System Function";
            this.CSF.UseVisualStyleBackColor = true;
            this.CSF.CheckedChanged += new System.EventHandler(this.CSF_CheckedChanged);
            // 
            // PSF
            // 
            this.PSF.AutoSize = true;
            this.PSF.Location = new System.Drawing.Point(9, 29);
            this.PSF.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PSF.Name = "PSF";
            this.PSF.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PSF.Size = new System.Drawing.Size(190, 24);
            this.PSF.TabIndex = 0;
            this.PSF.TabStop = true;
            this.PSF.Text = "Pure System Function";
            this.PSF.UseVisualStyleBackColor = true;
            this.PSF.CheckedChanged += new System.EventHandler(this.PSF_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(336, 242);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 34);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SelectSystemFunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 286);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SelectSystemFunction";
            this.Text = "Select Type of System Function";
            this.Load += new System.EventHandler(this.SelectSystemFunction_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton CSF;
        private System.Windows.Forms.RadioButton PSF;
        private System.Windows.Forms.Button button1;
    }
}