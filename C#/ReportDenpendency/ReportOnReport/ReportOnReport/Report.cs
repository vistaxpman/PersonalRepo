using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportOnReport
{
    [Table("Report")]
    public class Report
    {
        public int ReportId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Source> Sources { get; set; }

        public virtual ICollection<Set> Sets { get; set; }
    }
}