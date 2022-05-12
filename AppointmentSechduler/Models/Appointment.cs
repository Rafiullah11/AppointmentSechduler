using System;

namespace AppointmentSechduler.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Discription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public string AdminId { get; set; }
        public bool IsDoctorApproved { get; set; }
    }
}
