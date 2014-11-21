using Microsoft.SqlServer.ReportingServices2010;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace ReportParameters
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var reportingService = new ReportingService2010
                {
                    Credentials = CredentialCache.DefaultCredentials,
                    Url = "http://SQLREPORTS/ReportServer/ReportService2010.asmx"
                };

            var items = reportingService.FindItems("/", BooleanOperatorEnum.And, new Property[] { }, new SearchCondition[] { });
            var reports = from item in items
                          let typeName = item.TypeName
                          where typeName == "Report" && item.Hidden != true
                          select item;
            using (var writer = new StreamWriter(File.OpenWrite("ReportParameter.csv")))
            {
                writer.WriteLine("ReportName,ParameterName");
                foreach (var report in reports)
                {
                    Debug.WriteLine(report.Name);
                    var parameters = reportingService.GetItemParameters(report.Path, null, false, null, null);
                    foreach (var parameter in parameters)
                    {
                        Debug.WriteLine("{0} - {1}", report.Name, parameter.Name);
                        writer.WriteLine("{0},{1}", report.Name, parameter.Name);
                    }
                }
            }
        }
    }
}