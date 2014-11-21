using System;
using System.Diagnostics;
using System.Reflection;

namespace LogHelper
{
    class LogHelper
    {
        public static string CallingMethod()
        {
            // NOTE: You can't get the frame in production enviroment because of pdb files.
            // So normally you can't build a helper function like this. :(
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();
            return string.Format("[{0}.{1}]", methodBase.DeclaringType, methodBase.Name);
        }
    }

    class LogHelperTest
    {
        static void Main(string[] args)
        {
            WhatsMyName();
        }

        private static void WhatsMyName()
        {
            InnerMethod();
        }

        private static void InnerMethod()
        {
            // NOTE: It is ALWAYS run even without pdb files.
            // StackFrame knows the current frame well. :)
            StackFrame stackFrame = new StackFrame();
            Console.WriteLine(stackFrame.GetMethod().Name);
            Console.WriteLine(LogHelper.CallingMethod());
        }
    }
}
