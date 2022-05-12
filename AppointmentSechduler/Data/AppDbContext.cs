using AppointmentSechduler.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSechduler.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions <AppDbContext> option):base(option)
        {

        }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
