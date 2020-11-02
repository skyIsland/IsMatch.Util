using System;
using System.Collections.Generic;
using System.Text;

namespace IsMatch.DesignPatterns._01SimpleFactory
{
    /// <summary>
    /// 具体产品陆奥
    /// </summary>
    public class Mutsu : BattleShip
    {
        /// <summary>
        /// 构造函数（这里只是做个输出提示）
        /// </summary>
        public Mutsu()
        {
            Console.WriteLine("我是具体产品Mutsu。");
        }
    }
}
