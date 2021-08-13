using System;

namespace DynamicData.Data.Models.BaseModels
{
    public interface IModel<TKey>
    {
        TKey Id { get; set; }

        DateTime CreatedOn { get; set; }

        DateTime ModifiedOn { get; set; }
    }
}
