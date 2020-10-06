namespace update
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.line1 = new System.Windows.Forms.Label();
            this.line2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 79);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(384, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // line1
            // 
            this.line1.AutoSize = true;
            this.line1.Location = new System.Drawing.Point(12, 18);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(31, 12);
            this.line1.TabIndex = 1;
            this.line1.Text = "line1";
            // 
            // line2
            // 
            this.line2.AutoSize = true;
            this.line2.Location = new System.Drawing.Point(12, 47);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(31, 12);
            this.line2.TabIndex = 2;
            this.line2.Text = "line2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 121);
            this.ControlBox = false;
            this.Controls.Add(this.line2);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "Update";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label line1;
        private System.Windows.Forms.Label line2;
    }
}

