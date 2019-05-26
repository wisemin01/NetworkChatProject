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
            Direct3D9Manager.Instance.CreateDirect3D9("My DirectX Client", 1280, 720);
          
            SceneManager.Instance.AddScene<LoginScene>("Login");
            SceneManager.Instance.AddScene<ChatScene> ("Lobby");
            SceneManager.Instance.AddScene<ChatScene> ("Room");

            SceneManager.Instance.ChangeScene("Login");
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
