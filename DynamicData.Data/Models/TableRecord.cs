using System;
using DynamicData.Data.Models.BaseModels;

namespace DynamicData.Data.Models
{
    public class TableRecord : BaseDeletableModel<string>
    {
        public TableRecord()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Name { get; set; }

        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }
    }
}
