using CefSharp;
using CefSharp.WinForms;
using Sipaa.Framework;
using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Gma.System.MouseKeyHook;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace SilentBrowser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            Subscribe();

        }

        private IKeyboardMouseEvents m_GlobalHook;

        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.MouseClick += MouseClickHook;
            m_GlobalHook.KeyDown += KeyDownHook;
            
        }

        private void MouseClickHook (object sender, MouseEventArgs e)
        {

            switch (e.Button)
            {
                case MouseButtons.Middle:

                    if (WindowState == FormWindowState.Normal)
                    {
                        openNewPage(null);
                    } else if (WindowState == FormWindowState.Maximized)
                    {
                        openNewPage(null);
                    }

                    break;
            }

        }

        private void KeyDownHook(object sender, KeyEventArgs e)
        {

            

        }

        public void Unsubscribe()
        {

            
            m_GlobalHook.Dispose();
        }

        private DownloadHandler dHandler;
        public static Form1 myForm;
        private IDisplayHandler diHandler;
        private KeyboardHandler keyboardHandler;
        private void guna2GradientCircleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        public static void openNewPage(String url)
        {
            myForm.createNewPage(url);
        }
        public static void closeActiveTab()
        {
            myForm.tabControl1.TabPages.RemoveAt(myForm.tabControl1.SelectedIndex);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            myForm = this;

            dHandler = new DownloadHandler(this);
            diHandler = new DisplayHandler();
            keyboardHandler = new KeyboardHandler(this);


            tabControl1.SelectedIndexChanged += (a, g) =>
            {
                if (tabControl1.SelectedIndex == tabControl1.TabPages.Count - 1)
                {
                    createNewPage();
                }
            };

        }

        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);
            e.Graphics.FillRectangle(Brushes.Pink, rc);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private void createNewPage(String url = null)
        {

            String pageName = "Page" + new Random().Next(5, 1000000000) + "mda" + new Random().Next(5, 1000000000) + "pizdec" + new Random().Next(5, 1000000000);

            Browser browser = new Browser();

            SButton back = new SButton();
            back.Location = new Point(3, 3);
            back.Size = new Size(30, 31);
            back.Text = "<-";

            SButton update = new SButton();
            update.Location = new Point(39, 3);
            update.Size = new Size(25, 31);
            update.Text = "O";

            SButton next = new SButton();
            next.Location = new Point(70, 3);
            next.Size = new Size(30, 31);
            next.Text = "->";

            STextBox searchBox = new STextBox();
            searchBox.Location = new Point(107, 3);
            searchBox.Size = new Size(tabControl1.Width - 150, 31);

            SButton close = new SButton();
            close.Location = new Point(tabControl1.Width - 35, 3);
            close.Size = new Size(25, 31);
            close.Text = "X";

            browser.browser.Location = new Point(3, 41);
            browser.browser.Size = new Size(tabControl1.Width - 20, tabControl1.Height - 50);

            TabPage page = new TabPage();
            page.Controls.Add(back);
            page.Controls.Add(update);
            page.Controls.Add(next);
            page.Controls.Add(searchBox);
            page.Controls.Add(browser.browser);
            page.Controls.Add(close);

            page.Name = pageName;
            page.Text = "Test";
            page.Size = new Size(tabControl1.Width - 30, tabControl1.Height - 30);

            back.MouseClick += (a, g) =>
            {
                browser.browser.Back();
            };

            update.MouseClick += (a, g) =>
            {
                browser.browser.Reload();
            };

            next.MouseClick += (a, g) =>
            {
                browser.browser.Redo();
            };

            searchBox.KeyDown += (a, g) =>
            {
                
                if (g.KeyCode == Keys.Enter)
                {
                    browser.browser.LoadUrl(searchBox.Texts);
                }
            };
            searchBox.KeyPress += (a, g) =>
            {
                if (g.KeyChar == '\r')
                {
                    browser.browser.LoadUrl(searchBox.Texts);
                }
            };

            
            browser.browser.LoadError += (a, g) =>
            {

                String ErrorCode = g.ErrorCode.ToString();
                String ErrorMessage = g.ErrorText;
                browser.browser.LoadHtml("<html><head><div align='center'></div><title>Error</title></head><body><div align='center'><h2>Punx browser.<br/>Error load page. <br/> Code: " + ErrorCode +  " <br/> Message: " + ErrorMessage + "<br/>Redirect to duck after 3 seconds.</h2></div></body></html>");
                new Thread(() =>
                {
                    Thread.Sleep(3000);
                    browser.browser.BeginInvoke((MethodInvoker)delegate
                    {
                        browser.browser.LoadUrl("https://duckduckgo.com");
                    });
                }).Start();
            };
            browser.browser.LoadingStateChanged += (a, g) =>
            {

            };

            browser.browser.FrameLoadStart += (a, g) =>
            {

                next.BackColor = Color.IndianRed;
                back.BackColor = Color.IndianRed;
                update.BackColor = Color.IndianRed;
                searchBox.BackColor = Color.IndianRed;
                close.BackColor = Color.IndianRed;

            };
            browser.browser.FrameLoadEnd += (a, g) =>
            {
                next.BackColor = Color.MediumSlateBlue;
                back.BackColor = Color.MediumSlateBlue;
                update.BackColor = Color.MediumSlateBlue;
                searchBox.BackColor = Color.MediumSlateBlue;
                close.BackColor = Color.MediumSlateBlue;
            };
            browser.browser.DownloadHandler = dHandler;
            browser.browser.DisplayHandler = diHandler;
            browser.browser.KeyboardHandler = keyboardHandler;
            

            close.MouseClick += (a, g) =>
            {
                searchBox.BeginInvoke((MethodInvoker)delegate
                {
                    int i = 0;
                    foreach (TabPage pag in tabControl1.TabPages)
                    {
                        if (pag.Name == pageName)
                        {
                            tabControl1.TabPages.RemoveAt(i);
                        }
                        i = i + 1;
                    }
                });
            };

            browser.browser.AddressChanged += (a, g) =>
            {
                searchBox.BeginInvoke((MethodInvoker)delegate
                {
                    searchBox.Texts = g.Address;
                });
            };

            browser.browser.TitleChanged += (a, g) =>
            {
                searchBox.BeginInvoke((MethodInvoker)delegate
                {
                    int i = 0;
                    foreach (TabPage pag in tabControl1.TabPages)
                    {
                        if (pag.Name == pageName)
                        {
                            tabControl1.TabPages[i].Text = g.Title;
                        }
                        i = i + 1;
                    }
                });
            };

            String ourl = null;

            if (url == null)
            {
                 ourl = "https://duckduckgo.com";
            } else
            {
                ourl = url;
            }

            browser.browser.LoadUrl(ourl);

            tabControl1.TabPages.Insert(tabControl1.TabPages.Count - 1,page);
            tabControl1.SelectedIndex = tabControl1.TabPages.Count - 2;

            searchBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            close.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            back.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            update.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            next.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            browser.browser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        }

        private void sButton3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            createNewPage();
            tabControl1.TabPages.RemoveAt(0);
        }

        private void guna2GradientCircleButton2_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            } else
            {
                WindowState = FormWindowState.Maximized;
            }
        }

        private void guna2GradientCircleButton3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            openNewPage("https://byxatab.com/");
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            openNewPage("https://rsload.net/");
        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {
            createNewPage();
            tabControl1.TabPages.RemoveAt(0);
        }

        private void guna2PictureBox5_Click(object sender, EventArgs e)
        {
            createNewPage("https://byxatab.org/");
            tabControl1.TabPages.RemoveAt(0);
        }

        private void guna2PictureBox4_Click(object sender, EventArgs e)
        {
            createNewPage("https://rsload.net/");
            tabControl1.TabPages.RemoveAt(0);
        }

        private void guna2PictureBox6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("LordFilm это пираты которые предоставляют бесплатный доступ ко всем фильмам\nНо... LordFilm часто банят поэтому он использует тысячи зеркал\n" +
                "Отследить зеркала невозможно поэтому я открою страничку в утке\nИ ты сам(а) должен зайти на сайт, примеры зеркал lordfilm:\n" +
                "https://v.lordfilm.film/\n" +
                "https://tv-9.lordfilm6.net/\n" +
                "И прочие подобные зеркала. Слава непотопляемому кораблю пиратов!", "Сайт LordFilm",MessageBoxButtons.OK,MessageBoxIcon.Information);
            createNewPage("https://duckduckgo.com/?q=LordFilm");
        }

        private void guna2PictureBox7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("LordFilm это пираты которые предоставляют бесплатный доступ ко всем фильмам\nНо... LordFilm часто банят поэтому он использует тысячи зеркал\n" +
                "Отследить зеркала невозможно поэтому я открою страничку в утке\nИ ты сам(а) должен зайти на сайт, примеры зеркал lordfilm:\n" +
                "https://v.lordfilm.film/\n" +
                "https://tv-9.lordfilm6.net/\n" +
                "И прочие подобные зеркала. Слава непотопляемому кораблю пиратов!", "Сайт LordFilm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            createNewPage("https://duckduckgo.com/?q=LordFilm");
        }
    }
}
