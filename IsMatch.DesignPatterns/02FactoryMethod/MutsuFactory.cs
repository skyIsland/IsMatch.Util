using IsMatch.DesignPatterns._01SimpleFactory;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsMatch.DesignPatterns._02FactoryMethod
{
    public class MutsuFactory : AbstractFactory
    {
        public override BattleShip Create()
        {
            return new Mutsu();
        }
    }
}
