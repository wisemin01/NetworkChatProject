﻿using MNetwork.Debuging;
using MNetwork.Utility;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ServerHost
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServerGUIForm());
        }
    }
}
