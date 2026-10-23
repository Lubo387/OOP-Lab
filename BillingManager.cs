using System;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Managers
{
    public class BillingManager
    {
        private readonly AppointmentManager _appointments;

        public BillingManager(AppointmentManager appointments)
        {
            _appointments = appointments;
        }

        public IPayable[] GetAllUnpaid()
        {
            return FilterUnpaid(_appointments.GetAll());
        }

        public IPayable[] GetUnpaidByPatient(int patientId)
        {
            return FilterUnpaid(_appointments.GetByPatient(patientId));
        }

        private IPayable[] FilterUnpaid(Appointment[] appointments)
        {
            int count = 0;

            for (int i = 0; i < appointments.Length; i++)
            {
                if (!appointments[i].IsPaid && !appointments[i].IsCancelled)
                    count++;
            }

            IPayable[] result = new IPayable[count];
            int index = 0;

            for (int i = 0; i < appointments.Length; i++)
            {
                if (!appointments[i].IsPaid && !appointments[i].IsCancelled)
                {
                    result[index] = appointments[i];
                    index++;
                }
            }

            return result;
        }

        public decimal GetTotalDebt()
        {
            return CalculateDebt(GetAllUnpaid());
        }

        public decimal GetPatientDebt(int patientId)
        {
            return CalculateDebt(GetUnpaidByPatient(patientId));
        }

        private decimal CalculateDebt(IPayable[] items)
        {
            decimal total = 0m;

            for (int i = 0; i < items.Length; i++)
                total += items[i].GetCost();

            return total;
        }

        public bool PayAppointment(int appointmentId)
        {
            Appointment[] appointments = _appointments.GetAll();

            for (int i = 0; i < appointments.Length; i++)
            {
                Appointment appointment = appointments[i];

                if (appointment.Id != appointmentId)
                    continue;

                if (appointment.IsPaid || appointment.IsCancelled)
                    return false;

                appointment.MarkPaid();
                return appointment.IsPaid;
            }

            return false;
        }

        public void DisplayUnpaid(IPayable[] items)
        {
            if (items.Length == 0)
            {
                Console.WriteLine("Немає неоплачених записів.");
                return;
            }

            for (int i = 0; i < items.Length; i++)
            {
                string details = items[i] is Appointment appointment
                    ? appointment.ToString()
                    : "Запис #" + (i + 1);

                Console.WriteLine(
                    details + " | Сума: " + items[i].GetCost().ToString("F2") + " грн");
            }
        }
    }
}
