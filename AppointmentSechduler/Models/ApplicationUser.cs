using Microsoft.AspNetCore.Identity;

namespace AppointmentSechduler.Models
{
        public class ApplicationUser : IdentityUser
        {
            public string Name { get; set; }
        }
    
}
