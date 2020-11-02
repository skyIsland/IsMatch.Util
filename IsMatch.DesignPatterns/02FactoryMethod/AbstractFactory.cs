using IsMatch.DesignPatterns._01SimpleFactory;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsMatch.DesignPatterns._02FactoryMethod
{
    /// <summary>
    /// 抽象工厂
    /// </summary>
    public abstract class AbstractFactory
    {
        /// <summary>
        /// 生成产品
        /// </summary>
        /// <returns></returns>
        public abstract BattleShip Create();
    }
}
