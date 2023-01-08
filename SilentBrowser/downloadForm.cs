using CefSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SilentBrowser
{
    public partial class downloadForm : Form
    {
        public downloadForm()
        {
            InitializeComponent();
        }

        public static DownloadItem last;
        public static bool brokeDownload;

        public static void updateDownloadInformation(DownloadItem item)
        {

            downloadFormUse.guna2CircleProgressBar1.Value = item.PercentComplete;
            downloadFormUse.guna2HtmlLabel1.Text = "Принято: " + item.ReceivedBytes + "/" + item.TotalBytes;
            downloadFormUse.guna2HtmlLabel2.Text = "Видимая ссылка: " + item.Url;
            downloadFormUse.guna2HtmlLabel3.Text = "Прямая ссылка: " + item.OriginalUrl;
            downloadFormUse.guna2HtmlLabel4.Text = "Полный путь к файлу: " + item.FullPath;
            downloadFormUse.guna2HtmlLabel5.Text = "Название файла: " + item.SuggestedFileName;

            last = item;

        }

        public static downloadForm downloadFormUse;

        private void downloadForm_Load(object sender, EventArgs e)
        {
            downloadFormUse = this;
        }

        private void guna2GradientCircleButton1_Click(object sender, EventArgs e)
        {
            if (last.IsComplete)
            {
                Close();
            }
            else
            {
                if (MessageBox.Show("Вы действительно хотите прервать загрузку?") == DialogResult.OK)
                {
                    last.IsCancelled = true;
                    brokeDownload = true;
                    Close();
                }
            }
        }

    }
}
