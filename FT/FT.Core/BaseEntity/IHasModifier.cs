using System;

namespace HR.Core.BaseEntity
{
    public interface IHasModifier
    {
        Nullable<Guid> ModifiedBy { get; set; }
    }
}
