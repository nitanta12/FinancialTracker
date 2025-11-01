using System.ComponentModel.DataAnnotations;

namespace FT.Core.Security
{
    public class UserCore
    {
        
        [Required]
        
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
       
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }

        public int? UserId { get; set; }
    }

    public class ApplicationIdentityResult
    {
        public List<string> Errors
        {
            get;
            private set;
        }

        public bool Succeeded
        {
            get;
            private set;
        }

        public ApplicationIdentityResult(List<string> errors, bool succeeded)
        {
            Succeeded = succeeded;
            Errors = errors;
        }
    }

    public class ResetPasswordDto
    {
        public string UserId { get; set; }
        public string TokenId { get; set; }
        public string Password { get; set; }
    }
}
