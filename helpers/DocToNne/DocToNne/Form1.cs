using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocToNne
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void ButConvert_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialogue = new OpenFileDialog();
            openFileDialogue.Filter = "Word Documents|*.doc; *.docx";

            if (openFileDialogue.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                var path = openFileDialogue.FileName;
                var miss = System.Type.Missing;
                Microsoft.Office.Interop.Word.Document wordDoc = wordApp.Documents.Open(path,
                    miss, miss, miss, miss,
                    miss, miss, miss, miss,
                    miss, miss, miss, miss,
                    miss, miss, miss);
                
                if (File.Exists("output.nne"))
                {
                    File.Delete("output.nne");
                }

                using (FileStream fileStream = new FileStream("output.nne", FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(fileStream))
                    {
                        fileWriter.WriteLine("hNone\r\nHNo Doc Data\r\npNone\r\nPNo Doc Data");

                        foreach (Paragraph p in wordDoc.Paragraphs)
                        {
                            var line = p.Range.Text.Trim();
                            if (line.IndexOf("Ch. ") == 0)
                            {
                                fileWriter.WriteLine("n[chapter]");
                            }
                            else
                            {
                                fileWriter.WriteLine("n" + line);
                            }
                        }
                    }
                }

                MessageBox.Show("Complete", "File converted to 'output.nne'", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
