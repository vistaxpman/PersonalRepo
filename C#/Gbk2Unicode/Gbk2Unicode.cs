using System;
using System.IO;
using System.Text;

namespace Gbk2Unicode
{
   class Program
   {
      static void Main(string[] args)
      {
         if (args.Length != 2)
         {
            Console.WriteLine("Usage: Gbk2Unicode dir file");
            return;
         }
         
         DirectoryInfo directoryInfo = new DirectoryInfo(args[0]);
         if (!directoryInfo.Exists)
         {
            Console.WriteLine("Can't find the specified directory!");
            return;
         }

         FileInfo[] fileInfos = directoryInfo.GetFiles(args[1], SearchOption.AllDirectories);
         Encoding gbkEncoding = Encoding.GetEncoding("GBK");
         Encoding unicodeEncoding = Encoding.Unicode;
         foreach (FileInfo fileInfo in fileInfos)
         {
            Console.WriteLine("processing " + fileInfo.FullName);
            byte[] gbkBytes;
            using (BinaryReader reader =
                new BinaryReader(new FileStream(fileInfo.FullName, FileMode.Open), gbkEncoding))
            {
               gbkBytes = reader.ReadBytes((int)fileInfo.Length);
            }
            using (StreamWriter writer =
                    new StreamWriter(fileInfo.FullName))
            {
               byte[] unicodeBytes = Encoding.Convert(gbkEncoding, unicodeEncoding, gbkBytes);
               char[] unicodeChars = new char[unicodeEncoding.GetCharCount(unicodeBytes, 0, unicodeBytes.Length)];
               unicodeEncoding.GetChars(unicodeBytes, 0, unicodeBytes.Length, unicodeChars, 0);
               string unicodeString = new string(unicodeChars);
               writer.Write(unicodeString);
            }
         }
      }
   }
}
