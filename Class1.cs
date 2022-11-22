using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _7pract
{
    public class Cursor
    {
        public Cursor(int Min, int Max, int Pos)
        {
            min = Min;
            max = Max;
            pos = Pos;
        }
        public int pos;
        public int min;
        public int max;
    }
    public class Element
    {
        public Element(bool IsFolder, string ElName, string ElPath)
        {
            elName = ElName;
            isFolder = IsFolder;
            elPath = ElPath;
        }
        public string elPath;
        public bool isFolder;
        public string elName;
        public double size;
    }
    public static class Functions
    {
        private static void AddFile(string path)
        {
            File.Create(path);
        }
        private static void AddDir(string path)
        {
            Directory.CreateDirectory(path);
        }
        private static void RemoveFile(string path)
        {
            File.Delete(path);
        }
        private static void RemoveDir(string path)
        {
            // try to catch recursion here!
            foreach (string file in Directory.GetFiles(path))
            {
                File.Delete(file);
            }
            foreach (string dir in Directory.GetDirectories(path))
            {
                RemoveDir(dir);
            }
        }
        public static void SelectFunc(string directory)
        {
            string[] menu = { "Новый файл", "Новая папка", "Удалить файл", "Удалить папку", "Отмена" };
            Cursor cursor = new Cursor(1, 0, 1);
            bool exit = false;
            while (!exit)
            {
                cursor.max = 0;
                Console.WriteLine("Выбор действия");
                foreach (string func in menu)
                {
                    cursor.max++;
                    Console.WriteLine("   " + func);
                }
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
                        switch (cursor.pos)
                        {
                            case 1:
                                Console.Clear();
                                Console.WriteLine("Имя файла и его расширение: ");
                                string fileName = Console.ReadLine();
                                AddFile(directory+"\\"+fileName);
                                break;
                        }
                        break;
                }
            }
        }
    }
    public static class GetDirectory
    {
        private static Element GetParent(string directory)
        {
            DirectoryInfo parent = Directory.GetParent(directory);
            if (parent != null)
                return new Element(true, "На уровень выше", parent.FullName);
            else
                return new Element(true, "К выбору диска", "\\");
        }
        public static List<Element> Files(string directory)
        {
            List<Element> elements = new List<Element>();
            DirectoryInfo parent = Directory.GetParent(directory);
            elements.Add(GetParent(directory));
            string[] files = Directory.GetFiles(directory);
            string[] dirs = Directory.GetDirectories(directory);
            foreach (string dir in dirs)
            {
                Element elem = new Element(true, dir.Split("\\")[^1] + '\\', dir);
                elements.Add(elem);
            }
            foreach (string file in files)
            {
                Element elem = new Element(false, file.Split("\\")[^1], file);
                elements.Add(elem);
            }
            return elements;
        }
        public static void FileOpenError()
        {
            Console.Clear();
            Console.Write("Ошибка при открытии файла! Окно закроется через 2 секунды");
            Thread.Sleep(1000);
            Console.Clear();
            Console.Write("Ошибка при открытии файла! Окно закроется через 1 секунду");
            Thread.Sleep(1000);
        }
    }
}
