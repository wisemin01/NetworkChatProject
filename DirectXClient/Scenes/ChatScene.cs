using System;
using GameFramework;
using GameFramework.Manager;
using SharpDX;
using TCPNetwork.Client;
using SharpDX.DirectInput;
using GameFramework.Structure;

namespace DirectXClient
{
    public class ChatScene : Scene
    {
        public ChatScene() : base() { }

        public override void Initialize()
        {
            D3D9Manager.Instance.CreateTexture("ChatInput", "./Resource/ChatInput.png");
            D3D9Manager.Instance.CreateFont("ChatListFont", "메이플스토리 Light", 25, false);
            D3D9Manager.Instance.CreateFont("ChatInputFont", "메이플스토리 Light", 35, false);
            
            var TextList = GameObjectManager.Instance.AddObject(new TextList(
                25, new Vector3(30, D3D9Manager.Instance.WindowHeight - 160, 0),
                "ChatListFont"));

            NetworkClientManager.Instance.TextDraw = TextList;

            var TextInput = GameObjectManager.Instance.AddObject(new TextInputField("ChatInputFont")
            {
                Position        = new Vector3(404, D3D9Manager.Instance.WindowHeight - 120, 0),
                FieldTexture    = D3D9Manager.Instance.FindTexture("ChatInput"),
                MaxLength       = 35,
                StringColor     = new Color(127, 127, 127),
                StringOffset    = new Vector3(16, 3, 0)
            });

            TextInput.OnEnter += delegate (object sender, string s) {
                NetworkClientManager.Instance.SendMessageToServer(s);
                TextInputField input = sender as TextInputField;
                input.Clear();
            };

            GameObjectManager.Instance.AddObject(new StateObserver());
        }

        public override void FrameRender()
        {
        }

        public override void FrameUpdate()
        {
            if (NetworkClientManager.Instance.IsConnection == false)
            {
                SceneManager.Instance.ChangeScene("Login");
            }
        }

        public override void Release()
        {
            D3D9Manager.Instance.FontDispose();
            D3D9Manager.Instance.TextureDispose();
        }
    }
}
