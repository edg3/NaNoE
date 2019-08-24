﻿using NaNoE.Objective;
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

            int chapterNum = 0;
            try // TODO: this may need adjustments
            {
                chapterNum =
                    _novel.GroupBy(x => x,
                                   StringComparer.InvariantCultureIgnoreCase)
                          .ToDictionary(x => x.Key, x => x.Count())
                          ["[chapter]"];
            }
            catch { /* Opinions needed, this feels filthy */ }

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
                    case "[chapter]": midPoint += "<hr /><div><b>" + chapterNum.ToString() + "</b> [ <i><a href=\"]" + (_novel.Count - 5 + i).ToString() + "\">Del</a></i> ]</div><br /><br />"; break;
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
                ans += "<li><div>" + items[i] + " [ <i><a href=\"["+ i.ToString() +"\">Edit</a>, <a href=\"]"+ i.ToString() +"\">Del</a></i> ]</div></li>";
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
            lblParagraphCount.Text = "P: " + (rtbInput.Text.Split(' ')).Length.ToString();
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
            items.Add(txtContainerAdd.Text);
            ObjectiveDB.RunCMD("INSERT INTO notes (val) VALUES ('" + txtContainerAdd.Text + "');");
            var ans1 = ObjectiveDB.RunCMD("SELECT * FROM helpers WHERE name = '" + (string)lstContains.SelectedItem +"';");
            ans1.Read();
            var ans2 = ObjectiveDB.RunCMD("SELECT max(id) FROM notes;");
            ans2.Read();
            ObjectiveDB.RunCMD("INSERT INTO helpersjoint (helperid, noteid) VALUES (" + ans1.GetInt32(0) + ", " + ans2.GetInt32(0) + ");");
            txtContainerAdd.Text = "";

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
        private void butExport_Click(object sender, EventArgs e)
        {
            // Sourced helpers: https://www.c-sharpcorner.com/article/generate-word-document-using-c-sharp/
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Word Document (.docx)|.docx|All Files (*.*)|*.*";
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
                    foreach (var line in _novel)
                    {
                        if (line != "[chapter]") doc.InsertParagraph("\t" + line, false, _format_tail);
                        else doc.InsertParagraph("Ch. " + (++chCount).ToString(), false, _format_head);
                    }

                    doc.Save();
                }
            }
        }

        /// <summary>
        /// Saving and Loading Data
        /// </summary>
        // Save a document obviously
        private void butSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            // Bug: wont show the files. Hmm...
            sfd.Filter = "nne saves (*.nne)|.nne|All Files (*.*)|*.*";
            sfd.FilterIndex = 2;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // TODO: thoughts on this? You should manage your own backups I suppose...
                if (File.Exists(sfd.FileName))
                {
                    File.Delete(sfd.FileName);
                }
                var f = File.OpenWrite(sfd.FileName);
                var fw = new StreamWriter(f);

                // Save all helpers
                var k = new List<string>(_helpers.Keys);
                for (int i = 0; i < _helpers.Count; i++)
                {
                    string title = "", content = "";
                    string title_fluff = "";
                    
                    title = k[i];
                    // Save format: 'h' + <title> = title of the helper to start in loading
                    fw.WriteLine("h" + title);
                    // Make it 10 chars long for processing ease
                    if (title.Length < 10)
                    {
                        while ((title + title_fluff).Length < 10)
                            title_fluff += " ";
                    }
                    // Go through all contents of the helpers to share what we have in the helper
                    // Save formate: 'H' + <title> + <title_fluff> + <content> = the 10 char long name in the file
                    // TODO: remove the spaces on input from front and back I suddenly thought
                    for (int j = 0; j < _helpers[title].Count; j++)
                    {
                        content = _helpers[title][j];
                        // See [W] - this is a copy of 'fix'
                        // while (content.Contains(title)) content = content.Remove(0, 10);
                        fw.WriteLine("H" + content);
                        fw.Flush();
                    }
                }

                // Save all plot
                var l = new List<string>(_plot.Keys);
                for (int i = 0; i < _plot.Count; i++)
                {
                    string title = "", content = "";
                    string title_fluff = "";

                    title = l[i];
                    // Save format: 'h' + <title> = title of the plot point to start in loading
                    fw.WriteLine("p" + title);
                    // Make it 10 chars long for processing ease
                    if (title.Length < 10)
                    {
                        while ((title + title_fluff).Length < 10)
                            title_fluff += " ";
                    }
                    // Go through all contents of the helpers to share what we have in the helper
                    // Save formate: 'H' + <title> + <title_fluff> + <content> = the 10 char long name in the file
                    // TODO: remove the spaces on input from front and back I suddenly thought [Q] has suggestion, may have been fixed with input safety
                    for (int j = 0; j < _plot[title].Count; j++)
                    {
                        content = _plot[title][j];
                        // [W] Note: saving for some reason needs missing title, how it loads perhaps?
                        //   - We may want to bring the removal of the title back eventually
                        //     I double checked that using the new fix to the save system works
                        // while (content.Contains(title)) content = content.Remove(0, 10);
                        fw.WriteLine("P" + content);
                        fw.Flush();
                    }
                }

                // Save all chapters
                //  - AKA. Saving the actual novel
                for (int i = 0; i < _novel.Count; i++)
                {
                    var ans = _novel[i];
                    ans = ans.Trim(' '); // [Q]

                    fw.WriteLine("n" + ans);
                    fw.Flush();
                }

                // Finito - always flush
                fw.Flush();
                fw.Close();
                f.Close();
            }
        }

        // Load a document obviously
        // TODO: consider makin it double check only if you actually have done something it should check?
        private void butLoad_Click(object sender, EventArgs e)
        {
            bool cont = true;
            if (_plot.Count != 0 || _novel.Count != 0 || _helpers.Count != 0) // This means you typed something
                if (MessageBox.Show("Have you saved everything you need to save?", "Double Checking", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    cont = false;

            if (cont)
            {
                lstContains.SelectedIndex = -1;
                lstOptions.SelectedIndex = -1;
                while (lstContains.Items.Count > 0) lstContains.Items.RemoveAt(0);

                ObjectiveDB db = new ObjectiveDB("temp.sqlite");
                ObjectiveDB.TestNew();

                // _helpers = new Dictionary<string, List<string>>();
                // _novel = new List<string>();
                // _plot = new Dictionary<string, List<string>>();

                ClearWeb();
                ClearContains();
                WebShowNovel();
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
                    _novel.RemoveAt(i);
                    
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
    }
}
