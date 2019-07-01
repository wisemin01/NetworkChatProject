using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNetwork.Time
{
    public class Time
    {
        static public DateTime Now
        {
            get => DateTime.Now;
        }

        static public string TimeLog
        {
            get => DateTime.Now.ToString();
        }

        /// <summary>
        /// 현재 시간을 ( 시간 : 분 : 초 ) 형식 string 으로 반환합니다.
        /// </summary>
        static public string TimeLogHMS
        {
            get => DateTime.Now.ToString("hh:mm:ss");
        }
    }
}
