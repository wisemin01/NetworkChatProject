using SharpDX.Windows;
using SharpDX.Direct3D9;

namespace GameFramework.Manager
{
    using D3D9 = SharpDX.Direct3D9;
    using BGRAColor = SharpDX.Mathematics.Interop.RawColorBGRA;

    public partial class Direct3D9Manager
    {
        public delegate void UpdateCallback();
        public delegate void RenderCallback();
    }

    public partial class Direct3D9Manager
    {
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
        private RenderForm renderForm;

        private D3D9.Device d3d9Device;
        private D3D9.Sprite d3d9Sprite;

        private UpdateCallback OnUpdate;
        private RenderCallback OnRender;

        private BGRAColor backBufferColor = new BGRAColor(178, 103, 32, 255);

        // Propertie

        public Device D3D9Device
        {
            get => d3d9Device;
        }

        public BGRAColor BackBufferColor
        {
            get => backBufferColor;
            set => backBufferColor = value;
        }

        public int WindowWidth
        {
            get => renderForm.Size.Width;
        }

        public int WindowHeight
        {
            get => renderForm.Size.Height;
        }

        // Function

        public void CreateRenderForm(string title, int width, int height)
        {
            renderForm = new RenderForm(title)
            {
                ClientSize        = new System.Drawing.Size(width, height),
                AllowUserResizing = false
            };

            Direct3D d3d = new Direct3D();

            d3d9Device = new Device(
                d3d,
                0,
                DeviceType.Hardware,
                renderForm.Handle,
                CreateFlags.HardwareVertexProcessing,
                new PresentParameters(width, height));

            d3d9Sprite = new Sprite(d3d9Device);
        }

        public void Exit()
        {
            Release();
        }

        private void OnFrame()
        {
            // Frame Update
            OnUpdate();

            // Rendering
            {
                d3d9Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer,
                    backBufferColor, 1.0f, 0);

                d3d9Device.BeginScene();
                d3d9Sprite.Begin(SpriteFlags.AlphaBlend);

                OnRender();

                d3d9Sprite.End();
                d3d9Device.EndScene();
                d3d9Device.Present();
            }
        }

        public void Run(UpdateCallback updateCallback, RenderCallback renderCallback)
        {
            OnUpdate = updateCallback;
            OnRender = renderCallback;

            RenderLoop.Run(renderForm, OnFrame);
        }

        public void Release()
        {
            FontDispose();

            d3d9Sprite.Dispose();
            d3d9Device.Dispose();
            renderForm.Dispose();
        }
    }
}
