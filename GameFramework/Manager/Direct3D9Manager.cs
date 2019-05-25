using SharpDX.Windows;
using SharpDX.Direct3D9;

namespace GameFramework.Manager
{
    using D3D9 = SharpDX.Direct3D9;

    class Direct3D9Manager
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
        private RenderForm  renderForm;

        private D3D9.Device d3d9Device;
        private Sprite      d3d9Sprite;

        // Propertie

        public D3D9.Device D3D9Device
        {
            get => d3d9Device;
        }

        // Function

        public Direct3D9Manager()
        {

        }

        private void RenderCallback()
        {

        }

        public void Run()
        {
            RenderLoop.Run(renderForm, RenderCallback);
        }

        public void Release()
        {
            renderForm.Dispose();
        }
    }
}
