namespace DocToNne
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
            this.butConvert = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // butConvert
            // 
            this.butConvert.Location = new System.Drawing.Point(12, 12);
            this.butConvert.Name = "butConvert";
            this.butConvert.Size = new System.Drawing.Size(174, 23);
            this.butConvert.TabIndex = 0;
            this.butConvert.Text = "Convert to Nne";
            this.butConvert.UseVisualStyleBackColor = true;
            this.butConvert.Click += new System.EventHandler(this.ButConvert_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(198, 47);
            this.Controls.Add(this.butConvert);
            this.Name = "Form1";
            this.Text = "DocToNne";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button butConvert;
    }
}

