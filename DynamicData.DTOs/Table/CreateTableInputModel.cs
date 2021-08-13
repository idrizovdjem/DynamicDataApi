using DynamicData.DTOs.Column;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DynamicData.DTOs.Table
{
    public class CreateTableInputModel
    {
        [Required]
        [MinLength(3), MaxLength(400)]
        public string Name { get; set; }

        public List<CreateColumnInputModel> Columns { get; set; }
    }
}
