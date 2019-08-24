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
            this.lstOptions = new System.Windows.Forms.ListBox();
            this.lstContains = new System.Windows.Forms.ListBox();
            this.txtContainsAdd = new System.Windows.Forms.TextBox();
            this.butContainsAdd = new System.Windows.Forms.Button();
            this.webContainer = new System.Windows.Forms.WebBrowser();
            this.txtContainerAdd = new System.Windows.Forms.TextBox();
            this.butContainerAdd = new System.Windows.Forms.Button();
            this.webBook = new System.Windows.Forms.WebBrowser();
            this.rtbInput = new System.Windows.Forms.RichTextBox();
            this.butStartChapter = new System.Windows.Forms.Button();
            this.lblNovelCount = new System.Windows.Forms.Label();
            this.lblParagraphCount = new System.Windows.Forms.Label();
            this.butEdit = new System.Windows.Forms.Button();
            this.numStart = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDocXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstOptions
            // 
            this.lstOptions.FormattingEnabled = true;
            this.lstOptions.Items.AddRange(new object[] {
            "Helpers",
            "Plot"});
            this.lstOptions.Location = new System.Drawing.Point(592, 30);
            this.lstOptions.Name = "lstOptions";
            this.lstOptions.Size = new System.Drawing.Size(216, 30);
            this.lstOptions.TabIndex = 2;
            this.lstOptions.SelectedIndexChanged += new System.EventHandler(this.lstOptions_SelectedIndexChanged);
            // 
            // lstContains
            // 
            this.lstContains.FormattingEnabled = true;
            this.lstContains.Location = new System.Drawing.Point(592, 92);
            this.lstContains.Name = "lstContains";
            this.lstContains.Size = new System.Drawing.Size(394, 95);
            this.lstContains.TabIndex = 3;
            this.lstContains.SelectedIndexChanged += new System.EventHandler(this.lstContains_SelectedIndexChanged);
            // 
            // txtContainsAdd
            // 
            this.txtContainsAdd.Location = new System.Drawing.Point(592, 66);
            this.txtContainsAdd.MaxLength = 10;
            this.txtContainsAdd.Name = "txtContainsAdd";
            this.txtContainsAdd.Size = new System.Drawing.Size(374, 20);
            this.txtContainsAdd.TabIndex = 4;
            // 
            // butContainsAdd
            // 
            this.butContainsAdd.Location = new System.Drawing.Point(972, 64);
            this.butContainsAdd.Name = "butContainsAdd";
            this.butContainsAdd.Size = new System.Drawing.Size(14, 23);
            this.butContainsAdd.TabIndex = 5;
            this.butContainsAdd.Text = "+";
            this.butContainsAdd.UseVisualStyleBackColor = true;
            this.butContainsAdd.Click += new System.EventHandler(this.butContainsAdd_Click);
            // 
            // webContainer
            // 
            this.webContainer.Location = new System.Drawing.Point(592, 193);
            this.webContainer.MinimumSize = new System.Drawing.Size(20, 20);
            this.webContainer.Name = "webContainer";
            this.webContainer.Size = new System.Drawing.Size(394, 329);
            this.webContainer.TabIndex = 6;
            this.webContainer.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webContainer_Navigating);
            // 
            // txtContainerAdd
            // 
            this.txtContainerAdd.Location = new System.Drawing.Point(592, 531);
            this.txtContainerAdd.Name = "txtContainerAdd";
            this.txtContainerAdd.Size = new System.Drawing.Size(374, 20);
            this.txtContainerAdd.TabIndex = 7;
            // 
            // butContainerAdd
            // 
            this.butContainerAdd.Location = new System.Drawing.Point(972, 528);
            this.butContainerAdd.Name = "butContainerAdd";
            this.butContainerAdd.Size = new System.Drawing.Size(14, 23);
            this.butContainerAdd.TabIndex = 8;
            this.butContainerAdd.Text = "+";
            this.butContainerAdd.UseVisualStyleBackColor = true;
            this.butContainerAdd.Click += new System.EventHandler(this.butContainerAdd_Click);
            // 
            // webBook
            // 
            this.webBook.Location = new System.Drawing.Point(12, 30);
            this.webBook.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBook.Name = "webBook";
            this.webBook.Size = new System.Drawing.Size(564, 521);
            this.webBook.TabIndex = 9;
            this.webBook.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBook_Navigating);
            // 
            // rtbInput
            // 
            this.rtbInput.Location = new System.Drawing.Point(12, 572);
            this.rtbInput.Name = "rtbInput";
            this.rtbInput.Size = new System.Drawing.Size(796, 97);
            this.rtbInput.TabIndex = 10;
            this.rtbInput.Text = "";
            this.rtbInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtbInput_KeyDown);
            // 
            // butStartChapter
            // 
            this.butStartChapter.Location = new System.Drawing.Point(822, 591);
            this.butStartChapter.Name = "butStartChapter";
            this.butStartChapter.Size = new System.Drawing.Size(79, 78);
            this.butStartChapter.TabIndex = 12;
            this.butStartChapter.Text = "Chapter Start";
            this.butStartChapter.UseVisualStyleBackColor = true;
            this.butStartChapter.Click += new System.EventHandler(this.butStartChapter_Click);
            // 
            // lblNovelCount
            // 
            this.lblNovelCount.AutoSize = true;
            this.lblNovelCount.Location = new System.Drawing.Point(814, 30);
            this.lblNovelCount.Name = "lblNovelCount";
            this.lblNovelCount.Size = new System.Drawing.Size(44, 13);
            this.lblNovelCount.TabIndex = 13;
            this.lblNovelCount.Text = "Words: ";
            // 
            // lblParagraphCount
            // 
            this.lblParagraphCount.AutoSize = true;
            this.lblParagraphCount.Location = new System.Drawing.Point(819, 573);
            this.lblParagraphCount.Name = "lblParagraphCount";
            this.lblParagraphCount.Size = new System.Drawing.Size(26, 13);
            this.lblParagraphCount.TabIndex = 14;
            this.lblParagraphCount.Text = "P: 0";
            // 
            // butEdit
            // 
            this.butEdit.Location = new System.Drawing.Point(907, 572);
            this.butEdit.Name = "butEdit";
            this.butEdit.Size = new System.Drawing.Size(79, 76);
            this.butEdit.TabIndex = 15;
            this.butEdit.Text = "Edit";
            this.butEdit.UseVisualStyleBackColor = true;
            this.butEdit.Click += new System.EventHandler(this.butEdit_Click);
            // 
            // numStart
            // 
            this.numStart.Location = new System.Drawing.Point(945, 651);
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
            this.label1.Location = new System.Drawing.Point(909, 653);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "start:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.exportDocXToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(994, 24);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createNewToolStripMenuItem,
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // createNewToolStripMenuItem
            // 
            this.createNewToolStripMenuItem.Name = "createNewToolStripMenuItem";
            this.createNewToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.createNewToolStripMenuItem.Text = "Create New";
            this.createNewToolStripMenuItem.Click += new System.EventHandler(this.CreateNewToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // exportDocXToolStripMenuItem
            // 
            this.exportDocXToolStripMenuItem.Name = "exportDocXToolStripMenuItem";
            this.exportDocXToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.exportDocXToolStripMenuItem.Text = "Export DocX";
            this.exportDocXToolStripMenuItem.Click += new System.EventHandler(this.ExportDocXToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 681);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numStart);
            this.Controls.Add(this.butEdit);
            this.Controls.Add(this.lblParagraphCount);
            this.Controls.Add(this.lblNovelCount);
            this.Controls.Add(this.butStartChapter);
            this.Controls.Add(this.rtbInput);
            this.Controls.Add(this.webBook);
            this.Controls.Add(this.butContainerAdd);
            this.Controls.Add(this.txtContainerAdd);
            this.Controls.Add(this.webContainer);
            this.Controls.Add(this.butContainsAdd);
            this.Controls.Add(this.txtContainsAdd);
            this.Controls.Add(this.lstContains);
            this.Controls.Add(this.lstOptions);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(1010, 720);
            this.MinimumSize = new System.Drawing.Size(1010, 720);
            this.Name = "FormMain";
            this.Text = "NaNoE";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lstOptions;
        private System.Windows.Forms.ListBox lstContains;
        private System.Windows.Forms.TextBox txtContainsAdd;
        private System.Windows.Forms.Button butContainsAdd;
        private System.Windows.Forms.WebBrowser webContainer;
        private System.Windows.Forms.TextBox txtContainerAdd;
        private System.Windows.Forms.Button butContainerAdd;
        private System.Windows.Forms.WebBrowser webBook;
        private System.Windows.Forms.RichTextBox rtbInput;
        private System.Windows.Forms.Button butStartChapter;
        private System.Windows.Forms.Label lblNovelCount;
        private System.Windows.Forms.Label lblParagraphCount;
        private System.Windows.Forms.Button butEdit;
        private System.Windows.Forms.NumericUpDown numStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportDocXToolStripMenuItem;
    }
}

