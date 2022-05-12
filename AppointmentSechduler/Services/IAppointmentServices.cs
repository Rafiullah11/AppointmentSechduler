using AppointmentSechduler.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentSechduler.Services
{
    public interface IAppointmentServices
    {
        List<DoctorVM> GetAllDoctors();
        List<PatientVM> GetAllPatients();
        public Task<int> AddUpdat(AppointmentVM model);
        public List<AppointmentVM> DoctorEventById(string doctorId);
        public List<AppointmentVM> PatientEventById(string patientId);
        public AppointmentVM GetById(int id);
        public  Task<int> ConfirmEvent(int id);
        public  Task<int> Delete(int id);
    }
}
