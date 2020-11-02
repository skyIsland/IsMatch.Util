using System;
using System.Collections.Generic;
using System.Text;

namespace IsMatch.DesignPatterns._01SimpleFactory
{
    /// <summary>
    /// 具体工厂 根据参数生成对应的产品类(返回的是基类)
    /// </summary>
    public class Factory
    {
        public BattleShip CreateBattleShip(string type)
        {
            BattleShip result = null;
            switch (type)
            {
                case nameof(Mutsu):
                    result = new Mutsu();
                    break;
                case nameof(Nagato):
                    result = new Nagato();
                    break;
                default:
                    throw new ArgumentException("参数错误，生成产品失败哟。");
            }
            return result;
        }
    }
}
