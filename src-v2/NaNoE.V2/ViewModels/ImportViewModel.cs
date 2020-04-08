using Microsoft.Office.Interop.Word;
using NaNoE.V2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.ViewModels
{
    class ImportViewModel
    {
        internal void ImportDocX(string doc, string sql)
        {
            DBManager.Instance.Create(sql);

            Application ap = new Application();
            List<Paragraph> Paragraphcs = new List<Paragraph>();
            ap.Visible = false;
            Document document = ap.Documents.Open(doc);
            int where = 0;
            foreach (Paragraph para in document.Content.Paragraphs)
            {
                string s = para.Range.Text.Replace("\r","");
                if (s == "[ch]")
                {
                    // Chapter
                    DBManager.Instance.InsertChapter(where);
                }
                else if (s.Substring(0, 4) == "[co]")
                {
                    // Comment
                    DBManager.Instance.InsertNote(where, s);
                }
                else if (s.Length > 0) // Skip blank lines
                {
                    // Paragraph
                    DBManager.Instance.InsertParagraph(where, s, false);
                }
                // Note: TODO - think should we add EACH feature in general?
                ++where;
            }
            document.Close();

            DBManager.Instance.Disconnect();
        }
    }
}
