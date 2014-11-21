using System;
using System.Collections.Generic;

namespace ReportOnReport
{
    internal class RelationshipContext
    {
        private static readonly Lazy<RelationshipContext> LazyInstance = new Lazy<RelationshipContext>(() => new RelationshipContext());
        private readonly IList<Report> _reports = new List<Report>();
        private readonly IList<Source> _sources = new List<Source>();
        private readonly IList<Set> _sets = new List<Set>();

        private RelationshipContext()
        {
        }

        public static RelationshipContext Instance
        {
            get { return LazyInstance.Value; }
        }

        public IList<Report> Reports
        {
            get { return _reports; }
        }

        public IList<Source> Sources
        {
            get { return _sources; }
        }

        public IList<Set> Sets
        {
            get { return _sets; }
        }
    }
}