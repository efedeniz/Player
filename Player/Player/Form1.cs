using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Player.Controller;
using Timer = System.Windows.Forms.Timer;
using Microsoft.Win32;

namespace Player
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        public const int KEYEVENTF_EXTENTEDKEY = 1;
        public const int KEYEVENTF_KEYUP = 0;
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;// code to jump to next track
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;// code to play or pause a song
        public const int VK_MEDIA_PREV_TRACK = 0xB1;// code to jump to prev track

        private Controller.LowLevelKeyboardListener listener;
        private bool crtlPressed = false;
        private Timer timer = new Timer
        {
            Interval = 2000
        };
        public Form1()
        {
            InitializeComponent();

            RegistryKey registry = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registry.SetValue("Player", Application.ExecutablePath.ToString());

            this.ShowInTaskbar = false;
            notifyIcon1.Icon = SystemIcons.Application;
            notifyIcon1.Visible = true;
            this.WindowState = FormWindowState.Minimized;

            if (this.WindowState == FormWindowState.Normal)
            {
                
            }
       
         
        }

      
        private void playPause()
        {
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        private void prevTrack()
        {
            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        private void nextTrack()
        {
            keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listener = new Controller.LowLevelKeyboardListener();
            listener.OnKeyPressed += listener_OnKeyPressed;

            listener.HookKeyboard();
        }

        private void listener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            string key = e.KeyPressed.ToString();
           
            timer.Enabled = true;
            timer.Tick += new System.EventHandler(OnTimerEvent);
            if (key.Equals("LeftCtrl"))
            {
                crtlPressed = true;

            }

            Console.Write(key);
          

            if (crtlPressed)
            {
                

                if (key.Equals("Up"))
                {
                    playPause();
                    crtlPressed = false;

                }
                else if (key.Equals("Left"))
                {
                    prevTrack();
                    crtlPressed = false;
                }
                else if (key.Equals("Right"))
                {
                    nextTrack();
                    crtlPressed = false;
                }

               

            }

            
        }

        private void OnTimerEvent(object sender, EventArgs e)
        {
            if (crtlPressed)
            {
                crtlPressed = false;
            }
           

            timer.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            prevTrack();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            playPause();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            nextTrack();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;

            if (this.WindowState == FormWindowState.Normal)
            {
                this.ShowInTaskbar = true;
                notifyIcon1.Icon = SystemIcons.Application;
                notifyIcon1.Visible = false;
                this.WindowState = FormWindowState.Normal;
            }
        }
    }

    
}
