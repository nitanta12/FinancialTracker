using System;

namespace HR.Core.BaseEntity;

public interface IModifiedOn
{
    DateTime? ModifiedOn { get; set; }

}
