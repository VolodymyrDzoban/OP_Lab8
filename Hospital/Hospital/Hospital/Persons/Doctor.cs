using Hospital.VPlan;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Persons
{
    public class Doctor : Person
    {
        public string Specialization { get; private set; }
        public VisitPlan VisitPlan { get; } = new VisitPlan();

        public Doctor(int id, string name, string surname, string specialization)
        {
            this.ID = id;
            this.Name = name;
            this.Surname = surname;
            this.Specialization = specialization;
        }
        public string GetSpecialization()
        {
            return Specialization;
        }

        public void EditData(string name, string surname, string specialization)
        {
            this.Name = name;
            this.Surname = surname;
            this.Specialization = specialization;
        }

        public List<VisitRecord> GetVisitPlanForDay(DateOnly date)
        {
            return VisitPlan.GetPlanForDay(date);
        }

        public Dictionary<DateOnly, List<VisitRecord>> GetVisitPlan()
        {
            return VisitPlan.GetPlan();
        }
    }
}
