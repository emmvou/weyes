using System.Diagnostics;
using System.Runtime.InteropServices;

namespace weyes
{
    public partial class Form1 : Form
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public Form1()
        {
            InitializeComponent();
           
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            eyes1.Invalidate();
        }

        private void eyes1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void eyes1_DoubleClick(object sender, EventArgs e)
        {
            if(FormBorderStyle == FormBorderStyle.None)
            {
                var old = PointToScreen(Point.Empty);
                FormBorderStyle = FormBorderStyle.Sizable;
                BackColor = SystemColors.Window;
                var n = PointToScreen(Point.Empty);
                Location = new Point(Location.X - (n.X - old.X), Location.Y - (n.Y - old.Y));
            }
            else
            {
                var old = PointToScreen(Point.Empty);
                FormBorderStyle = FormBorderStyle.None;
                BackColor = Color.Fuchsia;
               Location = old;
            }
        }
    }
}