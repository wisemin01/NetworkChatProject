using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows.Forms;

using MNetwork;
using MNetwork.Engine;
using MNetwork.Logic;
using MNetwork.Debuging;
using MNetwork.Time;

namespace ServerHost
{
    public partial class ServerGUIForm : Form
    {
        ChattingLogic logic;

        public ServerGUIForm()
        {
            InitializeComponent();
            FormClosed += OnFormClosing;

            Debug.LogPath = $"./Log/Server[{Time.TimeLogYMD}].txt";
            Debug.OnLog += Debug_OnLog;
        }

        private void OnFormClosing(object sender, EventArgs e)
        {
            Debug.Flush();
        }

        private void Debug_OnLog(object sender, string e)
        {
            DrawText(e);
        }

        private void ServerStartButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
                return;
            try
            {
                int port = int.Parse(textBox2.Text);

                logic = new ChattingLogic();

                MEngine.Instance.Intialize(logic, new ChattingPacketTranslater());
                MEngine.Instance.Start("127.0.0.1", 9199);

                button1.Enabled = false;
            }
            catch (FormatException)
            {
                MessageBox.Show("입력 형식이 맞지 않습니다.", "Error");
            }
            catch (OverflowException ex)
            {
                MessageBox.Show("최대 입력 범위를 벗어났습니다.\n"
                    + "Error Message : " + ex.Message, "Error");
            }
        }
        
        private void RoomListSelectChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();

            var list = logic.NetworkLobby.FindRoom(listBox1.SelectedItem as string).PlayerList;

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
    }
}
