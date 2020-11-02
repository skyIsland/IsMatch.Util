using IsMatch.DesignPatterns._01SimpleFactory;
using IsMatch.DesignPatterns._02FactoryMethod;
using System;

namespace IsMatch.DesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // 简单工厂模式
            TestSimpleFactory();

            // 工厂方法模式
            TestFactoryMethod();

            Console.ReadKey();
        }

        static void TestSimpleFactory()
        {
            var factory = new Factory();

            var mutsu = factory.CreateBattleShip(nameof(Mutsu));
            var nagato = factory.CreateBattleShip(nameof(Nagato));
        }

        static void TestFactoryMethod()
        {
            var mutsu = new MutsuFactory().Create();
            var nagato = new NagatoFactory().Create();
        }
    }
}
