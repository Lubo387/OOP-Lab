using System;
using System.Collections.Generic;
using ClinicApp.Models;

namespace ClinicApp.Managers
{
    public class AnalyticsManager
    {
        private readonly AppointmentManager _appointments;
        private readonly DoctorManager _doctors;
        private readonly PatientManager _patients;

        public AnalyticsManager(
            AppointmentManager appointments,
            DoctorManager doctors,
            PatientManager patients)
        {
            _appointments = appointments;
            _doctors = doctors;
            _patients = patients;
        }

        public IEnumerable<DoctorStats> ComputeDoctorStats()
        {
            Doctor[] doctors = _doctors.GetAll();
            Appointment[] appointments = _appointments.GetAll();

            foreach (Doctor doctor in doctors)
            {
                int count = 0;
                decimal revenue = 0m;
                DateTime lastDate = DateTime.MinValue;

                foreach (Appointment appointment in appointments)
                {
                    if (appointment.DoctorId != doctor.Id)
                        continue;

                    count++;
                    revenue += appointment.GetCost();

                    if (appointment.ScheduledAt > lastDate)
                        lastDate = appointment.ScheduledAt;
                }

                yield return new DoctorStats(
                    doctor.Id,
                    doctor.FullName,
                    count,
                    revenue,
                    lastDate);
            }
        }

        public IEnumerable<PatientStats> ComputePatientStats()
        {
            Patient[] patients = _patients.GetAll();
            Appointment[] appointments = _appointments.GetAll();

            foreach (Patient patient in patients)
            {
                int count = 0;
                decimal spent = 0m;
                DateTime lastDate = DateTime.MinValue;

                foreach (Appointment appointment in appointments)
                {
                    if (appointment.PatientId != patient.Id)
                        continue;

                    count++;
                    spent += appointment.GetCost();

                    if (appointment.ScheduledAt > lastDate)
                        lastDate = appointment.ScheduledAt;
                }

                yield return new PatientStats(
                    patient.Id,
                    patient.FullName,
                    count,
                    spent,
                    lastDate);
            }
        }
    }
}
