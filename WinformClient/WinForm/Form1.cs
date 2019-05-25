using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPNetwork;
using TCPNetwork.Client;

namespace TCPNetwork
{
    public partial class ClientGUIForm : Form, ITextDraw
    {
        public ClientGUIForm()
        {
            InitializeComponent();
            this.textBox3.KeyDown += TextBox3_keyDown;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string serverPort   = textBox2.Text;
            string ipAdress     = textBox4.Text;
            string userName     = textBox5.Text;

            if (string.IsNullOrWhiteSpace(userName))
            {
                MessageBox.Show("이름을 입력해주세요", "Caution");
                return;
            }

            if (string.IsNullOrWhiteSpace(ipAdress))
            {
                MessageBox.Show("IP 주소를 입력해주세요", "Caution");
                return;
            }

            if (string.IsNullOrWhiteSpace(serverPort))
            {
                MessageBox.Show("포트를 입력해주세요", "Caution");
                return;
            }

            try
            {
                NetworkClientManager.Instance.Initialize(userName, ipAdress, int.Parse(serverPort), this);

                if (NetworkClientManager.Instance.ConnectToServer())
                {
                    Connect.Enabled = false;
                }

            }
            catch (FormatException)
            {
                MessageBox.Show("입력 형식이 맞지 않습니다.", "Error");

            }
            catch (OverflowException)
            {
                MessageBox.Show("최대 입력 범위를 벗어났습니다.", "Error");
            }
            catch (Exception)
            {
                Application.Exit();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
                return;

            if (NetworkClientManager.Instance.IsRunning == false)
            {
                MessageBox.Show("서버에 먼저 접속 후 전송을 시도하세요", "Send Error");
                return;
            }

            NetworkClientManager.Instance.SendMessageToServer(textBox3.Text);
            textBox3.Text = string.Empty;
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox3_keyDown(object sender, KeyEventArgs e)
        {
            // 엔터키 입력시 버튼과 같은 효과
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
            }
        }

        private void TextBox9_TextChanged(object sender, EventArgs e)
        {

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
                textBox1.AppendText(text + Environment.NewLine);
        }

        void ITextDraw.ShowMessageBox(string text, string caption)
        {
            MessageBox.Show(text, caption);
        }
    }
}
