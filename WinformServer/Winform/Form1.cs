using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows.Forms;

using ChattingNetwork.Server;
using MNetwork.Utility;
using MNetwork.Debuging;

namespace ServerHost
{
    public partial class ServerGUIForm : Form
    {
        public ServerGUIForm()
        {
            InitializeComponent();
            FormClosed += OnFormClosing;
            ServerManager.Instance.AddDebuger(Debug_OnLog);
        }

        private void OnFormClosing(object sender, EventArgs e)
        {
            ServerManager.Instance.Stop();
        }

        private void Debug_OnLog(object sender, string e)
        {
            DrawText(e);
        }

        private void ServerStartButtonClick(object sender, EventArgs e)
        {
            INIFile.Get("ip", out string ip, "./Data/serverinfo.ini", "SERVER");
            INIFile.Get("port", out string port, "./Data/serverinfo.ini", "SERVER");

            bool result = ServerManager.Instance.Start(ip, ushort.Parse(port));

            if (result == true)
            {
                button1.Enabled = false;
            }
        }
        
        void DrawText(string text)
        {
            if (ChattingList.InvokeRequired)
            {
                ChattingList.BeginInvoke(new MethodInvoker(delegate
                  {
                      ChattingList.AppendText(text + Environment.NewLine);
                  }));
            }
            else
            {
                ChattingList.AppendText(text + Environment.NewLine);
            }
        }

        private void ThreadCount_Click(object sender, EventArgs e)
        {
            Debug.Log($"THREAD COUNT : {System.Diagnostics.Process.GetCurrentProcess().Threads.Count}");
        }
    }
}
