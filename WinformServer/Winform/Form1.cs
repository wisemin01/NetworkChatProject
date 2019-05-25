using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TCPNetwork;
using TCPNetwork.Server;

namespace TCPNetwork
{
    public partial class ServerGUIForm : Form, ITextDraw, INetworkDebugger
    {
        public ServerGUIForm()
        {
            InitializeComponent();
        }

        public void DrawText(string text)
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.BeginInvoke(new MethodInvoker(delegate
                {
                    textBox1.AppendText(text + Environment.NewLine);
                }));
            }
            else
            {
                textBox1.AppendText(text + Environment.NewLine);
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text) == false)
            {
                try
                {
                    int port = int.Parse(textBox2.Text);

                    NetworkServerManager.Instance.Initialize(port, this);
                    NetworkServerManager.Instance.SetNetworkDebugger(this);
                    NetworkServerManager.Instance.StartServer();

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
                catch (Exception)
                {
                    Application.Exit();
                }

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();

            List<Tuple<TcpClient, string>> list 
                = NetworkServerManager.Instance.GetUserClientList(
                    listBox1.SelectedItem as string);

            if (list == null)
                return;

            foreach (var Iter in list)
            {
                listBox2.Items.Add(Iter.Item2);
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

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void ITextDraw.DrawText(string text)
        {
            throw new NotImplementedException();
        }

        void ITextDraw.ShowMessageBox(string text, string caption)
        {
            throw new NotImplementedException();
        }
    }
}
