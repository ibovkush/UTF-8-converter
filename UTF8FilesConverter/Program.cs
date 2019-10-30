using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTF8FilesConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length<2)
            {
                Console.Write($"The absolute path and at least one extension requared.");
                Console.ReadKey();
                return;
            }

            var directory = new DirectoryInfo(args[0]);

            if (!directory.Exists)
            {
                Console.Write($"Can't find directory {args[0]}");
                Console.ReadKey();
                return;
            }

            var exts = new List<string>();
            for (int i = 1; i < args.Length; i++)
            {
                exts.Add($"*.{args[i]}");
            }
            var extsstr = string.Join(",", exts);

            Console.Write($"The ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(extsstr);
            Console.ResetColor();
            Console.Write($" files in ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(directory.Name);
            Console.ResetColor();
            Console.Write($" and sub directories will be converted to UTF-8. Continue? [y/n]");

            var key = Console.ReadKey();
            if (key.KeyChar.ToString().ToLower() == "n")
            {
                Console.WriteLine();
                Console.WriteLine("Closing...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine();
            Console.WriteLine("Converting files...");
            var count = 0;
            var errors = 0;

            foreach (var f in directory.EnumerateFiles("*.*", SearchOption.AllDirectories)
            .Where(file => extsstr.Contains(file.Extension))
            )
            {
                Console.WriteLine();
                Console.ResetColor();
                Console.Write("Converting ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(f.FullName.Replace(directory.FullName, $".."));
                string s = File.ReadAllText(f.FullName);
                try
                {
                    File.WriteAllText(f.FullName, s, Encoding.UTF8);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($" [OK]");
                    count++;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($" [Error]");
                    Console.ResetColor();
                    Console.WriteLine($"Error: {ex.Message}");
                    errors++;
                }
            }
            Console.ResetColor();
            Console.WriteLine();
            Console.Write($"Done.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($" [{count}] ");
            Console.ResetColor();
            Console.Write($"files converted.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($" [{errors}] ");
            Console.ResetColor();
            Console.Write($"errors occurred.");
            Console.WriteLine();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
