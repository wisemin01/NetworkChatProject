using GameFramework.Manager;
using System;
using MNetwork.Engine;
using MNetwork.Debuging;

using ChattingNetwork;
using ChattingNetwork.Client;

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

            SceneManager.Instance.AddScene<LoginScene>("Login");
            SceneManager.Instance.AddScene<LobbyScene>("Lobby");
            SceneManager.Instance.AddScene<ChatScene>("Chat");

            SceneManager.Instance.ChangeScene("Login");

            ClientManager.Instance.Connect("127.0.0.1", 9199);
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
