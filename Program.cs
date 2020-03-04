using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VrSharp;
namespace ROFSDecode
{
    public static class Extensions
    {
        public static string GetCString(this byte[] file, int address, Encoding encoding)
        {
            int count = 0;
            while (file[address + count] != 0)
                count++;
            return encoding.GetString(file, address, count);
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            byte[] filelist = File.ReadAllBytes("FILELIST.DIR");
            int index = 0;
            string programPath = Environment.CurrentDirectory + "\\";
            string currentFolder = programPath;
            
            while (true)
            {
                string path = filelist.GetCString(index, Encoding.Default);
                path.Replace("/", "\\");
                index += path.Length+ 1;
                if (path.Length == 0)
                    break;
                
                if(Path.HasExtension(path))
                {
                    string placeholderFilename = filelist.GetCString(index, Encoding.Default);
                    Console.WriteLine("renaming " + placeholderFilename + " to " + path + " at " + currentFolder);
                    index += placeholderFilename.Length + 1;
                    path = currentFolder + "\\" +  path;
                    File.Move(placeholderFilename, path);
                }
                else
                {
                    path = programPath + path;
                    currentFolder = path;
                    Console.WriteLine("Entered folder " + currentFolder);
                    Directory.CreateDirectory(path);
                    index++;
                }
            }
            Console.ReadKey();
        }
    }
}
