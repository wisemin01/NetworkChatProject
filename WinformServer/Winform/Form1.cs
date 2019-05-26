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
    public partial class ServerGUIForm : Form, ITextDraw, INetworkOutput
    {
        public ServerGUIForm()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text) == false)
            {
                try
                {
                    int port = int.Parse(textBox2.Text);

                    NetworkServerManager.Instance.Initialize(port);
                    NetworkServerManager.Instance.TextDraw      = this;
                    NetworkServerManager.Instance.NetworkOutput = this;
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
        
        void ITextDraw.DrawText(string text)
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

        void ITextDraw.ShowMessageBox(string text, string caption)
        {
            MessageBox.Show(text, caption);
        }

        void ITextDraw.DrawColorText(string text, int r, int g, int b, int a)
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

        void ITextDraw.ClearText()
        {
            throw new NotImplementedException();
        }
    }
}
