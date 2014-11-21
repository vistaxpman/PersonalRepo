using log4net;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ReportOnReport
{
    internal class DataSetParser
    {
        private static readonly ILog Logger = LogManager.GetLogger("DataSetParser");
        private static readonly RelationshipContext Context = RelationshipContext.Instance;

        private readonly Set _set;
        private readonly List<string> _dependencies = new List<string>();

        public DataSetParser(Set set)
        {
            _set = set;
        }

        public void Parse()
        {
            if (!string.IsNullOrWhiteSpace(_set.Name))
            {
                // Shared Dataset
                var fileName = Path.Combine(Program.Folder, _set.Name + ".rsd");
                Debug.Assert(File.Exists(fileName));

                var reader = XmlReader.Create(fileName);
                var root = XElement.Load(reader);

                // Initialized Namespace manager
                var namespaceManager = new XmlNamespaceManager(reader.NameTable);
                foreach (XAttribute attribute in root.Attributes().Where(a => a.IsNamespaceDeclaration))
                {
                    string localName = attribute.Name.LocalName;
                    if (localName == "xmlns")
                    {
                        localName = "default";
                    }

                    namespaceManager.AddNamespace(localName, attribute.Value);
                }

                var query = root.XPathSelectElement("default:DataSet/default:Query", namespaceManager);
                Debug.Assert(query != null);
                var referenceSource = query.XPathSelectElement("default:DataSourceReference", namespaceManager).Value;
                Debug.Assert(!string.IsNullOrWhiteSpace(referenceSource));
                var dataSource = Context.Sources.FirstOrDefault(s => s.Name == referenceSource);
                if (dataSource == null)
                {
                    dataSource = new Source
                        {
                            Name = referenceSource,
                            Reports = new List<Report>(),
                        };
                    Context.Sources.Add(dataSource);
                    foreach (var report in _set.Reports)
                    {
                        dataSource.Reports.Add(report);
                        report.Sources.Add(dataSource);
                    }
                }
                _set.Source = dataSource;

                var command = query.XPathSelectElement("default:CommandText", namespaceManager).Value;
                Debug.Assert(!string.IsNullOrWhiteSpace(command));
                _set.Command = command;
            }

            // Output command
            Logger.InfoFormat("DATASET: {0}", _set.Name);
            Logger.InfoFormat("    Command:\n {0}", _set.Command);
            Logger.Info("");

            ParseCommand(_set.Command);

            Logger.InfoFormat("    Dependency: {0}", _set.Dependency);
            Logger.Info("");
        }

        private void ParseCommand(string command)
        {
            if (!command.ToUpper().Contains("\n"))
            {
                // Store procedure
                _dependencies.Add(command);
            }
            else
            {
                command = RemoveComment(command);
                ParseDependencyByRegex(command, @"(?<=FROM\s+)[^\s(@]+");
                ParseDependencyByRegex(command, @"(?<=JOIN\s+)[^\s(@]+");
                ParseDependencyByRegex(command, @"(?<=EXEC\s+)[^\s(@]+");
            }

            _set.Dependency = string.Join(",", _dependencies.Distinct());
        }

        private string RemoveComment(string command)
        {
            command = Regex.Replace(command, @"/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/", "", RegexOptions.Multiline);
            return Regex.Replace(command, "--.*", "");
        }

        private void ParseDependencyByRegex(string command, string regexExpression)
        {
            var matches = Regex.Matches(command, regexExpression, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                _dependencies.Add(match.Value);
            }
        }
    }
}