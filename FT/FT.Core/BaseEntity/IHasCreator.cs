using System;

namespace HR.Core.BaseEntity
{
    public interface IHasCreator
    {
        Nullable<Guid> CreatedBy { get; set; }
    }
}
