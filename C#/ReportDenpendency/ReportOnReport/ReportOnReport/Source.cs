using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportOnReport
{
    [Table("Source")]
    public class Source
    {
        public int SourceId { get; set; }

        public string ConnectionString { get; set; }

        public string DataProvider { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}