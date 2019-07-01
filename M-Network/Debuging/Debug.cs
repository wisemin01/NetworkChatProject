using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;

namespace MNetwork.Debuging
{
    public partial class Debug
    {
        public const int logCapacity = 128;

        public static event EventHandler<string> OnLog;

        private static readonly object locker = new object();

        private static List<string> logList = new List<string>(logCapacity);

        /// <summary> 파일에 로그를 쓸 것인가 </summary>
        public static bool IsWriteOnFile { get; set; } = true;
        /// <summary> 콘솔에 로그를 쓸 것인가 </summary>
        public static bool IsWriteOnConsole { get; set; } = true;

        /// <summary> 로그 출력 경로 ( 파일 전용 ) </summary>
        public static string LogPath { get; set; } = $"./Log/Log.txt";

        /// <summary> 기본적인 로그를 남깁니다. </summary>
        public static void Log(string context)
        {
            string log = $"[LOG]<Thread:{Thread.CurrentThread.ManagedThreadId}>[{Time.Time.TimeLogHMS}] : {context}";

            if (IsWriteOnConsole) WriteToConsole(log);
            if (IsWriteOnFile) WriteToFile(log);

            OnLog?.Invoke(null, log);
        }

        /// <summary> 경고 로그를 남깁니다. </summary>
        public static void WarningLog(string context)
        {
            string log = $"[WARNING] <Thread ID : {Thread.CurrentThread.ManagedThreadId}> [{Time.Time.TimeLogHMS}] : {context}";

            if (IsWriteOnConsole) WriteToConsole(log);
            if (IsWriteOnFile) WriteToFile(log);

            OnLog?.Invoke(null, log);
        }

        /// <summary> 에러 로그를 남깁니다. </summary>
        public static void ErrorLog(string context)
        {
            string log = $"[ERROR] <Thread ID : {Thread.CurrentThread.ManagedThreadId}> [{Time.Time.TimeLogHMS}] : {context}";

            if (IsWriteOnConsole) WriteToConsole(log);
            if (IsWriteOnFile) WriteToFile(log);

            OnLog?.Invoke(null, log);
        }

        /// <summary> condition이 false 이면 호출 스택을 보여주는 메시지 상자를 표시합니다. </summary>
        public static void Assert(bool condition)
        {
            System.Diagnostics.Debug.Assert(condition);
        }

        /// <summary>
        /// 지정된 로그 파일 경로에 메모리에 저장된 로그를 저장하고
        /// 저장된 로그를 비웁니다.
        /// </summary>
        public static bool Flush()
        {
            try
            {
                lock (locker)
                {
                    System.IO.StreamWriter stream = new System.IO.StreamWriter(LogPath, true);

                    foreach (string context in logList)
                    {
                        stream.WriteLine(context);
                    }

                    stream.Close();
                    logList.Clear();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 지정된 로그 파일 경로에 저장된 로그를 모두 비웁니다.
        /// </summary>
        public static bool ClearLogFile()
        {
            try
            {
                lock (locker)
                {
                    System.IO.StreamWriter stream = new System.IO.StreamWriter(LogPath, false);

                    stream.Close();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// logList 에 context를 추가합니다.
        /// logList 가 가득 찼다면 자동으로 로그들을 파일에 Flush() 합니다.
        /// </summary>
        private static bool WriteToFile(string context)
        {
            logList.Add(context);

            if (logList.Count >= logCapacity - 1)
            {
                return Flush();
            }

            return true;
        }

        /// <summary> 콘솔에 context를 씁니다. </summary>
        private static void WriteToConsole(string context)
        {
            Console.WriteLine(context);
        }
    }

    public partial class Debug
    {
        public static void Log(MethodBase methodBase, string context)
        {
            Log($"{methodBase.ReflectedType.FullName}.{methodBase.Name}() : {context}");
        }

        public static void WarningLog(MethodBase methodBase, string context)
        {
            WarningLog($"{methodBase.ReflectedType.FullName}.{methodBase.Name}() : {context}");
        }

        public static void ErrorLog(MethodBase methodBase, string context)
        {
            ErrorLog($"{methodBase.ReflectedType.FullName}.{methodBase.Name}() : {context}");
        }
    }
}