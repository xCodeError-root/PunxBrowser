using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentBrowser
{
	internal class DownloadHandler : IDownloadHandler
	{
		readonly Form1 myForm;

		public DownloadHandler(Form1 form)
		{
			myForm = form;
		}

		public bool CanDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, string url, string requestMethod)
		{
			return true;
		}

		private bool isOpenedDownloadForm = false;


		public void OnBeforeDownload(IWebBrowser webBrowser, IBrowser browser, DownloadItem item, IBeforeDownloadCallback callback)
		{
			Console.WriteLine("upload");
			if (!callback.IsDisposed)
			{
				using (callback)
				{

					
					String path = null;

					if (downloadPathSelect.isPathSessionSelected)
                    {
						path = downloadPathSelect.pathToDownload + "\\" + item.SuggestedFileName;
                    } else
                    {
						downloadPathSelect pathSelector = new downloadPathSelect();
						pathSelector.ShowDialog();
						if (downloadPathSelect.isPathSelected)
                        {
							path = downloadPathSelect.pathToDownload + "\\" + item.SuggestedFileName;
                        }
                    }

					// if file should not be saved, path will be null, so skip file
					if (path == null)
					{

						callback.Continue(path, false);

					}
					else
					{
						callback.Continue(path, true);

						
						
					}

				}
			}
		}

		public void OnDownloadUpdated(IWebBrowser webBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
		{
			if (!isOpenedDownloadForm)
			{
				downloadForm download = new downloadForm();
				download.Show();
				isOpenedDownloadForm = true;
			}

			downloadForm.updateDownloadInformation(downloadItem);
			if (downloadItem.IsInProgress)
			{
				
				//callback.Cancel();
			}
			//Console.WriteLine(downloadItem.Url + " %" + downloadItem.PercentComplete + " complete");
		}
	}
}
