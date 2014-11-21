using System;
using System.Diagnostics;
using System.IO;

namespace ConvertId3
{
	class Program
	{
		static void Main(string[] args)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(args[0]);
			if (!directoryInfo.Exists)
			{
				Console.WriteLine("{0} Can't be found.", args[0]);
				return;
			}

			string tempFileName;
			do
			{
				tempFileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			} while (File.Exists(tempFileName));

			FileInfo[] fileInfos = directoryInfo.GetFiles("*.mp3", SearchOption.AllDirectories);
			foreach (FileInfo f in fileInfos)
			{
				string orginalName = f.FullName;
				f.IsReadOnly = false;
				f.MoveTo(tempFileName);
				Process process = new Process();
				process.StartInfo.FileName = "java.exe";
				
				process.StartInfo.Arguments =
					"-jar id3iconv-0.2.1.zip -e gbk -removev1 \"" + f.FullName + "\"";
				process.StartInfo.UseShellExecute = false;
				Console.WriteLine("processing " + f.FullName);
				process.Start();
				process.WaitForExit();
				f.MoveTo(orginalName);
			}
		}
	}
}
