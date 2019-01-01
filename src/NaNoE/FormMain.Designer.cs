namespace NaNoE
{
    partial class FormMain
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
            this.butLoad = new System.Windows.Forms.Button();
            this.butSave = new System.Windows.Forms.Button();
            this.lstOptions = new System.Windows.Forms.ListBox();
            this.lstContains = new System.Windows.Forms.ListBox();
            this.txtContainsAdd = new System.Windows.Forms.TextBox();
            this.butContainsAdd = new System.Windows.Forms.Button();
            this.webContainer = new System.Windows.Forms.WebBrowser();
            this.txtContainerAdd = new System.Windows.Forms.TextBox();
            this.butContainerAdd = new System.Windows.Forms.Button();
            this.webBook = new System.Windows.Forms.WebBrowser();
            this.rtbInput = new System.Windows.Forms.RichTextBox();
            this.butExport = new System.Windows.Forms.Button();
            this.butStartChapter = new System.Windows.Forms.Button();
            this.lblNovelCount = new System.Windows.Forms.Label();
            this.lblParagraphCount = new System.Windows.Forms.Label();
            this.butEdit = new System.Windows.Forms.Button();
            this.numStart = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).BeginInit();
            this.SuspendLayout();
            // 
            // butLoad
            // 
            this.butLoad.Location = new System.Drawing.Point(1, 1);
            this.butLoad.Name = "butLoad";
            this.butLoad.Size = new System.Drawing.Size(75, 23);
            this.butLoad.TabIndex = 0;
            this.butLoad.Text = "Load";
            this.butLoad.UseVisualStyleBackColor = true;
            this.butLoad.Click += new System.EventHandler(this.butLoad_Click);
            // 
            // butSave
            // 
            this.butSave.Location = new System.Drawing.Point(82, 1);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(75, 23);
            this.butSave.TabIndex = 1;
            this.butSave.Text = "Save";
            this.butSave.UseVisualStyleBackColor = true;
            this.butSave.Click += new System.EventHandler(this.butSave_Click);
            // 
            // lstOptions
            // 
            this.lstOptions.FormattingEnabled = true;
            this.lstOptions.Items.AddRange(new object[] {
            "Helpers",
            "Plot"});
            this.lstOptions.Location = new System.Drawing.Point(583, 1);
            this.lstOptions.Name = "lstOptions";
            this.lstOptions.Size = new System.Drawing.Size(216, 95);
            this.lstOptions.TabIndex = 2;
            this.lstOptions.SelectedIndexChanged += new System.EventHandler(this.lstOptions_SelectedIndexChanged);
            // 
            // lstContains
            // 
            this.lstContains.FormattingEnabled = true;
            this.lstContains.Location = new System.Drawing.Point(583, 129);
            this.lstContains.Name = "lstContains";
            this.lstContains.Size = new System.Drawing.Size(216, 95);
            this.lstContains.TabIndex = 3;
            this.lstContains.SelectedIndexChanged += new System.EventHandler(this.lstContains_SelectedIndexChanged);
            // 
            // txtContainsAdd
            // 
            this.txtContainsAdd.Location = new System.Drawing.Point(583, 102);
            this.txtContainsAdd.MaxLength = 10;
            this.txtContainsAdd.Name = "txtContainsAdd";
            this.txtContainsAdd.Size = new System.Drawing.Size(196, 20);
            this.txtContainsAdd.TabIndex = 4;
            // 
            // butContainsAdd
            // 
            this.butContainsAdd.Location = new System.Drawing.Point(785, 100);
            this.butContainsAdd.Name = "butContainsAdd";
            this.butContainsAdd.Size = new System.Drawing.Size(14, 23);
            this.butContainsAdd.TabIndex = 5;
            this.butContainsAdd.Text = "+";
            this.butContainsAdd.UseVisualStyleBackColor = true;
            this.butContainsAdd.Click += new System.EventHandler(this.butContainsAdd_Click);
            // 
            // webContainer
            // 
            this.webContainer.Location = new System.Drawing.Point(583, 230);
            this.webContainer.MinimumSize = new System.Drawing.Size(20, 20);
            this.webContainer.Name = "webContainer";
            this.webContainer.Size = new System.Drawing.Size(216, 182);
            this.webContainer.TabIndex = 6;
            // 
            // txtContainerAdd
            // 
            this.txtContainerAdd.Location = new System.Drawing.Point(583, 418);
            this.txtContainerAdd.Name = "txtContainerAdd";
            this.txtContainerAdd.Size = new System.Drawing.Size(196, 20);
            this.txtContainerAdd.TabIndex = 7;
            // 
            // butContainerAdd
            // 
            this.butContainerAdd.Location = new System.Drawing.Point(785, 416);
            this.butContainerAdd.Name = "butContainerAdd";
            this.butContainerAdd.Size = new System.Drawing.Size(14, 23);
            this.butContainerAdd.TabIndex = 8;
            this.butContainerAdd.Text = "+";
            this.butContainerAdd.UseVisualStyleBackColor = true;
            this.butContainerAdd.Click += new System.EventHandler(this.butContainerAdd_Click);
            // 
            // webBook
            // 
            this.webBook.Location = new System.Drawing.Point(1, 30);
            this.webBook.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBook.Name = "webBook";
            this.webBook.Size = new System.Drawing.Size(576, 306);
            this.webBook.TabIndex = 9;
            // 
            // rtbInput
            // 
            this.rtbInput.Location = new System.Drawing.Point(1, 342);
            this.rtbInput.Name = "rtbInput";
            this.rtbInput.Size = new System.Drawing.Size(491, 97);
            this.rtbInput.TabIndex = 10;
            this.rtbInput.Text = "";
            this.rtbInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtbInput_KeyDown);
            // 
            // butExport
            // 
            this.butExport.Location = new System.Drawing.Point(502, 1);
            this.butExport.Name = "butExport";
            this.butExport.Size = new System.Drawing.Size(75, 23);
            this.butExport.TabIndex = 11;
            this.butExport.Text = "Export";
            this.butExport.UseVisualStyleBackColor = true;
            this.butExport.Click += new System.EventHandler(this.butExport_Click);
            // 
            // butStartChapter
            // 
            this.butStartChapter.Location = new System.Drawing.Point(498, 415);
            this.butStartChapter.Name = "butStartChapter";
            this.butStartChapter.Size = new System.Drawing.Size(79, 23);
            this.butStartChapter.TabIndex = 12;
            this.butStartChapter.Text = "Chapter Start";
            this.butStartChapter.UseVisualStyleBackColor = true;
            this.butStartChapter.Click += new System.EventHandler(this.butStartChapter_Click);
            // 
            // lblNovelCount
            // 
            this.lblNovelCount.AutoSize = true;
            this.lblNovelCount.Location = new System.Drawing.Point(352, 11);
            this.lblNovelCount.Name = "lblNovelCount";
            this.lblNovelCount.Size = new System.Drawing.Size(44, 13);
            this.lblNovelCount.TabIndex = 13;
            this.lblNovelCount.Text = "Words: ";
            // 
            // lblParagraphCount
            // 
            this.lblParagraphCount.AutoSize = true;
            this.lblParagraphCount.Location = new System.Drawing.Point(499, 345);
            this.lblParagraphCount.Name = "lblParagraphCount";
            this.lblParagraphCount.Size = new System.Drawing.Size(26, 13);
            this.lblParagraphCount.TabIndex = 14;
            this.lblParagraphCount.Text = "P: 0";
            // 
            // butEdit
            // 
            this.butEdit.Location = new System.Drawing.Point(271, 6);
            this.butEdit.Name = "butEdit";
            this.butEdit.Size = new System.Drawing.Size(75, 23);
            this.butEdit.TabIndex = 15;
            this.butEdit.Text = "Edit";
            this.butEdit.UseVisualStyleBackColor = true;
            this.butEdit.Click += new System.EventHandler(this.butEdit_Click);
            // 
            // numStart
            // 
            this.numStart.Location = new System.Drawing.Point(224, 9);
            this.numStart.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numStart.Name = "numStart";
            this.numStart.Size = new System.Drawing.Size(41, 20);
            this.numStart.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(194, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "start:";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numStart);
            this.Controls.Add(this.butEdit);
            this.Controls.Add(this.lblParagraphCount);
            this.Controls.Add(this.lblNovelCount);
            this.Controls.Add(this.butStartChapter);
            this.Controls.Add(this.butExport);
            this.Controls.Add(this.rtbInput);
            this.Controls.Add(this.webBook);
            this.Controls.Add(this.butContainerAdd);
            this.Controls.Add(this.txtContainerAdd);
            this.Controls.Add(this.webContainer);
            this.Controls.Add(this.butContainsAdd);
            this.Controls.Add(this.txtContainsAdd);
            this.Controls.Add(this.lstContains);
            this.Controls.Add(this.lstOptions);
            this.Controls.Add(this.butSave);
            this.Controls.Add(this.butLoad);
            this.MaximumSize = new System.Drawing.Size(816, 489);
            this.MinimumSize = new System.Drawing.Size(816, 489);
            this.Name = "FormMain";
            this.Text = "NaNoE";
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butLoad;
        private System.Windows.Forms.Button butSave;
        private System.Windows.Forms.ListBox lstOptions;
        private System.Windows.Forms.ListBox lstContains;
        private System.Windows.Forms.TextBox txtContainsAdd;
        private System.Windows.Forms.Button butContainsAdd;
        private System.Windows.Forms.WebBrowser webContainer;
        private System.Windows.Forms.TextBox txtContainerAdd;
        private System.Windows.Forms.Button butContainerAdd;
        private System.Windows.Forms.WebBrowser webBook;
        private System.Windows.Forms.RichTextBox rtbInput;
        private System.Windows.Forms.Button butExport;
        private System.Windows.Forms.Button butStartChapter;
        private System.Windows.Forms.Label lblNovelCount;
        private System.Windows.Forms.Label lblParagraphCount;
        private System.Windows.Forms.Button butEdit;
        private System.Windows.Forms.NumericUpDown numStart;
        private System.Windows.Forms.Label label1;
    }
}

