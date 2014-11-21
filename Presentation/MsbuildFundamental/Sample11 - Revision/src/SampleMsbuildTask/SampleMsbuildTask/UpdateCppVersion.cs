using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace SampleMsbuildTask
{
    public class UpdateCppVersion : Task
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
            versionFileContent = Regex.Replace(versionFileContent, @"(?<=#define VERSION_MAJOR\s*)(\d+)", Major.ToString());
            versionFileContent = Regex.Replace(versionFileContent, @"(?<=#define VERSION_MINOR\s*)(\d+)", Minor.ToString());
            versionFileContent = Regex.Replace(versionFileContent, @"(?<=#define VERSION_REVISION\s*)(\d+)", Revision.ToString());
            versionFileContent = Regex.Replace(versionFileContent, @"(?<=#define VERSION_BUILD\s*)(\d+)", BuildNumber.ToString());
            File.WriteAllText(VersionFile, versionFileContent);

            return true;
        }
    }
}