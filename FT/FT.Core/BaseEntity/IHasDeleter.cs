using System;

namespace HR.Core.BaseEntity
{
    public interface IHasDeleter
    {
        Guid? DeletedBy { get; set; }
    }
}
