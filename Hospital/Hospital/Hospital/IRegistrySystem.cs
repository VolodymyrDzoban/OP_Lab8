using Hospital.Persons;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital
{
    public interface IRegistrySystem
    {
        public void AddDoctor(Doctor doctor);
        public void RemoveDoctor(int doctorId);
        public void AddPatient(Patient patient);
        public void RemovePatient(int patientId);
        public Patient SearchPatient(string patientName, string patientSurname);
        public Patient SearchPatient(int patientId);
        public Doctor SearchDoctor(string doctorName, string doctorSurname);
        public Doctor SearchDoctor(int doctorId);
        public List<Doctor> SearchDoctorsBySpecialization(string specialization);
        public List<Doctor> GetAllDoctors();
        public void ViewAllDoctorsWithSpecializations();
        public void ViewDoctorVisitPlan(int doctorId);
        public void ViewDoctorVisitPlan(Doctor doctor);
        public void ViewAllDoctorsVisitPlan();
        public void ViewPatientMedicalCard(int patientId);
        public void ViewPatientMedicalCard(Patient patient);
        public void ViewAllPatientsMedicalCardsOfDoctor(int doctorId);
        public void ViewAllPatientsMedicalCardsOfDoctor(Doctor doctor);
        public void ViewAllPatientsMedicalCardsOfAllDoctors();
    }
}
