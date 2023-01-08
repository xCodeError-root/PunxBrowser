using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentBrowser
{
    internal class Browser
    {
        public ChromiumWebBrowser browser;
        public Browser()
        {

            BrowserSettings settings = new BrowserSettings();
            settings.WebGl = CefState.Enabled;
            settings.WindowlessFrameRate = 120;
            settings.JavascriptAccessClipboard = CefState.Disabled;
            settings.RemoteFonts = CefState.Disabled;
            settings.DefaultFontSize = 16;
            settings.ImageLoading = CefState.Enabled;
            settings.Javascript = CefState.Enabled;
            settings.JavascriptCloseWindows = CefState.Disabled;
            settings.JavascriptDomPaste = CefState.Disabled;
            settings.TabToLinks = CefState.Disabled;

             browser = new ChromiumWebBrowser();
            browser.BrowserHwnd = new IntPtr(new Random().Next(0, 9999999));
            browser.BrowserSettings = settings;

        }

    }
}
