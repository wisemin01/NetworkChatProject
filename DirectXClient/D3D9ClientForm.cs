using GameFramework.Manager;
using System;

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
        }

        public void Run()
        {
            D3D9Manager.Instance.Run(Update, Render);
        }

        void Update()
        {
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
