using System;

namespace HR.Core.BaseEntity
{
    public interface IHasTenant
    {
        Guid? TenantId { get; set; }
    }
}
