using System;
using GameFramework.Manager;
using SharpDX;
using TCPNetwork.Client;

namespace DirectXClient
{
    class D3D9ClientForm : IDisposable
    {
        public void Initialize()
        {
            Direct3D9Manager.Instance.CreateRenderForm("My DirectX Client", 1280, 720);

            SceneManager.Instance.AddScene<ChatScene>("Lobby");
            SceneManager.Instance.ChangeScene("Lobby");
        }

        public void Run()
        {
            Direct3D9Manager.Instance.Run(Update, Render);
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
