using System;

namespace DynamicData.Data.Models.BaseModels
{
    public class BaseDeletableModel<TKey> : BaseModel<TKey>, IDeletableModel<TKey>
    {
        public DateTime DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
