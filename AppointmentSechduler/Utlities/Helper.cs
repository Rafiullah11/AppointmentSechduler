using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AppointmentSechduler.Utlities
{
    public static class Helper
    {
        public static string Admin = "Admin";
        public static string Doctor = "Doctor";
        public static string Patient = "Patient";

        public static string appointmentAdded = "Appointment added succssefully";
        public static string appointmentUdated = "Appointment Udated succssefully";
        public static string appointmentDelete = "Appointment Delete succssefully";
        public static string appointmentExists = "Appointmentyou try is already Exists";
        public static string appointmentNotExists = "Appointmentyou not Exists";
        public static string appointmentAddError = "Something went wrong plz try again";
        public static string appointmentUpdateError = "Something went wrong plz try again";
        public static string somethingWentWrong = "Something went wrong plz try again";
        public static string MeetingConfirm = "Meeting confirmed";
        public static string MeetingConfirmError = "Meeting error....";

        public static int success_code =1;
        public static int failure_code =0;
    
        public static List<SelectListItem> GetListDD( bool IsAdmin)
        {
            if (IsAdmin)
            {
                return new List<SelectListItem>
            {
                    new SelectListItem{Value = Helper.Admin,Text=Helper.Admin},
            };
            }
            else
            {
                return new List<SelectListItem>
            {
                    new SelectListItem{Value = Helper.Doctor,Text=Helper.Doctor},
                    new SelectListItem{Value = Helper.Patient,Text=Helper.Patient}
            };
            }
        }

        public static List<SelectListItem> GetTimeDropDown()
        {
            int minute = 60;

            var duration = new List<SelectListItem>();
            for (int i = 1; i <=12; i++)
            {
                duration.Add(new SelectListItem { Value = minute.ToString(), Text = i + "hr" });
                minute = minute + 30;
                duration.Add(new SelectListItem { Value = minute.ToString(), Text = i + "Hr 30 min" });
                minute = minute + 30;
            }
            return duration;
        } 
    }
}
