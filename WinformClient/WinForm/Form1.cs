using System;
using System.Threading;
using System.Windows.Forms;

using MNetwork.Engine;
using MNetwork.Debuging;
using MNetwork.Time;
using MNetwork.Packet;
using MNetwork.RPC;

using ChattingPacket;

namespace ClientHost
{
    public partial class ClientGUIForm : Form
    {
        public ClientGUIForm()
        {
            InitializeComponent();
            SendContextBox.KeyDown += SendFieldKeyDown;
            FormClosed += OnFormClosing;

            Debug.LogPath = $"./Log/Client[{Time.TimeLogYMD}].txt";
            Debug.OnLog += Debug_OnLog;
        }

        private void OnFormClosing(object sender, EventArgs e)
        {
            MNetworkEntry.Instance.Shutdown();
            Debug.Flush();
        }

        private void Debug_OnLog(object sender, string e)
        {
            DrawText(e);
        }

        private void ConnectButtonClick(object sender, EventArgs e)
        {
            string serverPort   = portInputBox.Text;
            string ipAdress     = ipInputBox.Text;
            string userName     = nameInputBox.Text;

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
                MNetworkEntry.Instance.Initialize(
                    new ChattingCallback(),
                    new ChattingPacketTranslater());
                MNetworkEntry.Instance.Run("127.0.0.1", 9199);

                new Thread(delegate () { MNetworkEntry.Instance.Update(); }) { IsBackground = true }.Start();

                Connect.Enabled = false;
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

        private void SendButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SendContextBox.Text))
                return;

            ChattingRequestPacket packet = new ChattingRequestPacket
            {
                Sender = "CLIENT",
                Text = SendContextBox.Text
            };

            bool result = MNetworkEntry.Instance.Send(new ProtobufPacket<ChattingRequestPacket>(0, PacketEnum.ProcessType.Data,
                (int)MessageType.ChattingRequest, packet));

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
