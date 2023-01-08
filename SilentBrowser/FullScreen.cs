using CefSharp;
using CefSharp.Enums;
using CefSharp.Structs;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SilentBrowser
{


    public class DisplayHandler : IDisplayHandler
    {
        public static Control parent;
        public static Form fullScreenForm;
        void IDisplayHandler.OnAddressChanged(IWebBrowser browserControl, AddressChangedEventArgs addressChangedArgs)
        {
        }
        void IDisplayHandler.OnTitleChanged(IWebBrowser browserControl, TitleChangedEventArgs titleChangedArgs)
        {
        }
        void IDisplayHandler.OnFaviconUrlChange(IWebBrowser browserControl, IBrowser browser, IList<string> urls)
        {
        }

        public static  System.Drawing.Size LastSize = new System.Drawing.Size(0, 0);
        public static  System.Drawing.Point LastLocation = new System.Drawing.Point(0, 0);

        public static bool now = false;

        void IDisplayHandler.OnFullscreenModeChange(IWebBrowser browserControl, IBrowser browser, bool fullscreen)
        {
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;
            if (chromiumWebBrowser == null)
            {
                return;
            }
            chromiumWebBrowser.BeginInvoke((MethodInvoker)delegate
            {
                try
                {

                        now = fullscreen;
                        if (fullscreen)
                        {
                            parent = chromiumWebBrowser.Parent;
                            parent.Controls.Remove(chromiumWebBrowser);
                            fullScreenForm = new Form();
                            fullScreenForm.FormBorderStyle = FormBorderStyle.None;
                            fullScreenForm.WindowState = FormWindowState.Maximized;
                            fullScreenForm.Controls.Add(chromiumWebBrowser);

                            LastSize = chromiumWebBrowser.Size;
                            LastLocation = chromiumWebBrowser.Location;

                            chromiumWebBrowser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                            chromiumWebBrowser.Size = new System.Drawing.Size(fullScreenForm.Width - 10, fullScreenForm.Height - 10);
                            chromiumWebBrowser.Location = new System.Drawing.Point(0, 0);

                        chromiumWebBrowser.Focus();
                        fullScreenForm.ShowDialog(parent.FindForm());


                        }
                        else
                        {

                            chromiumWebBrowser.Size = LastSize;
                            chromiumWebBrowser.Location = LastLocation;

                            fullScreenForm.Controls.Remove(chromiumWebBrowser);
                            parent.Controls.Add(chromiumWebBrowser);
                            fullScreenForm.Close();
                            fullScreenForm.Dispose();
                            fullScreenForm = null;

                        chromiumWebBrowser.Focus();

                        }

                } catch (Exception)
                {

                }
            });
        }
        public bool fullScreen(IWebBrowser browserControl, bool fullscreen)
        {
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;
            chromiumWebBrowser.BeginInvoke((MethodInvoker)delegate
            {
                if (fullscreen)
                {
                    parent = chromiumWebBrowser.Parent;
                    parent.Controls.Remove(chromiumWebBrowser);
                    fullScreenForm = new Form();
                    fullScreenForm.FormBorderStyle = FormBorderStyle.None;
                    fullScreenForm.WindowState = FormWindowState.Maximized;
                    fullScreenForm.Controls.Add(chromiumWebBrowser);

                    LastSize = chromiumWebBrowser.Size;
                    LastLocation = chromiumWebBrowser.Location;

                    chromiumWebBrowser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                    chromiumWebBrowser.Size = new System.Drawing.Size(fullScreenForm.Width - 10, fullScreenForm.Height - 10);
                    chromiumWebBrowser.Location = new System.Drawing.Point(0, 0);

                    chromiumWebBrowser.Focus();

                    fullScreenForm.ShowDialog(parent.FindForm());


                }
                else
                {

                    chromiumWebBrowser.Size = LastSize;
                    chromiumWebBrowser.Location = LastLocation;

                    fullScreenForm.Controls.Remove(chromiumWebBrowser);
                    parent.Controls.Add(chromiumWebBrowser);
                    fullScreenForm.Close();
                    fullScreenForm.Dispose();
                    fullScreenForm = null;

                    chromiumWebBrowser.Focus();
                }
            });
            return false;
        }
        void IDisplayHandler.OnStatusMessage(IWebBrowser browserControl, StatusMessageEventArgs statusMessageArgs)
        {
        }
        bool IDisplayHandler.OnConsoleMessage(IWebBrowser browserControl, ConsoleMessageEventArgs consoleMessageArgs)
        {
            return false;
        }

        public bool OnAutoResize(IWebBrowser chromiumWebBrowser, IBrowser browser, Size newSize)
        {
            return false;  //throw new NotImplementedException();
        }

        public bool OnCursorChange(IWebBrowser chromiumWebBrowser, IBrowser browser, IntPtr cursor, CursorType type, CursorInfo customCursorInfo)
        {
            return false;  //throw new NotImplementedException();
        }

        public void OnLoadingProgressChange(IWebBrowser chromiumWebBrowser, IBrowser browser, double progress)
        {
            ;  //throw new NotImplementedException();
        }

        public bool OnTooltipChanged(IWebBrowser chromiumWebBrowser, ref string text)
        {
            return false; //throw new NotImplementedException();
        }
    }
}
