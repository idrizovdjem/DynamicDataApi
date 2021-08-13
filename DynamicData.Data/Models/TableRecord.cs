using System;
using System.ComponentModel.DataAnnotations;
using DynamicData.Data.Models.BaseModels;

namespace DynamicData.Data.Models
{
    public class TableRecord : BaseDeletableModel<string>
    {
        public TableRecord()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }
    }
}
