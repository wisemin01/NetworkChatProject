using GameFramework.Manager;
using System;
using MNetwork.Engine;
using MNetwork.Debuging;

using ChattingNetwork;
using ChattingNetwork.Client;
using MNetwork.Utility;
using MNetwork.Exceptions;
using System.Windows.Forms;

namespace DirectXClient
{
    public class ClientWindow
    {
        public const int Width = 1280;
        public const int Height = 720;
    }

    class D3D9ClientForm : IDisposable
    {
        public void Initialize()
        {
            D3D9Manager.Instance.CreateDirect3D9("My DirectX Client",
                ClientWindow.Width, ClientWindow.Height);

            SceneManager.Instance.AddScene<SignInScene>("SignIn");
            SceneManager.Instance.AddScene<SignUpScene>("SignUp");
            SceneManager.Instance.AddScene<LobbyScene>("Lobby");
            SceneManager.Instance.AddScene<ChatScene>("Chat");

            SceneManager.Instance.ChangeScene("SignIn");

            INIFile.Get("server_ip", out string ip, "./Data/clientinfo.ini", "CLIENT");
            INIFile.Get("server_port", out string port, "./Data/clientinfo.ini", "CLIENT");

            ClientManager.Instance.Connect(ip, ushort.Parse(port));

            ClientManager.Instance.OnConnect += OnServerConnect;
            ClientManager.Instance.OnDisconnect += OnServerDisconnect;

            Debug.Log("This client runs on a Direct X client.");
        }

        public void Run()
        {
            D3D9Manager.Instance.Run(Update, Render);
        }

        void Update()
        {
            ClientManager.Instance.Update();
            SceneManager.Instance.FrameUpdate();
        }

        void Render()
        {
            SceneManager.Instance.FrameRender();
        }

        void IDisposable.Dispose()
        {
            SceneManager.Instance.Release();
        }

        private void OnServerConnect(object sender, bool value)
        {
            if (value == false)
            {
                MessageBox.Show("서버 접속에 실패했습니다.\n클라이언트를 재시작 해주세요.", "CONNECTION FAILED")
                    .OnClosing += delegate { Application.Exit(); };
            }
        }

        private void OnServerDisconnect(object sender, EventArgs e)
        {
            MessageBox.Show("서버와의 연결이 끊어졌습니다.\n클라이언트를 재시작 해주세요.", "DISCONNECTED")
                 .OnClosing += delegate { Application.Exit(); };
        }
    }
}
