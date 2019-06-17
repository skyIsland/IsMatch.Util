using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsMatch.NfxTest
{
    public class GetHashCodeTest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override int GetHashCode()
        {
            Debug.WriteLine(base.GetHashCode());
            return new Random().Next(5, 10);
        }
    }
}
