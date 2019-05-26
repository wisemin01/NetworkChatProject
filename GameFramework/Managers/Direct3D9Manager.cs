using SharpDX.Windows;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GameFramework.Manager
{
    using D3D9      = SharpDX.Direct3D9;
    using BGRAColor = SharpDX.Mathematics.Interop.RawColorBGRA;
    
    public partial class Direct3D9Manager
    {
        public delegate void UpdateCallback();
        public delegate void RenderCallback();

        // Singleton
        private static Direct3D9Manager instance = null;

        public static Direct3D9Manager Instance
        {
            get
            {
                if (instance == null)
                    instance = new Direct3D9Manager();

                return instance;
            }
        }

        // Member
        private RenderMsgForm mainForm;

        private D3D9.Device d3d9Device;
        private D3D9.Sprite d3d9Sprite;

        private UpdateCallback OnUpdate;
        private RenderCallback OnRender;

        // Propertie

        public Device D3D9Device
        {
            get => d3d9Device;
        }

        public BGRAColor BackBufferColor { get; set; } = new BGRAColor(178, 103, 32, 255);

        public int WindowWidth
        {
            get => mainForm.Size.Width;
        }

        public int WindowHeight
        {
            get => mainForm.Size.Height;
        }
    }

    public partial class Direct3D9Manager
    {
        public void  CreateDirect3D9(string title, int width, int height)
        {
            mainForm = new RenderMsgForm(title)
            {
                ClientSize        = new System.Drawing.Size(width, height),
                AllowUserResizing = false
            };

            Direct3D d3d = new Direct3D();

            d3d9Device = new Device(
                d3d,
                0,
                DeviceType.Hardware,
                mainForm.Handle,
                CreateFlags.HardwareVertexProcessing,
                new PresentParameters(width, height));

            d3d9Sprite = new Sprite(d3d9Device);

            InitializeDirectInput();
        }

        public void  Exit()
        {
            
        }

        private void OnFrame()
        {
            // Key Update
            KeyUpdate();

            // Frame Update
            OnUpdate();

            // Rendering
            {
                d3d9Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer,
                    BackBufferColor, 1.0f, 0);

                d3d9Device.BeginScene();
                d3d9Sprite.Begin(SpriteFlags.AlphaBlend);

                OnRender();

                d3d9Sprite.End();
                d3d9Device.EndScene();
                d3d9Device.Present();
            }
        }

        public void  Run(UpdateCallback updateCallback, RenderCallback renderCallback)
        {
            OnUpdate = updateCallback;
            OnRender = renderCallback;

            RenderLoop.Run(mainForm, OnFrame);
        }

        public void  Release()
        {
            TextureDispose();
            FontDispose();

            directInput.Dispose();
            d3d9Sprite.Dispose();
            d3d9Device.Dispose();
            mainForm.Dispose();
        }

        public void  AddMessageHandler(EventHandler<Message> eventHandler)
        {
            mainForm.OnMessage += eventHandler;
        }

        public void  RemoveMessageHandler(EventHandler<Message> eventHandler)
        {
            mainForm.OnMessage -= eventHandler;
        }
    }
}
