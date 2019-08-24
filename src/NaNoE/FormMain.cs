using NaNoE.Objective;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Xceed.Words.NET;

/// <summary>
/// NaNoE - An attempted custom NaNoWriMo novel writing application
///   -=- There are tons of them already available, I just also needed to keep my projects personal
///   -=- Create by: Ernest "edg3" Loveland
/// 
/// Features that still need to be implemented:
///   -=- Editing helpers
///   -=- remind user to save a pre-edit copy
///   -=- Delete button for lines added in view for each item helpers/plot
///   -=- Potentially:
///       -> Chapter numbers in 'WebShowNovel' since we might not do more than 1 paragraph for some reason?
/// </summary>
namespace NaNoE
{
    public partial class FormMain : Form
    {
        static string BGStyle = "<style type =\"text/css\">" 
                              + "body { background-color: #CDCDCD; font-size: 10pt; } "
                              + "div { background-color: #ccebff; padding: 5px; border-color: black; border-style: solid; border-width: 1px; } "
                              + "a, a:link, a:visited, a:hover, a:active { color: red; } "
                              + "li { padding: 1px; } "
                              + "</style>";

        public FormMain()
        {
            InitializeComponent();

            ClearWeb();
            WebShowNovel();

            NaNoEdit.Init();
        }

        /// <summary>
        /// Stored Novel Data
        /// </summary>
        Dictionary<string, List<string>> _helpers
        {
            get
            {
                Dictionary<string, List<string>> ans = new Dictionary<string, List<string>>();
                var helpers = ObjectiveDB.RunCMD("SELECT * FROM helpers;");
                if (helpers != null)
                {
                    if (helpers.Read())
                    {
                        do
                        {
                            var id = helpers.GetInt32(0);
                            var name = helpers.GetString(1);

                            ans.Add(name, new List<string>());
                            GetHelpersNotes(id, ans[name]);
                        }
                        while (helpers.Read());
                    }
                }
                return ans;
            }
        }

        private void GetHelpersNotes(int id, List<string> notes)
        {
            var noteconnections = ObjectiveDB.RunCMD("SELECT * FROM helpersjoint WHERE helperid = " + id.ToString() + ";");
            if (noteconnections != null)
            {
                if (noteconnections.Read())
                {
                    do
                    {
                        var note = ObjectiveDB.RunCMD("SELECT * FROM notes WHERE id = " + noteconnections.GetInt32(2));
                        note.Read();
                        notes.Add(note.GetString(1));
                    }
                    while (noteconnections.Read());
                }
            }
        }

        Dictionary<string, List<string>> _plot
        {
            get
            {
                Dictionary<string, List<string>> ans = new Dictionary<string, List<string>>();
                var plots = ObjectiveDB.RunCMD("SELECT * FROM plots;");
                if (plots != null)
                {
                    if (plots.Read())
                    {
                        do
                        {
                            var id = plots.GetInt32(0);
                            var name = plots.GetString(1);

                            ans.Add(name, new List<string>());
                            GetPlotNotes(id, ans[name]);
                        }
                        while (plots.Read());
                    }
                }
                return ans;
            }
        }

        private void GetPlotNotes(int id, List<string> list)
        {
            var noteconnections = ObjectiveDB.RunCMD("SELECT * FROM plotsjoint WHERE plotid = " + id.ToString() + ";");
            if (noteconnections != null)
            {
                if (noteconnections.Read())
                {
                    do
                    {
                        var note = ObjectiveDB.RunCMD("SELECT * FROM notes WHERE id = " + noteconnections.GetInt32(2));
                        note.Read();
                        list.Add(note.GetString(1));
                    }
                    while (noteconnections.Read());
                }
            }
        }

        List<string> _novel
        {
            get
            {
                List<string> ans = new List<string>();
                var novelparagraphs = ObjectiveDB.RunCMD("SELECT * FROM paragraphs ORDER BY id DESC LIMIT 5;");
                if (novelparagraphs != null)
                {
                    if (novelparagraphs.Read())
                    {
                        do
                        {
                            ans.Add(novelparagraphs.GetString(1));
                        }
                        while (novelparagraphs.Read());
                    }
                }
                ans.Reverse();
                return ans;
            }
        }
        
