using System;

namespace DynamicData.Data.Models.BaseModels
{
    public class BaseModel<TKey> : IModel<TKey>
    {
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
