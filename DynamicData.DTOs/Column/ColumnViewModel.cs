using DynamicData.Common.Enums;

namespace DynamicData.DTOs.Column
{
    public class ColumnViewModel
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public bool IsRequired { get; set; }

        public bool IsUnique { get; set; }
    }
}
