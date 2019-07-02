using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsMatch.NfxTest
{
    public class StaticClass
    {
        private static string Str { get; set; }

        static StaticClass()
        {
            Str = "静态构造Str";
        }
          
        public StaticClass(string str)
        {
            Str = str;
        }

        public static string Result()
        {
            return Str;
        }
    }
}