        /// <summary>
        /// Interface Refreshing Code
        /// </summary>
        // Generates the last 5 paragraphs we inputted
        private void WebShowNovel()
        {
            // Note: this is flawed logic, it might take long with a bigger DB
            ObjectiveDB.DBCount();

            List<string> lines = new List<string>();

            lblParagraphCount.Text = "P: " + ObjectiveDB.CountParagraphs().ToString();
            
            if (_novel.Count > 5)
            {
                for (int i = -5; i < 0; i++)
                {
                    lines.Add(_novel[i + _novel.Count]);
                }
            }
            else
            {
                for (int i = 0; i < _novel.Count; i++)
                {
                    lines.Add(_novel[i]);
                }
            }

            string midPoint = "";
            for (int i = 0; i < lines.Count; i++)
            {
                switch (lines[i])
                {
                    case "[chapter]": midPoint += "<hr /><div><b>" + /*ObjectiveDB..ToString() +*/ "</b> [ <i><a href=\"]" + (_novel.Count - 5 + i).ToString() + "\">Del</a></i> ]</div><br /><br />"; break;
                    // TODO: fix bug on next line for how it works - if you start a novel it doesnt fit logic for the first few lines
                    //        - it starts on a negative number
                    default: midPoint += "&nbsp;<i>" + ((_novel.Count < 5) ? i + 1 : _novel.Count - 4 + i).ToString() + "</i>&nbsp;&nbsp;<div>" + lines[i] + "&nbsp;&nbsp[ <i><a href=\"[" + (_novel.Count - 5 + i).ToString() + "\">Edit</a>,&nbsp;<a href=\"]" + (_novel.Count - 5 + i).ToString() + "\">Del</a></i> ]</div><br />"; break;
                }
            }

            webBook.DocumentText = BGStyle + midPoint;

            lblNovelCount.Text = "Words: " + ObjectiveDB.WordCount;
        }

        // Clears the view of data points we added to the object - if empty there is no data
        private void ClearWeb()
        {
            webContainer.DocumentText = BGStyle + "<div style=\"color: grey; font-size: 10pt;\"><i>Please select what you want to see.</i></div>";
        }

        // Generate view of data points we added to an object
        private void GenerateHTML(List<string> items)
        {
            string ans = BGStyle + "<ul>";

            for (int i = 0; i < items.Count; i++)
            {
                ans += "<li><div>" + items[i] + " [ <i><a href=\"]"+ i.ToString() +"\">Del</a></i> ]</div></li>";
            }

            ans += "</ul>";

            webContainer.DocumentText = ans;
        }

        // Clear contains
        // TODO: I cant remember the 'RemoveAll' sort of use
        private void ClearContains()
        {
            while (lstContains.Items.Count > 0)
                lstContains.Items.RemoveAt(0);
        }

        // Update the count of the novel
        // Note: consider this can keep numbers and symbols as words
        public void UpdateNovelCount()
        {
            Thread t = new Thread(new ThreadStart(UpdateNovelCountThread));
            t.Start();
        }

        private void UpdateNovelCountThread()
        {
            ObjectiveDB.DBCount();
        }

        // Update the count within the paragraph
        // TODO: This may need better writing
        public void UpdateParagraphCount()
        {
            lblParagraphCount.Text = "P: " + ObjectiveDB.CountParagraphs();
        }

        /// <summary>
        /// Form Interaction
        /// </summary>
        // Swap between viewing a list of helpers and a list of plot points
        private void lstOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearContains();
            switch (lstOptions.SelectedItem)
            {
                case "Helpers":
                    {
                        var k = new List<string>(_helpers.Keys);
                        for (int i = 0; i < k.Count; i++)
                        {
                            lstContains.Items.Add(k[i]);
                        }
                    } break;
                case "Plot":
                    {
                        var k = new List<string>(_plot.Keys);
                        for (int i = 0; i < k.Count; i++)
                        {
                            lstContains.Items.Add(k[i]);
                        }
                    } break;
            }
        }

