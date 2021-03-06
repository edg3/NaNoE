﻿namespace NaNoE
{
    partial class FormEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEdit));
            this.lstProblems = new System.Windows.Forms.ListBox();
            this.rtbParagraph = new System.Windows.Forms.RichTextBox();
            this.butRefresh = new System.Windows.Forms.Button();
            this.butDone = new System.Windows.Forms.Button();
            this.butStopForNow = new System.Windows.Forms.Button();
            this.lblPosition = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstProblems
            // 
            this.lstProblems.FormattingEnabled = true;
            this.lstProblems.Location = new System.Drawing.Point(12, 38);
            this.lstProblems.Name = "lstProblems";
            this.lstProblems.Size = new System.Drawing.Size(346, 186);
            this.lstProblems.TabIndex = 0;
            this.lstProblems.DoubleClick += new System.EventHandler(this.LstProblems_DoubleClick);
            // 
            // rtbParagraph
            // 
            this.rtbParagraph.Location = new System.Drawing.Point(364, 38);
            this.rtbParagraph.Name = "rtbParagraph";
            this.rtbParagraph.Size = new System.Drawing.Size(357, 186);
            this.rtbParagraph.TabIndex = 1;
            this.rtbParagraph.Text = "";
            this.rtbParagraph.SelectionChanged += new System.EventHandler(this.RtbParagraph_SelectionChanged);
            this.rtbParagraph.TextChanged += new System.EventHandler(this.rtbParagraph_TextChanged);
            // 
            // butRefresh
            // 
            this.butRefresh.Location = new System.Drawing.Point(12, 225);
            this.butRefresh.Name = "butRefresh";
            this.butRefresh.Size = new System.Drawing.Size(223, 23);
            this.butRefresh.TabIndex = 2;
            this.butRefresh.Text = "Refresh Problems Above";
            this.butRefresh.UseVisualStyleBackColor = true;
            this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
            // 
            // butDone
            // 
            this.butDone.Location = new System.Drawing.Point(646, 230);
            this.butDone.Name = "butDone";
            this.butDone.Size = new System.Drawing.Size(75, 23);
            this.butDone.TabIndex = 3;
            this.butDone.Text = "Done";
            this.butDone.UseVisualStyleBackColor = true;
            this.butDone.Click += new System.EventHandler(this.butDone_Click);
            // 
            // butStopForNow
            // 
            this.butStopForNow.Location = new System.Drawing.Point(241, 225);
            this.butStopForNow.Name = "butStopForNow";
            this.butStopForNow.Size = new System.Drawing.Size(85, 23);
            this.butStopForNow.TabIndex = 4;
            this.butStopForNow.Text = "Stop For Now";
            this.butStopForNow.UseVisualStyleBackColor = true;
            this.butStopForNow.Click += new System.EventHandler(this.butStopForNow_Click);
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Location = new System.Drawing.Point(372, 235);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(50, 13);
            this.lblPosition.TabIndex = 5;
            this.lblPosition.Text = "Position: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Double Click to see more:";
            // 
            // FormEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 260);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblPosition);
            this.Controls.Add(this.butStopForNow);
            this.Controls.Add(this.butDone);
            this.Controls.Add(this.butRefresh);
            this.Controls.Add(this.rtbParagraph);
            this.Controls.Add(this.lstProblems);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormEdit";
            this.Text = "Edit Paragraph";
            this.Load += new System.EventHandler(this.FormEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstProblems;
        private System.Windows.Forms.RichTextBox rtbParagraph;
        private System.Windows.Forms.Button butRefresh;
        private System.Windows.Forms.Button butDone;
        private System.Windows.Forms.Button butStopForNow;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Label label1;
    }
}