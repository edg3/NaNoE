using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NaNoE.V2
{
    class NovelWebView
    {
        private static NovelWebView _instance;
        public static NovelWebView Instance { get { return _instance; } }

        private string _html1 = @"<html><head><style>";
        private string _css = @"
                               body { background: #DDD }
                               ";
        private string _html2 = @"</style>" + 
                                "<script type=\"text/javascript\">function OnEnter() { window.external.OnEnter(); }"
                                + "</head><body>";
        private string _html3 = @"</body></html>";

        internal static void Initiate(WebBrowser webView)
        {
            _instance = new NovelWebView(webView);
        }

        private NovelWebView(WebBrowser webView)
        {
            _webView = webView;
            _webView.ObjectForScripting = new NovelWebScript();
        }

        private int _index = 0;
        public int Index { get { return _index; } set { _index = value; RefreshView(); } }

        private WebBrowser _webView;
        
        public WebBrowser WebView { get { return _webView; } }    

        public void RefreshView()
        {
            if (!NovelDB.Connected)
            {
                _webView.NavigateToString(_html1 + _css + _html2 +
                                            "Content" +
                                          _html3);
            }
            else
            {
                var elements = NovelDB.GetView();
                var shownElements = "";
                for (int i = 0; i < elements.Count; i++)
                {
                    shownElements += "<div>" + elements[i].GetWeb() + "<br /></div>";
                }

                shownElements += "<hr/><textarea></textarea>";

                _webView.NavigateToString(_html1 + _css + _html2 +
                            shownElements +
                          _html3);
            }
        }

        public string GetView()
        {
            return "Temporary.";
        }
    }
}
