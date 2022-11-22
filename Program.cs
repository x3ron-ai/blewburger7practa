using System.Diagnostics;
using System.Linq.Expressions;
using static _7pract.Functions;

namespace _7pract
{
    internal class Program
    {
        static decimal ConvertToGb(double bytes)
        {
            return Math.Round(Convert.ToDecimal(bytes / 1024 / 1024 / 1024), 2);
        }
        static void Main(string[] args)
        {
            List<Element> elements = new List<Element>();
            string directory = "\\";
            while (true)
            {
                Console.Clear();
                if (elements.Count == 0)
                {
                    DriveInfo[] allDrives = DriveInfo.GetDrives();
                    foreach (DriveInfo drive in allDrives)
                    {
                        decimal totalSpace = ConvertToGb(drive.TotalSize);
                        decimal availableSpace = ConvertToGb(drive.AvailableFreeSpace);
                        Element elem = new Element(true, $"Диск {drive.Name} | Размер {totalSpace} гб | Доступно {availableSpace} гб", drive.Name);
                        elements.Add(elem);
                    }
                }
                else
                {
                    elements = GetDirectory.Files(directory);
                }
                bool exit = false;
                Cursor cursor = new Cursor(1,1,1);
                while (!exit)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ResetColor();
                    Console.Write("Проводник! Директория: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(directory);
                    Console.ResetColor();
                    cursor.max = 0;
                    foreach (Element elem in elements)
                    {
                        if (elem.isFolder)
                            Console.ForegroundColor = ConsoleColor.Green;
                        else
                            Console.ResetColor();
                        cursor.max++;
                        Console.WriteLine("   "+elem.elName);
                    }
                    Console.SetCursorPosition(0, cursor.pos);
                    Console.Write(">>");
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.DownArrow:
                            Console.SetCursorPosition(0, cursor.pos);
                            Console.Write("  ");
                            if (cursor.pos == cursor.max)
                                cursor.pos = 1;
                            else
                                cursor.pos++;
                            break;
                        case ConsoleKey.UpArrow:
                            Console.SetCursorPosition(0, cursor.pos);
                            Console.Write("  ");
                            if (cursor.pos == 1)
                                cursor.pos = cursor.max;
                            else
                                cursor.pos--;
                            break;
                        case ConsoleKey.Enter:
                            if (elements[cursor.pos - 1].isFolder)
                            {
                                directory = elements[cursor.pos - 1].elPath;
                                if (directory == "\\")
                                    elements.Clear();
                                exit = true;
                            }
                            else
                            {
                                try {
                                    Process.Start(elements[cursor.pos - 1].elPath);
                                }
                                catch
                                {
                                    GetDirectory.FileOpenError();
                                }
                            }
                            break;
                        case ConsoleKey.F1:
                            Functions.SelectFunc(directory);
                            break;
                    }
                }
            }
        }
    }
}