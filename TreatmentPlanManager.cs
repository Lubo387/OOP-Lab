using ClinicApp.Enums;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp.Managers
{
    public class TreatmentPlanManager
    {
        private readonly List<TreatmentPlan> _plans =
            new List<TreatmentPlan>();

        public bool Add(TreatmentPlan plan)
        {
            ValidationResult validation = ModelValidator.Validate(plan);

            if (!validation.IsValid)
            {
                validation.Print();
                return false;
            }

            _plans.Add(plan);
            return true;
        }

        public TreatmentPlan? GetById(int id)
        {
            foreach (TreatmentPlan plan in _plans)
            {
                if (plan.Id == id)
                    return plan;
            }

            return null;
        }

        public TreatmentPlan[] GetByPatient(int patientId)
        {
            List<TreatmentPlan> result = new List<TreatmentPlan>();

            foreach (TreatmentPlan plan in _plans)
            {
                if (plan.PatientId == patientId)
                    result.Add(plan);
            }

            return result.ToArray();
        }

        public TreatmentPlan[] GetByStatus(TreatmentStatus status)
        {
            List<TreatmentPlan> result = new List<TreatmentPlan>();

            foreach (TreatmentPlan plan in _plans)
            {
                if (plan.Status == status)
                    result.Add(plan);
            }

            return result.ToArray();
        }

        public TreatmentPlan[] GetAll()
        {
            return _plans.ToArray();
        }
    }
}
