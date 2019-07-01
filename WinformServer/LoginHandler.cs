using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MNetwork.Utility;

namespace ServerHost
{
    public struct UserData
    {
        public string ID;
        public string Password;
        public string UserName;
    }

    public class LoginHandler
    {
        public bool SignUp(string id, string password, string userName, out string context)
        {
            StringBuilder iniOutput = new StringBuilder();

            IniHelper.GetPrivateProfileString("UserData", id, string.Empty, iniOutput, 128, "./Data/userData.ini");

            context = "";

            return true;
        }

        public bool Login(string id, string password, out string userName)
        {
            userName = "";
            return true;
        }
    }
}
