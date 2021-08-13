using DynamicData.Common.Enums;

namespace DynamicData.DTOs.Column
{
    public class CreateColumnInputModel
    {
        public string Name { get; set; }

        public ColumnType Type { get; set; }

        public bool IsRequired { get; set; }

        public bool IsUnique { get; set; }
    }
}
