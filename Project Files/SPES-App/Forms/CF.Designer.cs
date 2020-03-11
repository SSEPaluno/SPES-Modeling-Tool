namespace SPES_App.Forms
{
    partial class ContextFunction
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
            this.LCF = new System.Windows.Forms.RadioButton();
            this.CCF = new System.Windows.Forms.RadioButton();
            this.PCF = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LCF);
            this.groupBox1.Controls.Add(this.CCF);
            this.groupBox1.Controls.Add(this.PCF);
            this.groupBox1.Location = new System.Drawing.Point(18, 18);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(388, 186);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // LCF
            // 
            this.LCF.AutoSize = true;
            this.LCF.Location = new System.Drawing.Point(10, 103);
            this.LCF.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LCF.Name = "LCF";
            this.LCF.Size = new System.Drawing.Size(197, 24);
            this.LCF.TabIndex = 2;
            this.LCF.TabStop = true;
            this.LCF.Text = "Local Context Function";
            this.LCF.UseVisualStyleBackColor = true;
            this.LCF.CheckedChanged += new System.EventHandler(this.LCF_CheckedChanged);
            // 
            // CCF
            // 
            this.CCF.AutoSize = true;
            this.CCF.Location = new System.Drawing.Point(10, 66);
            this.CCF.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CCF.Name = "CCF";
            this.CCF.Size = new System.Drawing.Size(250, 24);
            this.CCF.TabIndex = 1;
            this.CCF.TabStop = true;
            this.CCF.Text = "Collaborative Context Function";
            this.CCF.UseVisualStyleBackColor = true;
            this.CCF.CheckedChanged += new System.EventHandler(this.CCF_CheckedChanged);
            // 
            // PCF
            // 
            this.PCF.AutoSize = true;
            this.PCF.Location = new System.Drawing.Point(9, 29);
            this.PCF.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PCF.Name = "PCF";
            this.PCF.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PCF.Size = new System.Drawing.Size(192, 24);
            this.PCF.TabIndex = 0;
            this.PCF.TabStop = true;
            this.PCF.Text = "Pure Context Function";
            this.PCF.UseVisualStyleBackColor = true;
            this.PCF.CheckedChanged += new System.EventHandler(this.PCF_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(322, 229);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 34);
            this.button1.TabIndex = 3;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ContextFunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 283);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ContextFunction";
            this.Text = "Select Type of Context Function";
            this.Load += new System.EventHandler(this.ContextFunction_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton CCF;
        private System.Windows.Forms.RadioButton PCF;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton LCF;
    }
}