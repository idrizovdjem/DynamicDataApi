using DynamicData.DTOs.Column;

namespace DynamicData.DTOs.Table
{
    public class TableRecordViewModel
    {
        public string Name { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public ColumnViewModel[] Columns { get; set; }
    }
}
