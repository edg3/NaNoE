using NaNoE.Objective;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            ObjectiveDB odb = new ObjectiveDB("test.sqlite");

            ClearWeb();
            WebShowNovel();

            NaNoEdit.Init();
        }

        /// <summary>
        /// Stored Novel Data
        /// </summary>
        Dictionary<string, List<string>> _helpers = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> _plot = new Dictionary<string, List<string>>();
        List<string> _novel = new List<string>();
        
        /// <summary>
        /// Interface Refreshing Code
        /// </summary>
        // Generates the last 5 paragraphs we inputted
        private void WebShowNovel()
        {
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
        int _updateCountNovel = 0;
        int _updateCountNovelTo = 0;
        public void UpdateNovelCount()
        {
            if (_novel.Count > _updateCountNovelTo)
            {
                for (int q = _updateCountNovelTo; q < _novel.Count; q++)
                {
                    var s = _novel[q];
                    if (s != "[chapter]")
                    {
                        var t = s.Split(' ');
                        _updateCountNovel += t.Length; // TODO: potentially consider words without space e.g. 'end.then the'
                    }
                }
                _updateCountNovelTo = _novel.Count;
                lblNovelCount.Text = "Words: " + _updateCountNovel.ToString();
            }
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
            txtContainerAdd.Text = "";

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
                WebShowNovel();
                UpdateNovelCount();
            }

            UpdateParagraphCount();
        }

        // Add a new chapter marker and update.
        private void butStartChapter_Click(object sender, EventArgs e)
        {
            _novel.Add("[chapter]");
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

            _updateCountNovel = 0;
            _updateCountNovelTo = 0;
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

                _helpers = new Dictionary<string, List<string>>();
                _novel = new List<string>();
                _plot = new Dictionary<string, List<string>>();

                ClearWeb();
                ClearContains();
                WebShowNovel();

                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    //Load here
                    var f = File.OpenRead(ofd.FileName);
                    var fr = new StreamReader(f);

                    // helper
                    string l = fr.ReadLine();
                    string balf_ = "";
                    List<string> _flab = null;
                    do
                    {
                        if (l[0] == 'h')
                        {
                            l = l.Remove(0, 1);
                            if (_flab != null)
                            {
                                _helpers.Add(balf_, _flab);
                            }
                            balf_ = l;
                            _flab = new List<string>();

                            l = fr.ReadLine();
                        }
                        else if (l[0] == 'H')
                        {
                            l = l.Remove(0, 1);
                            _flab.Add(l);

                            l = fr.ReadLine();
                        }
                    }
                    while ((l[0] == 'h') || (l[0] == 'H'));
                    if (_flab != null)
                    {
                        _helpers.Add(balf_, _flab);
                        _flab = null;
                        balf_ = "";
                    }

                    // plot
                    do
                    {
                        if (l[0] == 'p')
                        {
                            l = l.Remove(0, 1);
                            if (_flab != null)
                            {
                                _plot.Add(balf_, _flab);
                            }
                            balf_ = l;
                            _flab = new List<string>();

                            l = fr.ReadLine();
                        }
                        else if (l[0] == 'P')
                        {
                            l = l.Remove(0, 1);
                            _flab.Add(l);

                            l = fr.ReadLine();
                        }
                    }
                    while ((l[0] == 'p') || (l[0] == 'P'));

                    if (_flab != null)
                    {
                        _plot.Add(balf_, _flab);
                        _flab = null;
                        balf_ = "";
                    }

                    int minorCount = 0;
                    int chapCount = 0;
                    // chapter
                    do
                    {
                        if (l[0] == 'n')
                        {
                            l = l.Remove(0, 1);
                            _novel.Add(l);

                            if (l != "[chapter]")
                            {
                                minorCount += l.Split(' ').Length;
                            }
                            else
                            {
                                chapCount++;
                            }

                            l = fr.ReadLine();
                        }
                    }
                    while (l != null);

                    _updateCountNovel = minorCount;
                    _updateCountNovelTo = _novel.Count - 1;
                    UpdateNovelCount();

                    WebShowNovel(); ;
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

                _updateCountNovel = 0;
                _updateCountNovelTo = 0;
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

                _updateCountNovel = 0;
                _updateCountNovelTo = 0;
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
                    _selected_items.RemoveAt(i);

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
