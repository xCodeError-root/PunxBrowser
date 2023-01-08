using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SilentBrowser
{
	internal class KeyboardHandler : IKeyboardHandler
	{

		Form1 myForm;


		public KeyboardHandler(Form1 form)
		{
			myForm = form;
		}

		public bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
		{
			return false;
		}

        public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
		{

            if (type == KeyType.RawKeyDown)
            {
                switch (windowsKeyCode)
                {
                    case 122:

						if (!DisplayHandler.now)
						{
							DisplayHandler handler = new DisplayHandler();
							handler.fullScreen(browserControl, true);
							DisplayHandler.now = true;
						} else
                        {
							DisplayHandler handler = new DisplayHandler();
							handler.fullScreen(browserControl, false);
							DisplayHandler.now = false;
						}
                        break;
                }
            }
			return false;
		}
	}
}
