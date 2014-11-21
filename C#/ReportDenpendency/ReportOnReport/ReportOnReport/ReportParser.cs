using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ReportOnReport
{
    internal class ReportParser
    {
        private static readonly ILog Logger = LogManager.GetLogger("ReportParser");
        private static readonly RelationshipContext Context = RelationshipContext.Instance;

        private readonly XElement _root;
        private readonly XmlNamespaceManager _namespaceManager;

        private readonly Report _report;

        public ReportParser(string fileName)
        {
            var reportName = Path.GetFileNameWithoutExtension(fileName);
            Logger.InfoFormat("REPORT: {0}", reportName);

            // Create report
            _report = new Report
                   {
                       Name = reportName,
                       Sources = new List<Source>(),
                       Sets = new List<Set>(),
                   };
            Context.Reports.Add(_report);

            // Load XML
            var reader = XmlReader.Create(fileName);
            _root = XElement.Load(reader);

            // Initialized Namespace manager
            _namespaceManager = new XmlNamespaceManager(reader.NameTable);
            foreach (XAttribute attribute in _root.Attributes().Where(a => a.IsNamespaceDeclaration))
            {
                string localName = attribute.Name.LocalName;
                if (localName == "xmlns")
                {
                    localName = "default";
                }

                _namespaceManager.AddNamespace(localName, attribute.Value);
            }
        }

        public void Parse()
        {
            ParseDataSources();
            ParseDataSets();
            Logger.Info(string.Empty);
        }

        private void ParseDataSources()
        {
            var sources = _root.XPathSelectElement("default:DataSources", _namespaceManager);
            if (sources != null)
            {
                foreach (var source in sources.Elements())
                {
                    Source dataSource = null;
                    var connectionProperties = source.XPathSelectElement("default:ConnectionProperties", _namespaceManager);
                    if (connectionProperties != null)
                    {
                        // Kalahari or shared DataSource
                        var dataProvider =
                            connectionProperties.XPathSelectElement("default:DataProvider", _namespaceManager).Value;
                        Debug.Assert(dataProvider == "XML" || dataProvider == "SQL");

                        var connectionString =
                            connectionProperties.XPathSelectElement("default:ConnectString", _namespaceManager).Value;
                        dataSource = Context.Sources.FirstOrDefault(s => s.ConnectionString == connectionString);
                        if (dataSource == null)
                        {
                            dataSource = new Source
                                {
                                    ConnectionString = connectionString,
                                    DataProvider = dataProvider,
                                    Reports = new List<Report>(),
                                };
                            Context.Sources.Add(dataSource);
                        }
                        Logger.InfoFormat("    DataSource: {0} ({1}).", dataSource.DataProvider, dataSource.ConnectionString);
                    }
                    else
                    {
                        var referenceSource = source.XPathSelectElement("default:DataSourceReference", _namespaceManager).Value;
                        Debug.Assert(!string.IsNullOrWhiteSpace(referenceSource));
                        dataSource = Context.Sources.FirstOrDefault(s => s.Name == referenceSource);
                        if (dataSource == null)
                        {
                            dataSource = new Source
                                {
                                    Name = referenceSource,
                                    Reports = new List<Report>(),
                                };
                            Context.Sources.Add(dataSource);
                        }
                        Logger.InfoFormat("    DataSource: {0}.", dataSource.Name);
                    }

                    dataSource.Name = source.Attribute("Name").Value;
                    _report.Sources.Add(dataSource);
                    dataSource.Reports.Add(_report);
                }
            }
            else
            {
                Logger.Info("    DataSource: NULL.");
            }

            Logger.Info(string.Empty);
        }

        private void ParseDataSets()
        {
            var sets = _root.XPathSelectElement("default:DataSets", _namespaceManager);
            foreach (var set in sets.Elements())
            {
                Set dataSet = null;
                var query = set.XPathSelectElement("default:Query", _namespaceManager);
                var sharedDataSet = set.XPathSelectElement("default:SharedDataSet", _namespaceManager);

                if (query != null)
                {
                    // It isn't a shared set.
                    var sourceName = query.XPathSelectElement("default:DataSourceName", _namespaceManager).Value;
                    var command = query.XPathSelectElement("default:CommandText", _namespaceManager).Value;
                    Debug.Assert(!string.IsNullOrWhiteSpace(sourceName));
                    dataSet = new Set
                        {
                            Command = command,
                            Reports = new List<Report>(),
                            Source = _report.Sources.First(s => s.Name == sourceName),
                        };
                    Debug.Assert(!string.IsNullOrWhiteSpace(command) || dataSet.Source.DataProvider == "XML");
                    Context.Sets.Add(dataSet);
                    command = command.Replace("\n", string.Empty);
                    Logger.InfoFormat("    DataSet: Command ({0}).", command.Substring(0, Math.Min(command.Length, 50)));
                }
                else if (sharedDataSet != null)
                {
                    string sharedDataSetName = sharedDataSet.XPathSelectElement("default:SharedDataSetReference", _namespaceManager).Value;
                    Debug.Assert(!string.IsNullOrWhiteSpace(sharedDataSetName));
                    dataSet = Context.Sets.FirstOrDefault(s => s.Name == sharedDataSetName);
                    if (dataSet == null)
                    {
                        dataSet = new Set
                            {
                                Name = sharedDataSetName,
                                Reports = new List<Report>(),
                            };

                        Context.Sets.Add(dataSet);
                    }
                    Logger.InfoFormat("    DataSet: Shared DataSet {0}.", dataSet.Name);
                }
                else
                {
                    Debug.Assert(false);
                }
                Debug.Assert(dataSet != null);
                _report.Sets.Add(dataSet);
                dataSet.Reports.Add(_report);
            }
        }
    }
}