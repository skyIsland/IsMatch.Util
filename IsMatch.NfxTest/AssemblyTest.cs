using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IsMatch.NfxTest
{
    public static class AssemblyTest
    {
        public static string DefaultEntityName { get; private set; }

        static AssemblyTest()
        {
            DefaultEntityName = "IsMatch.CnArticleSubscribe";
        }

        public static string GetEntityType(string entityName)
        {
            var type = Assembly.Load(DefaultEntityName).GetType(entityName);

            if(type == null)
            {
                throw new ArgumentException(nameof(entityName));
            }

            return type.ToString();
        }
    }
}
