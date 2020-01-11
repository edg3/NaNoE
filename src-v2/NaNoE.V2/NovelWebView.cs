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
        private string _html2 = @"</style></head><body>";
        private string _html3 = @"</body></html>";

        internal static void Initiate(WebBrowser webView)
        {
            _instance = new NovelWebView(webView);
        }

        private NovelWebView(WebBrowser webView)
        {
            _webView = webView;
        }

        private int _index = 0;
        public int Index { get { return _index; } set { _index = value; RefreshView(); } }

        private WebBrowser _webView;
        
        public WebBrowser WebView { get { return _webView; } }    

        public void RefreshView()
        {
            if (true)
            {
                _webView.NavigateToString(_html1 + _css + _html2 +
                                            "Content" +
                                          _html3);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public string GetView()
        {
            return "Temporary.";
        }
    }
}
