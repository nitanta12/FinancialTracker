using HR.Core.BaseEntity;
using Microsoft.AspNetCore.Identity;

namespace FT.EntityFramework.EntityFramework.Security.Models
{
    public class ApplicationIdentityRole : IdentityRole<string>, ICreatedOn, IHasCreator, IModifiedOn, IHasModifier, ISoftDelete//, IHasTenant
    {
        public ApplicationIdentityRole()
        {

        }

        public ApplicationIdentityRole(string name)
        {
            Name = name;
        }
        public bool IsFixed { get; set; }
        public bool IsActive { get; set; }
        public bool IsReadOnly { get; set; }
        public byte TypeId { get; set; }
        public string Description { get; set; }
        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ApplicationIdentityUserRole : IdentityUserRole<string>
    {
    }

    public class ApplicationIdentityUserClaim : IdentityUserClaim<string>
    {
    }

    public class ApplicationIdentityUserLogin : IdentityUserLogin<string>
    {
    }
    public class ApplicationUserToken : IdentityUserToken<string>
    {
    }
}

