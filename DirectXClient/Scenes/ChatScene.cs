using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using GameFramework.Manager;
using SharpDX;
using TCPNetwork.Client;

namespace DirectXClient
{
    public class ChatScene : Scene
    {
        public ChatScene() : base() { }

        public override void Initialize()
        {
            Direct3D9Manager.Instance.CreateFont("Font1", "Fixedsys", 50, false);

            var TextList = GameObjectManager.Instance.AddObject(
                new TextList(50,
                new Vector3(
                    0,
                    Direct3D9Manager.Instance.WindowHeight - 60,
                    0
                    ),
                "Font1"));

            NetworkClientManager.Instance.Initialize(
                "DirectXClient User", "127.0.0.1", 9199, TextList,
                delegate (string text) { Direct3D9Manager.Instance.Exit(); });
            NetworkClientManager.Instance.ConnectToServer();
        }
        public override void FrameRender()
        {
            GameObjectManager.Instance.RenderObjects();
        }

        public override void FrameUpdate()
        {
            GameObjectManager.Instance.UpdateObjects();
        }

        public override void Release()
        {
            GameObjectManager.Instance.ReleaseObjects();
        }
    }
}
