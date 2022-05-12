using AppointmentSechduler.Services;
using AppointmentSechduler.Utlities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSechduler.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentServices _appointmentServices;

        public AppointmentController(IAppointmentServices appointmentServices)
        {
            _appointmentServices = appointmentServices;
        }
        public IActionResult Index()
        {
           ViewBag.Duration = Helper.GetTimeDropDown();
           ViewBag.Doctorlist= _appointmentServices.GetAllDoctors();
           ViewBag.Patientlist= _appointmentServices.GetAllPatients();
            return View();
        }
    }
}
