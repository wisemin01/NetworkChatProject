using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MNetwork.Utility;

namespace ChattingNetwork.Server
{
    public struct UserData
    {
        public string ID;
        public string Password;
        public string UserName;
    }

    public class LoginHandler
    {
        /// <summary>
        /// 회원 가입 요청을 보냅니다.
        /// </summary>
        /// <param name="id"> 유저의 ID 정보입니다. </param>
        /// <param name="password"> 유저의 비밀번호 입니다. </param>
        /// <param name="userName"> 유저의 이름입니다. </param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool SignUp(string id, string password, string userName, out string context)
        {
            INIFile.Get(id, out string value, "./Data/userData.ini", "USER_DATA");

            if (value == "FAILED")
            {
                INIFile.Set(id, $"{userName}/{password}", "./Data/userData.ini", "USER_DATA");
                context = "회원가입에 성공했습니다.";
                return true;
            }
            else
            {
                context = "이미 해당 ID 가 존재합니다.";
                return false;
            }
        }

        public bool SignIn(string id, string password, out string userName)
        {
            INIFile.Get(id, out string value, "./Data/userData.ini", "USER_DATA");

            if (value == "FAILED")
            {
                userName = "NONE";
                return false;
            }

            string[] devideResult = value.Split(new char[] { '/' });

            string name = devideResult[0];
            string pw = devideResult[1];

            if (pw != password)
            {
                userName = "NONE";
                return false;
            }

            userName = name;
            return true;
        }

        public void Reset(string path)
        {
            throw new NotImplementedException();
        }
    }
}
