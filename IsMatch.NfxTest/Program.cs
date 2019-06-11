using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsMatch.NfxTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();

            Console.ReadKey();
        }

        private static void Test1()
        {
            AssemblyTest.GetEntityType("MailHelper");
        }
    }
}
