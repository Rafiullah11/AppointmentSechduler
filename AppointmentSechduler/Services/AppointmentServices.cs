using AppointmentSechduler.Data;
using AppointmentSechduler.Models;
using AppointmentSechduler.Models.ViewModels;
using AppointmentSechduler.Utlities;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSechduler.Services
{
    public class AppointmentServices : IAppointmentServices
    {
        private readonly AppDbContext _db;
        private readonly IEmailSender _emailSender;

        public AppointmentServices(AppDbContext db, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }

        public async Task<int> AddUpdat(AppointmentVM model)
        {
            var startDate = DateTime.Parse(model.StartDate); 
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));
            var doctor = _db.Users.FirstOrDefault(d => d.Id == model.DoctorId);
            var patient = _db.Users.FirstOrDefault(d => d.Id == model.PatientId);

            if (model !=null && model.Id>0)
            {
                //Code for Update

                var appointment = _db.Appointments.FirstOrDefault(x=>x.Id==model.Id);
                    appointment.StartDate = startDate;
                    appointment.EndDate = endDate;
                    appointment.AdminId = model.AdminId;
                    appointment.Discription = model.Discription;
                    appointment.DoctorId = model.DoctorId;
                    appointment.Duration = model.Duration;
                    appointment.PatientId = model.PatientId;
                    appointment.Title = model.Title;
                    appointment.IsDoctorApproved = false;

                await _db.SaveChangesAsync();
                return 1;
            }
            else
            {
                //code for create
                var create = new Appointment{
                    StartDate = startDate,
                    EndDate = endDate,
                    AdminId = model.AdminId,
                    Discription=model.Discription,
                    DoctorId=model.DoctorId,
                    Duration=model.Duration,
                    PatientId=model.PatientId,
                    Title=model.Title,
                    IsDoctorApproved=false
                };
                await _emailSender.SendEmailAsync(doctor.Email, "Appiontment Created", 
                    $"Your appointment is created with {patient.Name} in the pending status");
                await _emailSender.SendEmailAsync(patient.Email, "Appiontment Created", 
                    $"Your appointment is created with {doctor.Name} in the pending status");
                _db.Add(create);
                await _db.SaveChangesAsync();
            }
            await _db.SaveChangesAsync();
            return 2;
        }

        public async Task<int> ConfirmEvent(int id)
        {
            var appointment=_db.Appointments.FirstOrDefault(u => u.Id == id);
            if (appointment != null)
            {
                appointment.IsDoctorApproved = true;
               return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> Delete(int id)
        {
            var appointment = _db.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment!=null)
            {
                _db.Appointments.Remove(appointment);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public List<AppointmentVM> DoctorEventById(string doctorId)
        {
            return _db.Appointments.Where(x => x.DoctorId == doctorId).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Title = c.Title,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm"),
                Discription=c.Discription,
                Duration=c.Duration,
                IsDoctorApproved = c.IsDoctorApproved
            }).ToList() ;
        }

        public List<DoctorVM> GetAllDoctors()
        {
            var doctors = (from user in _db.Users
                           join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                           join roles in _db.Roles.Where (x=>x.Name==Helper.Doctor) on userRoles.RoleId equals roles.Id
                          select new DoctorVM
                          {
                              Id = user.Id,
                              Name = user.Name,
                          }
                          ).ToList();
           return doctors;
        }

        public List<PatientVM> GetAllPatients()
        {
            var patient = (from user in _db.Users
                           join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                           join roles in _db.Roles.Where(x => x.Name == Helper.Patient) on userRoles.RoleId equals roles.Id
                           select new PatientVM
                           {
                               Id = user.Id,
                               Name = user.Name,
                           }
                        ).ToList();
            return patient;
        }

        public AppointmentVM GetById(int id)
        {
            return _db.Appointments.Where(x => x.Id == id).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Title = c.Title,
                StartDate = c.StartDate.ToString("MM/dd/yyyy HH:mm:ss"),
                EndDate = c.EndDate.ToString("MM/dd/yyyy HH:mm:ss"),
                Discription = c.Discription,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved,
                DoctorId = c.DoctorId,
                PatientId = c.PatientId,
                DoctorName=_db.Users.Where(x=>x.Id==c.DoctorId).Select(x=>x.Name).FirstOrDefault(),
                PatientName=_db.Users.Where(x=>x.Id==c.PatientId).Select(x=>x.Name).FirstOrDefault()
            }).SingleOrDefault();
        }

        public List<AppointmentVM> PatientEventById(string patientId)
        {
            return _db.Appointments.Where(x => x.PatientId == patientId).ToList().Select(c => new AppointmentVM() 
            { 
             Id=c.Id,
              Title = c.Title,
              StartDate = c.StartDate.ToString("MM/dd/yyyy HH:mm:ss"),
              EndDate = c.EndDate.ToString("MM/dd/yyyy HH:mm:ss"),
              Duration = c.Duration,
              IsDoctorApproved = c.IsDoctorApproved,
              Discription = c.Discription
            }).ToList();
        }
    }
}
