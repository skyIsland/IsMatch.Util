using System;
using static System.Console;
using System.IO;
namespace IsMatch.Util
{
    class Program
    {
        /*
         * Use the first argument as the diretory
         * to the search, or default to the current directory.
         */
        static void Main(string[] args)
        {
            int totalLineCount = 0, DirectoryNo = 0;
            string directory;
            if (args.Length > 0)
            {
                directory = args[0];
            }
            else
            {
                directory = Directory.GetCurrentDirectory();
            }

            // tips
            WriteLine("start scan..." + directory);
            Title = $"first directory: {directory}";
            WriteLine("------------------------------------------------------------------------------------------------\n");
            WriteLine($"NO\tCurrent Diretory");

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();           
            totalLineCount = DirectoryCountLines(directory,ref DirectoryNo);
            sw.Stop();

            WriteLine("\n------------------------------------------------------------------------------------------------");
            WriteLine("\ncompleted process...");
            WriteLine($"\nResult:{totalLineCount} Line,times:{sw.ElapsedMilliseconds}ms");
            ReadKey();
        }

        static int DirectoryCountLines(string diretory,ref int DirectoryNo)
        {
            DirectoryNo++;
            WriteLine($"{DirectoryNo}\t{diretory}");
            int lineCount = 0;

            string[] filesInDirectory = Directory.GetFiles(diretory, "*.cs");
            foreach (string file in filesInDirectory)
            {
                lineCount += CoutLines(file);
            }

            string[] subDirectories = Directory.GetDirectories(diretory);
            foreach (string subDirectory in subDirectories)
            {
                lineCount += DirectoryCountLines(subDirectory, ref DirectoryNo);
            }
            return lineCount;
        }

        static int CoutLines(string file)
        {
            string line;
            int lineCount = 0;
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                StreamReader reader = new StreamReader(fs);
                line = reader.ReadLine();

                while (line != null)
                {
                    if(line.Trim() != "")
                    {
                        lineCount++;
                    }
                    line = reader.ReadLine();
                }
            }
            return lineCount;
        }
    }
}
