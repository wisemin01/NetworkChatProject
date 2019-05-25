using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
