using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace NativeException
{
    class Program
    {
        [DllImport("NativeLib.dll")]
        public static extern void func();

        static void Main(string[] args)
        {
            try
            {
                func();
            }
            catch (Exception e)
            {
                Console.WriteLine("Something bad happens." + e.ToString());
            }
        }
    }
}
