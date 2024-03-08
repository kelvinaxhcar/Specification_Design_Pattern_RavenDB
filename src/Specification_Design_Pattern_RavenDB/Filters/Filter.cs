using System.Collections.Generic;

namespace Specification_Design_Pattern_RavenDB.Filters
{
    public class Filter
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Label { get; set; }
        public string ObjType { get; set; }
        public List<FilterField> Fields { get; set; }
        public List<FilterOrder> Order { get; set; }
        public List<string> Group { get; set; }
        public List<string> VisibleColumns { get; set; }
    }
}
