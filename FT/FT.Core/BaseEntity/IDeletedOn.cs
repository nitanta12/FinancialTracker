using System;

namespace HR.Core.BaseEntity
{
    public interface IDeletedOn
    {
        DateTime? DeletedOn { get; set; }
    }
}
