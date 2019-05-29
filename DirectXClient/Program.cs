using System;

namespace DirectXClient
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (D3D9ClientForm clientForm = new D3D9ClientForm())
            {
                clientForm.Initialize();
                clientForm.Run();
            }
        }
    }
}
