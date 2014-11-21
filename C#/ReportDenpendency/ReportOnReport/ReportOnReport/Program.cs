using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ReportOnReport
{
    internal class Program
    {
        internal const string Folder = @"D:\Work\Reports\Reports";

        private static void Main(string[] args)
        {
            foreach (var report in Directory.GetFiles(Folder, "*.rdl"))
            {
                if (report.EndsWith("TradeWithNoPrice.rdl"))
                {
                    // The report is corrupt.
                    continue;
                }

                if (report.EndsWith("JapanShortStock.rdl") || report.EndsWith("ShortJapanesePositions.rdl"))
                {
                    // The report isn't added in project and the shared dataset is missing.
                    continue;
                }

                if (report.EndsWith("VarVolNullPrices.rdl"))
                {
                    // The report seems not finished.
                    continue;
                }

                new ReportParser(report).Parse();
            }

            foreach (var set in RelationshipContext.Instance.Sets)
            {
                new DataSetParser(set).Parse();
            }

            var instance = RelationshipContext.Instance;
            Database.SetInitializer(new DropCreateDatabaseAlways<ReportDbContext>());
            using (var context = new ReportDbContext())
            {
                context.Reports.AddRange(instance.Reports);
                context.Sources.AddRange(instance.Sources);
                context.Sets.AddRange(instance.Sets);
                context.SaveChanges();
            }
        }
    }
}