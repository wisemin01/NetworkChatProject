using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameFramework;
using GameFramework.Manager;
using GameFramework.Structure;
using SharpDX;

using ChattingNetwork;
using ChattingNetwork.Client;

namespace DirectXClient
{
    internal partial class SignUpScene
        : Scene
    {
        public SignUpScene()
        {

        }

        public override void Initialize()
        {
            D3D9Manager.Instance.CreateTexture("SignupButton", "./Resource/SignupButton.png");
            D3D9Manager.Instance.CreateTexture("NameInput", "./Resource/NameInput.png");
            D3D9Manager.Instance.CreateTexture("BackButton", "./Resource/BackButton.png");
            D3D9Manager.Instance.CreateTexture("Background", "./Resource/Background2.png");

            D3D9Manager.Instance.CreateFont("InputField", "Segoe UI", 35, false);

            ClientManager.Instance.OnSignUp += OnSignUp;

            InitializeComponent();
        }

        public void ToSignInButton_OnClick(object sender, EventArgs e)
        {
            SceneManager.Instance.ChangeScene("SignIn");
        }

        private void SignUpButton_OnClick(object sender, EventArgs e)
        {
            bool result = ClientManager.Instance.SignUp(idInput.Text, passwordInput.Text, userNameInput.Text);

            if (result == true)
            {
                idInput.Clear();
                passwordInput.Clear();
                userNameInput.Clear();
            }
        }

        private void OnSignUp(object sender, Tuple<string, bool> e)
        {
            if (e.Item2 == true)
            {
                MessageBox.Show($"{e.Item1}", "성공");
            }
            else
            {
                MessageBox.Show($"{e.Item1}", "실패");
            }
        }

        public override void FrameRender()
        {
        }

        public override void FrameUpdate()
        {
        }

        public override void Release()
        {
            Dispose();

            D3D9Manager.Instance.FontDispose();
            D3D9Manager.Instance.TextureDispose();

            ClientManager.Instance.OnSignUp -= OnSignUp;
        }
    }
}