        // Input box for adding an item to Helpers/Plot
        // TODO: reconsider this as it doesn't fit the logic of us changing what we view
        //          (i.e. could accidentally add to plot instead of helpers)
        private void butContainsAdd_Click(object sender, EventArgs e)
        {
            if (lstOptions.SelectedIndex != -1)
            {
                if (txtContainsAdd.Text == "")
                {
                    MessageBox.Show("Please input what you want to add");
                    return;
                }

                string opt = "";
                for (int i = 0; i < txtContainsAdd.Text.Length; i++)
                {
                    switch (txtContainsAdd.Text[i])
                    {
                        case '\t':
                        case ' ':
                        case '\n':
                            // This is attempting to take away unwanted spaces from input like whitespace or tabs only
                            // Though they can more or less be used
                            // TODO: do we still need this? Hopefully not
                            break;
                        default:
                            opt += txtContainsAdd.Text[i];
                            break;
                    }
                }
                if (opt.Length == 0)
                {
                    MessageBox.Show("You can't use that input");
                    return;
                }

                switch (lstOptions.SelectedItem)
                {
                    case "Helpers":
                        {
                            if (!_helpers.ContainsKey(txtContainsAdd.Text))
                            {
                                _helpers.Add(txtContainsAdd.Text, new List<string>());
                                lstContains.Items.Add(txtContainsAdd.Text);
                                ObjectiveDB.RunCMD("INSERT INTO helpers (name) VALUES ('" + txtContainsAdd.Text +"');");
                                txtContainsAdd.Text = "";
                            }
                            else
                            {
                                MessageBox.Show("You already have the helper for " + txtContainsAdd.Text);
                            }
                        } break;
                    case "Plot":
                        {
                            if (!_plot.ContainsKey(txtContainsAdd.Text))
                            {
                                _plot.Add(txtContainsAdd.Text, new List<string>());
                                lstContains.Items.Add(txtContainsAdd.Text);
                                ObjectiveDB.RunCMD("INSERT INTO plots (name)  VALUES ('" + txtContainsAdd.Text + "');");
                                txtContainsAdd.Text = "";
                            }
                            else
                            {
                                MessageBox.Show("You already have the plot element for " + txtContainsAdd.Text);
                            }
                        } break;
                }
            }
        }

