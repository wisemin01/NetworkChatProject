using System;
using System.Threading;
using System.Windows.Forms;

using MNetwork.Engine;
using MNetwork.Debuging;
using MNetwork.Time;
using MNetwork.Packet;
using MNetwork.RPC;

using ChattingNetwork.Client;
using MNetwork.Utility;

namespace ClientHost
{
    public partial class ClientGUIForm : Form
    {
        public ClientGUIForm()
        {
            InitializeComponent();
            SendContextBox.KeyDown += SendFieldKeyDown;
            FormClosed += OnFormClosing;
            ClientManager.Instance.AddDebuger(Debug_OnLog);
        }

        private void OnFormClosing(object sender, EventArgs e)
        {
            ClientManager.Instance.Disconnect();
        }

        private void Debug_OnLog(object sender, string e)
        {
            DrawText(e);
        }

        private void ConnectButtonClick(object sender, EventArgs e)
        {
            INIFile.Get("server_ip", out string ip, "./Data/clientinfo.ini", "CLIENT");
            INIFile.Get("server_port", out string port, "./Data/clientinfo.ini", "CLIENT");

            ClientManager.Instance.Connect(ip, ushort.Parse(port));
            ClientManager.Instance.UpdateAsync();

            Debug.Log("This client runs on a Windows Form");
        }

        private void SendButtonClick(object sender, EventArgs e)
        {
            if (CommandParser.Parse(SendContextBox.Text))
            {
                SendContextBox.Text = string.Empty;
                return;
            }

            bool result = ClientManager.Instance.SendChat(SendContextBox.Text);

            if (result == true)
                SendContextBox.Text = string.Empty;
        }

        private void SendFieldKeyDown(object sender, KeyEventArgs e)
        {
            // 엔터키 입력시 버튼과 같은 효과
            if (e.KeyCode == Keys.Enter)
            {
                SendButton.PerformClick();
            }
        }

        void DrawText(string text)
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.BeginInvoke(new MethodInvoker(delegate
                {
                    textBox1.AppendText(text + Environment.NewLine);
                }));
            }
            else
                textBox1.AppendText(text + Environment.NewLine);
        }
    }
}
