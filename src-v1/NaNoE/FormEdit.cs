using NaNoE.Objective;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NaNoE
{
    public partial class FormEdit : Form
    {
        public int ParagraphID { get; private set; }

        public FormEdit()
        {
            InitializeComponent();
            Continue = true;

            ParagraphID = -1;
        }

        private string _content = "";
        public string Content
        {
            get
            {
                return _content;
            }
            internal set
            {
                _content = value;

                // First content = ID, perhaps move to create?
                if (ParagraphID == -1)
                {
                    var answer = ObjectiveDB.RunCMD("SELECT id FROM paragraphs WHERE para = '" + Content.Replace("'","''") + "';");
                    answer.Read();
                    ParagraphID = answer.GetInt32(0);
                }
            }
        }
        public List<string> Edits { get; internal set; }
        public bool Continue { get; internal set; }

        private void FormEdit_Load(object sender, EventArgs e)
        {
            // Thoughts: Hmm, what about 'Enter' and 'Tab'? I think it can help editing but we keep it out of main?

            for (int i = 0; i < Edits.Count; i++)
            {
                lstProblems.Items.Add(Edits[i]);
            }

            rtbParagraph.Text = Content;
        }

        private void rtbParagraph_TextChanged(object sender, EventArgs e)
        {
            Content = rtbParagraph.Text;
        }

        private void butRefresh_Click(object sender, EventArgs e)
        {
            while (lstProblems.Items.Count > 0) lstProblems.Items.RemoveAt(0);
            var processing = NaNoEdit.Process(Content);
            foreach (var a in processing) lstProblems.Items.Add(a);
        }

        private void butDone_Click(object sender, EventArgs e)
        {
            if (ParagraphID != -1) ObjectiveDB.UpdatePara(ParagraphID, Content);
            this.Close();
        }

        private void butStopForNow_Click(object sender, EventArgs e)
        {
            if (ParagraphID != -1) ObjectiveDB.UpdatePara(ParagraphID, Content);
            Continue = false;
            this.Close();
        }

        private void RtbParagraph_SelectionChanged(object sender, EventArgs e)
        {
            var tmp = rtbParagraph.Text.Substring(0, rtbParagraph.SelectionStart).Count(a => a == ' ') + 1;
            lblPosition.Text = "Position: " + rtbParagraph.SelectionStart.ToString() + " / Word: " + tmp;
        }

        private void LstProblems_DoubleClick(object sender, EventArgs e)
        {
            if (lstProblems.SelectedIndex != -1)
            {
                MessageBox.Show(lstProblems.SelectedItem.ToString(), "Read More", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