        // Update which list we look at: helpers/plot
        private string _selected_index = "";
        private List<string> _selected_items = null;
        private void lstContains_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstContains.SelectedIndex == -1)
            {
                ClearWeb();
            }
            else
            {
                Dictionary<string, List<string>> _content = null;
                switch (lstOptions.SelectedItem)
                {
                    case "Helpers":
                        {
                            _content = _helpers;
                        }
                        break;
                    case "Plot":
                        {
                            _content = _plot;
                        }
                        break;
                }

                var a = _content[(string)lstContains.SelectedItem];
                _selected_index = (string)lstContains.SelectedItem;
                _selected_items = _content[_selected_index];
                GenerateHTML(a);
            }
        }
        
        // Add something that is a main topic for the plot/helper
        private void butContainerAdd_Click(object sender, EventArgs e)
        {
            if (lstContains.SelectedIndex == -1)
            {
                MessageBox.Show("Please select something this contains");
                return;
            }

            

            // TODO: fix user not doing correct things here.
            Dictionary<string, List<string>> _content = null;
            switch (lstOptions.SelectedItem)
            {
                case "Helpers":
                    {
                        _content = _helpers;
                    } break;
                case "Plot":
                    {
                        _content = _plot;
                    } break;
            }
            var items = _content[(string)lstContains.SelectedItem];

            switch (lstOptions.SelectedItem)
            {
                case "Helpers":
                    {
                        items.Add(txtContainerAdd.Text);
                        ObjectiveDB.RunCMD("INSERT INTO notes (val) VALUES ('" + txtContainerAdd.Text + "');");
                        var ans1 = ObjectiveDB.RunCMD("SELECT * FROM helpers WHERE name = '" + (string)lstContains.SelectedItem + "';");
                        ans1.Read();
                        var ans2 = ObjectiveDB.RunCMD("SELECT max(id) FROM notes;");
                        ans2.Read();
                        ObjectiveDB.RunCMD("INSERT INTO helpersjoint (helperid, noteid) VALUES (" + ans1.GetInt32(0) + ", " + ans2.GetInt32(0) + ");");
                        txtContainerAdd.Text = "";
                    }
                    break;
                case "Plot":
                    {
                        items.Add(txtContainerAdd.Text);
                        ObjectiveDB.RunCMD("INSERT INTO notes (val) VALUES ('" + txtContainerAdd.Text + "');");
                        var ans1 = ObjectiveDB.RunCMD("SELECT * FROM plots WHERE name = '" + (string)lstContains.SelectedItem + "';");
                        ans1.Read();
                        var ans2 = ObjectiveDB.RunCMD("SELECT max(id) FROM notes;");
                        ans2.Read();
                        ObjectiveDB.RunCMD("INSERT INTO plotsjoint (plotid, noteid) VALUES (" + ans1.GetInt32(0) + ", " + ans2.GetInt32(0) + ");");
                        txtContainerAdd.Text = "";
                    }
                    break;
            }

            _selected_items = items;

            GenerateHTML(items);
        }
        
        // Enter for "finished this paragraph for you NaNoE"
        private void rtbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var paragraph = rtbInput.Text;
                paragraph = paragraph.Trim(new char[] { ' ', '\n' });
                _novel.Add(paragraph);
                rtbInput.Text = "";
                ObjectiveDB.RunCMD("INSERT INTO paragraphs (para) VALUES ('" + paragraph + "');");
                WebShowNovel();
                UpdateNovelCount();
            }

            UpdateParagraphCount();
        }

        // Add a new chapter marker and update.
        private void butStartChapter_Click(object sender, EventArgs e)
        {
            _novel.Add("[chapter]");
            ObjectiveDB.RunCMD("INSERT INTO paragraphs (para) VALUES ('[chapter]');");
            WebShowNovel();
        }

        // Start editing process
        //  - I need to figure out how to make this easier and minimal (sorry, you might have to close a million edit windows)
        private void butEdit_Click(object sender, EventArgs e)
        {
            if (numStart.Value >= _novel.Count)
            {
                MessageBox.Show("Start index goes from 0 (the first) till 1 less than chapter count (so the usual programming version)");
                return;
            }

            MessageBox.Show("Warning: This could take long if you leave this process till too late...");
            
            for (int i = (int)(numStart.Value); i < _novel.Count; i++)
            {
                if (_novel[i] != "[chapter]")
                {
                    // Thoughts: this can cause memory waste I suppose
                    var editOpts = NaNoEdit.Process(_novel[i]);
                    if (editOpts.Count > 0)
                    {
                        FormEdit NaNoEditForm = new FormEdit();
                        NaNoEditForm.Text += " : p" + (i + 1).ToString();
                        NaNoEditForm.Content = _novel[i];
                        NaNoEditForm.Edits = editOpts;
                        var dialogResult = NaNoEditForm.ShowDialog();

                        // Editing 'done' or just closed
                        _novel[i] = NaNoEditForm.Content;

                        // Just figured I dont mind
                        if (NaNoEditForm.Continue == false) i = _novel.Count;
                    }
                }
            }

            UpdateNovelCount();
            WebShowNovel();

            MessageBox.Show("Edit Helper Complete");
        }

        /// <summary>
        /// Export a word document
        /// </summary>
        // Save a word document, was an easier option instead of pdf
        private void ExportDocX()
        {
            // Sourced helpers: https://www.c-sharpcorner.com/article/generate-word-document-using-c-sharp/
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Word Document (.docx)|.docx";
            sfd.FilterIndex = 2;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (var doc = DocX.Create(sfd.FileName))
                {
                    Formatting _format_head = new Formatting();
                    _format_head.Bold = true;
                    _format_head.Size = 72;

                    Formatting _format_tail = new Formatting();
                    _format_tail.Bold = false;
                    _format_tail.Size = 11;

                    int chCount = 0;

                    var paragraphs = ObjectiveDB.RunCMD("SELECT * FROM paragraphs;");
                    if (paragraphs != null)
                    {
                        if (paragraphs.Read())
                        {
                            do
                            {
                                var line = paragraphs.GetString(1);
                                if (line != "[chapter]") doc.InsertParagraph("\t" + line, false, _format_tail);
                                else doc.InsertParagraph("Ch. " + (++chCount).ToString(), false, _format_head);
                            }
                            while (paragraphs.Read());
                        }
                    }

                    doc.Save();
                }
            }
        }

        /// <summary>
        /// Web view for Helpers/Plot functions for Edit/Delete
        /// </summary>

        /// Function adjustment for seeing if we click edit/delete in main window
        private void webBook_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            // These if statements are so this isnt used unless the url is modified for entry with starting [ or ]
            // [ -> edit
            if (e.Url.AbsolutePath.ToString()[0] == '[') 
            {
                var removal = e.Url.AbsolutePath.ToString().Remove(0, 1);
                int i = int.Parse(removal);
                if (_novel[i] != "[chapter]")
                {
                    // Thoughts: this can cause memory waste I suppose
                    var editOpts = NaNoEdit.Process(_novel[i]);
                    if (editOpts.Count > 0)
                    {
                        FormEdit NaNoEditForm = new FormEdit();
                        NaNoEditForm.Content = _novel[i];
                        NaNoEditForm.Edits = editOpts;
                        var dialogResult = NaNoEditForm.ShowDialog();

                        // Editing 'done' or just closed
                        _novel[i] = NaNoEditForm.Content;

                        // Just figured I dont mind
                        if (NaNoEditForm.Continue == false) i = _novel.Count;
                    }
                }

                UpdateNovelCount();
                WebShowNovel();
            }
            // ] -> delete
            else if (e.Url.AbsolutePath.ToString()[0] == ']')
            {
                DialogResult d = MessageBox.Show("Are you sure you want to delete that element? Your changes will not be saved. Remember you saved BEFORE deleting.", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (d == DialogResult.Yes)
                {
                    // delete
                    var removal = e.Url.AbsolutePath.ToString().Remove(0, 1);
                    int i = int.Parse(removal);

                    var para = _novel[i];
                    ObjectiveDB.RunCMD("DELETE FROM paragraphs WHERE para = '" + para.Replace("'", "''") + "';");
                    UpdateNovelCount();
                    WebShowNovel();
                    
                    MessageBox.Show("Item deleted.");
                }
                else if (d == DialogResult.No)
                {
                    // dont delete
                    MessageBox.Show("We didn't delete that item.");
                }

                UpdateNovelCount();
                WebShowNovel();
            }
        }

        /// Function to manager plot/helpers edit and delete
        private void webContainer_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            // { - Edit
            if (e.Url.AbsolutePath.ToString()[0] == '[')
            {
                var removal = e.Url.AbsolutePath.ToString().Remove(0, 1);
                int i = int.Parse(removal);

                FormEdit NaNoEditForm = new FormEdit();
                NaNoEditForm.Content = _selected_items[i]; ;
                NaNoEditForm.Edits = new List<string>() { "Editing" };
                var dialogResult = NaNoEditForm.ShowDialog();
                
                _selected_items[i] = NaNoEditForm.Content;
                
                GenerateHTML(_selected_items);
            }
            // } - delete
            else if (e.Url.AbsolutePath.ToString()[0] == ']')
            {
                DialogResult d = MessageBox.Show("Are you sure you want to delete that element? Your changes will not be saved. Remember you saved BEFORE deleting.", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (d == DialogResult.Yes)
                {
                    // delete
                    var removal = e.Url.AbsolutePath.ToString().Remove(0, 1);
                    int i = int.Parse(removal);
                    var item = _selected_items[i];
                    _selected_items.RemoveAt(i);

                    if (lstOptions.SelectedIndex == 0) // Helpers
                    {
                        // This can be bugged if same text in 2 notes?
                        var selection = lstContains.SelectedItem.ToString();

                        var helper = ObjectiveDB.RunCMD("SELECT * FROM helpers WHERE name = '" + selection + "';");
                        helper.Read();
                        var note = ObjectiveDB.RunCMD("SELECT * FROM notes WHERE val = '" + item + "';");
                        note.Read();

                        var helperid = helper.GetInt32(0);
                        var noteid = note.GetInt32(0);

                        ObjectiveDB.RunCMD("DELETE FROM notes WHERE id = " + noteid.ToString() + ";");
                        ObjectiveDB.RunCMD("DELETE FROM helpersjoint WHERE helperid = " + helperid + " AND noteid = " + noteid + ";");
                    }
                    else // Plot
                    {
                        var selection = lstContains.SelectedItem.ToString();

                        var plotr = ObjectiveDB.RunCMD("SELECT * FROM plots WHERE name = '" + selection + "';");
                        plotr.Read();
                        var note = ObjectiveDB.RunCMD("SELECT * FROM notes WHERE val = '" + item + "';");
                        note.Read();

                        var plotid = plotr.GetInt32(0);
                        var noteid = note.GetInt32(0);

                        ObjectiveDB.RunCMD("DELETE FROM notes WHERE id = " + noteid.ToString() + ";");
                        ObjectiveDB.RunCMD("DELETE FROM plotsjoint WHERE plotid = " + plotid + " AND noteid = " + noteid + ";");
                    } 


                    MessageBox.Show("Item deleted.");
                }
                else if (d == DialogResult.No)
                {
                    // dont delete
                    MessageBox.Show("We didn't delete that item.");
                }

                GenerateHTML(_selected_items);
            }
            
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ObjectiveDB.Connection != null)
            {
                ObjectiveDB.Connection.Close();
            }
        }

        private void ExportDocXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportDocX();
        }

        private void CreateNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "SQLite|*.sqlite";
            saveFileDialog1.Title = "Please choose the save name";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.  
            if (saveFileDialog1.FileName != "")
            {
                if (File.Exists(saveFileDialog1.FileName))
                {
                    MessageBox.Show("Please don't choose an existing file.", "Not new.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                InitiateDB(saveFileDialog1.FileName);
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "SQLite|*.sqlite";
            openFileDialog1.Title = "Please open your NaNoE sqlite file";
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
                InitiateDB(openFileDialog1.FileName);
            }
        }

        private void InitiateDB(string name)
        {
            lstContains.SelectedIndex = -1;
            lstOptions.SelectedIndex = -1;
            while (lstContains.Items.Count > 0) lstContains.Items.RemoveAt(0);

            ObjectiveDB db = new ObjectiveDB(name);
            ObjectiveDB.TestNew();

            ClearWeb();
            ClearContains();
            WebShowNovel();
        }

        private void ImportnneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "NNE|*.nne";
            openFileDialog1.Title = "Please open your NaNoE .nne file";
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "SQLite|*.sqlite";
                saveFileDialog1.Title = "Please choose the save name";
                saveFileDialog1.ShowDialog();

                // If the file name is not an empty string open it for saving.  
                if (saveFileDialog1.FileName != "")
                {
                    if (File.Exists(saveFileDialog1.FileName))
                    {
                        MessageBox.Show("Please don't choose an existing file.", "Not new.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    InitiateDB(saveFileDialog1.FileName);

                    // Move nne into SQLite file
                    using (var file = openFileDialog1.OpenFile())
                    {
                        using (var reader = new StreamReader(file))
                        {
                            string line = null;
                            char first = ' ';
                            string rest = "";
                            int prevID = -1;
                            while ((line = reader.ReadLine()) != null)
                            {
                                first = line[0];
                                rest = line.Substring(1);
                                rest = rest.Replace("'", "''");
                                switch (first)
                                {
                                    case 'h':
                                        ObjectiveDB.RunCMD("INSERT INTO helpers (name) VALUES ('" + rest + "');");
                                        var obj = ObjectiveDB.RunCMD("SELECT MAX(id) FROM helpers;");
                                        obj.Read();
                                        prevID = obj.GetInt32(0);
                                        break;
                                    case 'H':
                                        ObjectiveDB.RunCMD("INSERT INTO notes (val) VALUES ('" + rest + "');");
                                        var noteid = ObjectiveDB.RunCMD("SELECT MAX(id) FROM notes;");
                                        noteid.Read();

                                        ObjectiveDB.RunCMD("INSERT INTO helpersjoint (helperid, noteid) VALUES (" + prevID + "," + noteid.GetInt32(0) + ");");
                                        break;
                                    case 'p':
                                        ObjectiveDB.RunCMD("INSERT INTO plots (name) VALUES ('" + rest + "');");
                                        var obj2 = ObjectiveDB.RunCMD("SELECT MAX(id) FROM plots;");
                                        obj2.Read();
                                        prevID = obj2.GetInt32(0);
                                        break;
                                    case 'P':
                                        ObjectiveDB.RunCMD("INSERT INTO notes (val) VALUES ('" + rest + "');");
                                        var noteid2 = ObjectiveDB.RunCMD("SELECT MAX(id) FROM notes;");
                                        noteid2.Read();

                                        ObjectiveDB.RunCMD("INSERT INTO plotsjoint (plotid,noteid) VALUES (" + prevID + "," + noteid2.GetInt32(0) + ");");
                                        break;
                                    case 'n':
                                        ObjectiveDB.RunCMD("INSERT INTO paragraphs (para) VALUES ('" + rest + "');");
                                        break;
                                    default:
                                        throw new NotImplementedException("Not implemented: " + first + "...");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
