using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace SampleMsbuildTask
{
    public class UpdateCSharpVersion : Task
    {
        [Required]
        public string VersionFile { get; set; }

        [Required]
        public int Major { get; set; }

        [Required]
        public int Minor { get; set; }

        [Required]
        public int Revision { get; set; }

        [Required]
        public int BuildNumber { get; set; }

        public override bool Execute()
        {
            if (!File.Exists(VersionFile))
            {
                Log.LogError("The version file doesn't exist.");
                return false;
            }

            string versionFileContent = File.ReadAllText(VersionFile);
            var version = string.Format("{0}.{1}.{2}.{3}", Major, Minor, Revision, BuildNumber);

            versionFileContent = Regex.Replace(versionFileContent,
                @"(?<=\[assembly:\s*AssemblyVersion\("")\d*\.\d*\.\d*\.\d*", version);
            versionFileContent = Regex.Replace(versionFileContent,
                @"(?<=\[assembly:\s*AssemblyFileVersion\("")\d*\.\d*\.\d*\.\d*", version);

            File.WriteAllText(VersionFile, versionFileContent);

            return true;
        }
    }
}