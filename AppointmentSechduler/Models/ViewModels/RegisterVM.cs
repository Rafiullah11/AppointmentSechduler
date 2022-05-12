using System.ComponentModel.DataAnnotations;

namespace AppointmentSechduler.Models.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage ="Password should be at least 2 charachters long",MinimumLength = 6)]
        public string Password { get; set; }

        [Required,DataType(DataType.Password), 
         Compare("Password",ErrorMessage ="Confirm password do not matched"),
         Display(Name ="Confirm password")]
        public string ConfirmPassword { get; set; }

        [Required, Display(Name ="Role Name")]
        public string RoleName { get; set; }
    }
}
