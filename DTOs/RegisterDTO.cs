using System.ComponentModel.DataAnnotations;

namespace pcman.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string EduId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}