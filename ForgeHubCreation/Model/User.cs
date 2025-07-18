using System.ComponentModel.DataAnnotations;

namespace ForgeHubCreation.Model
{
    public class User
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "FirstName is Required")]
        [StringLength(50, ErrorMessage = "FirstName Cannot be Exceed 50 Character")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is Required")]
        [StringLength(50, ErrorMessage = "LastName Cannot be Exceed 50 Character")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100, ErrorMessage = "Email Cannot be Exceed 100 Character")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [StringLength(100, MinimumLength =6, ErrorMessage = "Password must be greater than 6 charater")]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; }
        [Required(ErrorMessage = "PhoneNumber is Required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [StringLength(10, ErrorMessage = "PhoneNumber Cannot be Exceed 10 Character")]
        public string PhoneNumber { get; set; }
        public string TwoFactorKey { get; set; }

    }
}
