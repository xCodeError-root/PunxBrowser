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
    public partial class downloadPathSelect : Form
    {

        public static bool isPathSelected = false;
        public static bool isPathSessionSelected = false;
        public static string pathToDownload = "";
        public downloadPathSelect()
        {
            InitializeComponent();
        }

        private void downloadPathSelect_Load(object sender, EventArgs e)
        {

        }

        private void guna2GradientCircleButton1_Click(object sender, EventArgs e)
        {
            isPathSelected = false;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (guna2CheckBox1.Checked)
                {
                    isPathSessionSelected = true;
                }
                pathToDownload = dialog.SelectedPath;
                isPathSelected=true;
                Close();
            }  else
            {
                MessageBox.Show("Вы не выбрали путь, чтобы отменить выбор пути просто нажмите на красный кружочек.");
            }
        }
    }
}
