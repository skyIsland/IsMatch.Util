using System;
using static System.Console;
using System.IO;

namespace IsMatch.ScanCodeLine
{
    class Program
    {
        /*
         * Use the first argument as the diretory
         * to the search, or default to the current directory.
         */
        static void Main(string[] args)
        {
            int totalLineCount = 0, directoryNo = 0;
            var directory = args.Length > 0 ? args[0] : Directory.GetCurrentDirectory();

            // tips
            WriteLine("start scan..." + directory);
            Title = $"first directory: {directory}";
            WriteLine("------------------------------------------------------------------------------------------------\n");
            WriteLine($"NO\tCurrent Diretory");

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();           
            totalLineCount = DirectoryCountLines(directory,ref directoryNo);
            sw.Stop();

            WriteLine("\n------------------------------------------------------------------------------------------------");
            WriteLine("\ncompleted process...");
            WriteLine($"\nResult:{totalLineCount} Line,times:{sw.ElapsedMilliseconds}ms");
            ReadKey();
        }

        static int DirectoryCountLines(string diretory,ref int directoryNo)
        {
            directoryNo++;
            WriteLine($"{directoryNo}\t{diretory}");
            int lineCount = 0;

            string[] filesInDirectory = Directory.GetFiles(diretory, "*.cs");
            foreach (string file in filesInDirectory)
            {
                WriteSucLog($"\t   {file.Replace(diretory, "").TrimStart('\\')}");//\0
                lineCount += CoutLines(file);
            }

            string[] subDirectories = Directory.GetDirectories(diretory);
            foreach (string subDirectory in subDirectories)
            {
                lineCount += DirectoryCountLines(subDirectory, ref directoryNo);
            }
            return lineCount;
        }

        static int CoutLines(string file)
        {
            int lineCount = 0;
            try
            {
                using (FileStream fs = new FileStream(file, FileMode.Open))
                {
                    StreamReader reader = new StreamReader(fs);
                    var line = reader.ReadLine();

                    while (line != null)
                    {
                        if (line.Trim() != "")
                        {
                            lineCount++;
                        }
                        line = reader.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLine($"打开文件发生错误，原因:{ex.Message}\n");
            }
           
            return lineCount;
        }

        static void WriteSucLog(string msg)
        {
            ConsoleColor thatColor = ForegroundColor;
            ForegroundColor = ConsoleColor.Red;
            WriteLine(msg);
            ForegroundColor = thatColor;
        }
    }
}
