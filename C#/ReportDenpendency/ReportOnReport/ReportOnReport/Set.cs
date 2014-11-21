using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportOnReport
{
    [Table("Set")]
    public class Set
    {
        public int SetId { get; set; }

        public string Name { get; set; }

        public string Command { get; set; }

        public Source Source { get; set; }

        public string Dependency { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}