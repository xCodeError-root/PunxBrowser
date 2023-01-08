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
	internal class ContextMenuHandler : IContextMenuHandler
	{

		private const int ShowDevTools = 26501;
		private const int CloseDevTools = 26502;
		private const int SaveImageAs = 26503;
		private const int SaveAsPdf = 26504;
		private const int SaveLinkAs = 26505;
		private const int CopyLinkAddress = 26506;
		private const int OpenLinkInNewTab = 26507;
		private const int CloseTab = 40007;
		private const int RefreshTab = 40008;
		private const int Print = 26508;

		private string lastSelText = "";

		Form1 myForm;

		public ContextMenuHandler(Form1 form)
		{
			myForm = form;
		}

		public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
		{

			

		}

		public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
		{

			int id = (int)commandId;

			switch (id)
            {
				case ShowDevTools:

					browser.ShowDevTools();

					break;
				case CloseDevTools:

					browser.CloseDevTools();

					break;
				case SaveImageAs:

					browser.GetHost().StartDownload(parameters.SourceUrl);

					break;
				case SaveLinkAs:

					browser.GetHost().StartDownload(parameters.LinkUrl);

					break;
				case OpenLinkInNewTab:

					Form1.openNewPage(parameters.LinkUrl);

					break;
				case CopyLinkAddress:

					Clipboard.SetText(parameters.LinkUrl);

					break;
				case CloseTab:

					Form1.closeActiveTab();

					break;

				case SaveAsPdf:

					SaveFileDialog sfd = new SaveFileDialog();
					sfd.Filter = "PDF Files | *.pdf";
					if (sfd.ShowDialog() == DialogResult.OK)
					{
						//string path = Path.GetFileName(sfd.FileName);
						browser.PrintToPdfAsync(sfd.FileName, new PdfPrintSettings()
						{
							BackgroundsEnabled = true
						});
					}

					break;
				case Print:

					browser.Print();

					break;
            }


			return false;
		}

		public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
		{

		}

		public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
		{

			// show default menu
			return false;
		}
	}
}
