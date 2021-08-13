using System;

namespace DynamicData.Data.Models.BaseModels
{
    public interface IDeletableModel<TKey> : IModel<TKey>
    {
        DateTime DeletedOn { get; set; }

        bool IsDeleted { get; set; }
    }
}
