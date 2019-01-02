using System;

namespace IsMatch.Spider
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var courseSource = new CourseSource();
            courseSource.Work();
            Console.ReadKey();
        }
    }
}
