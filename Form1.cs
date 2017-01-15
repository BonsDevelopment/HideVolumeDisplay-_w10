using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HideVolumeDisplay__w10
{
    public partial class Form1 : Form
    {

        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;

        public Form1()
        {
            InitializeComponent();
            CreateContextMenu();
            notifyIcon1.ShowBalloonTip(1000);
        }

        static private bool IsOpen(string processName)
        {
            return Process.GetProcesses().Any((Process p) => p.ProcessName == (processName));
        }

        static private bool DetectKey()
        {

            if (pInvokes.GetAsyncKeyState(Keys.VolumeUp) || pInvokes.GetAsyncKeyState(Keys.VolumeDown) ||
                    pInvokes.GetAsyncKeyState(Keys.VolumeMute) || pInvokes.GetAsyncKeyState(Keys.MediaPlayPause) ||
                    pInvokes.GetAsyncKeyState(Keys.MediaNextTrack) || pInvokes.GetAsyncKeyState(Keys.MediaPreviousTrack)) { return true; }

            return false;
        }

        private void CheckTimer_Tick(object sender, EventArgs e)
        {
            if (DetectKey())
            {
                IntPtr hwndToVolume = pInvokes.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "NativeHWNDHost", "");

                if (hwndToVolume != IntPtr.Zero)
                    pInvokes.MoveWindow(hwndToVolume, 0, 0, 0, 0, true);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.ShowBalloonTip(200);
            notifyIcon1.ContextMenu = contextMenu1;
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\HideVolumeDisplay_w10.exe"))
                menuItem2.Checked = true;
            else
            {
                menuItem2.Checked = false;
            }
        }

        private void CreateContextMenu()
        {
            contextMenu1 = new ContextMenu();
            menuItem1 = new MenuItem();
            menuItem2 = new MenuItem();

            contextMenu1.MenuItems.AddRange(
                    new System.Windows.Forms.MenuItem[] { this.menuItem1, menuItem2 });


            this.menuItem1.Index = 1;
            this.menuItem1.Text = "E&xit";
            
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

            this.menuItem2.Index = 0;
            this.menuItem2.Text = "S&tartup";

            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);


        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            string startupExecutable = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\HideVolumeDisplay_w10.exe" ;

            if (!menuItem2.Checked)
            {
                
                byte[] curFileBytes = File.ReadAllBytes(Application.ExecutablePath.ToString());

                File.WriteAllBytes(startupExecutable, curFileBytes);
                menuItem2.Checked = true;
            }

            else
            {
                if (File.Exists(startupExecutable))
                    File.Delete(startupExecutable);

                menuItem2.Checked = false;
            }
            


           
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipText = "Closing... Volume display will not be hidden anymore.";
            
            notifyIcon1.ShowBalloonTip(200);
            this.Close();
        }
    }
}
