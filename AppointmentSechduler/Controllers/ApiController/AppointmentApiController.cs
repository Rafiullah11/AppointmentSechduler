using AppointmentSechduler.Models.ViewModels;
using AppointmentSechduler.Services;
using AppointmentSechduler.Utlities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AppointmentSechduler.Controllers.ApiController
{
    [Route("api/Appointment")]
    [ApiController]
    public class AppointmentApiController : Controller
    {
        private readonly IAppointmentServices _appointmentServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string role;
        private readonly string loginUserId;

        public AppointmentApiController(IAppointmentServices appointmentServices, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentServices = appointmentServices;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        [HttpPost]
        [Route("SaveCalendarData")]
        public IActionResult SaveCalendarData( AppointmentVM data )
        {
            var commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status = _appointmentServices.AddUpdat(data).Result;
                if (commonResponse.status==1)
                {
                    commonResponse.message = Helper.appointmentUdated;
                }
                if (commonResponse.status == 2)
                {
                    commonResponse.message = Helper.appointmentAdded;
                }
            }
            catch (Exception e)
            {

                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
           
            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("GetCalendarData")]
        public IActionResult GetCalendarData(string doctorId)
        {
            var commonResponse = new CommonResponse<List<AppointmentVM>>();
            try
            {
                if (role == Helper.Doctor)
                {
                    commonResponse.dataenum=_appointmentServices.DoctorEventById(loginUserId);
                    commonResponse.status= Helper.success_code;
                }
                else if(role == Helper.Patient)
                {
                    commonResponse.dataenum = _appointmentServices.PatientEventById(loginUserId);
                    commonResponse.status = Helper.success_code;
                }
                else
                {
                    commonResponse.dataenum = _appointmentServices.DoctorEventById(doctorId);
                    commonResponse.status = Helper.success_code;
                }
            }
            catch (Exception e)
            {

                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }

            return Ok(commonResponse);
        } 
        
        [HttpGet]
        [Route("GetCalendarDataById/{id}")]
        public IActionResult GetCalendarDataById(int id)
        {
            var commonResponse = new CommonResponse<AppointmentVM>();
            try
            {
               
                    commonResponse.dataenum = _appointmentServices.GetById(id);
                    commonResponse.status = Helper.success_code;
                
            }
            catch (Exception e)
            {

                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }

            return Ok(commonResponse);
        }

        [HttpGet]
        [Route ("DeleteAppoinment/{id}")]
        public async Task<IActionResult> DeleteAppoinment(int id)
        {
            var commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status =await _appointmentServices.Delete(id);
                commonResponse.message = commonResponse.status == 1 ? Helper.appointmentDelete : Helper.somethingWentWrong;
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status=Helper.failure_code;
            }
            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("ConfirmEvent/{id}")]
        public IActionResult ConfirmEvent(int id)
        {
            var commonResponse = new CommonResponse<int>();
            try
            {
                var result =  _appointmentServices.ConfirmEvent(id).Result;

                if (result>0)
                {
                    commonResponse.status = Helper.success_code;
                    commonResponse.message = Helper.MeetingConfirm;
                }
                else
                {
                    commonResponse.status = Helper.failure_code;
                    commonResponse.message = Helper.MeetingConfirmError;
                }
            }
            catch (Exception e)
            {

                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return  Ok(commonResponse);
        }
    }
}
