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
        
        private void RoomListSelectChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();

            var list = ServerManager.Instance.Lobby.FindRoom(listBox1.SelectedItem as string).PlayerContainer;

            if (list == null)
                return;

            foreach (var Iter in list)
            {
                listBox2.Items.Add(Iter.Value.UserName);
            }
        }

        public void AddRoomToListBox(string roomName)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.BeginInvoke(new MethodInvoker(delegate
                {
                    listBox1.Items.Add(roomName);
                }));
            }
            else
            {
                listBox1.Items.Add(roomName);
            }
        }

        public void RemoveRoomToListBox(string roomName)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.BeginInvoke(new MethodInvoker(delegate
                {
                    listBox1.Items.Remove(roomName);
                }));
            }
            else
            {
                listBox1.Items.Remove(roomName);
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
