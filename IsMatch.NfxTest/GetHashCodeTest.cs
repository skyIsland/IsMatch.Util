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
        public int X { get; set; }

        public int Y { get; set; }

        public override int GetHashCode()
        {
            Debug.WriteLine(base.GetHashCode());

            //return (this.X * 5) * (this.Y * 10);

            return new Random().Next(1, 22);
        }

        public override bool Equals(object obj)
        {
            GetHashCodeTest gct = obj as GetHashCodeTest;

            //if(gct.X == this.X && gct.Y == this.Y)
            //{
            //    return true;
            //}
            if(this.GetHashCode() == gct.GetHashCode())
            {
                return true;
            }

            return false;
        }
    }
}
