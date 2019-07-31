using GameFramework.Manager;
using System;
using MNetwork.Engine;
using MNetwork.Debuging;

using ChattingNetwork;
using ChattingNetwork.Client;
using MNetwork.Utility;

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
    }
}
