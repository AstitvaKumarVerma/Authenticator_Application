using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Authenticator_Application_Backend.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        public string Password { get; set; }

        [StringLength(10)]
        public string? Otp { get; set; }
        public DateTime? OtpCreatedAt { get; set; }

        public bool IsActive { get; set; } = true;
        //public bool Google_2FA_Enabled { get; set; } = false;
        //public bool Microsoft_2FA_Enabled { get; set; } = false;
        public bool OtpVerification_Done { get; set; } = false;
        public bool IsAuthenticated { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
