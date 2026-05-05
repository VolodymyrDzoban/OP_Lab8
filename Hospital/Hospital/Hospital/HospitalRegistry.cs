using Hospital.Persons;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital
{
    public class HospitalRegistry : IRegistrySystem
    {
        private readonly List<Doctor> doctors = new List<Doctor>();
        private readonly List<Patient> patients = new List<Patient>();

        public void AddDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor), "Doctor cannot be null.");
            }
            if (doctors.Exists(d => d.ID == doctor.ID))
            {
                throw new ArgumentException($"A doctor with ID {doctor.ID} already exists.");
            }

            this.doctors.Add(doctor);
        }
        public void RemoveDoctor(int doctorId)
        {
            int removed = this.doctors.RemoveAll(d => d.ID == doctorId);
            if (removed == 0)
            {
                throw new InvalidOperationException($"Doctor with ID {doctorId} not found.");
            }
        }
        public void AddPatient(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient), "Patient cannot be null.");
            }
            if (patients.Exists(p => p.ID == patient.ID))
            {
                throw new ArgumentException($"A patient with ID {patient.ID} already exists.");
            }

            this.patients.Add(patient);
        }
        public void RemovePatient(int patientId)
        {
            int removed = this.patients.RemoveAll(p => p.ID == patientId);

            if (removed == 0)
            {
                throw new InvalidOperationException($"Patient with ID {patientId} not found.");
            }
        }
        public Patient SearchPatient(string patientName, string patientSurname)
        {
            return this.patients.Find(p => p.Name == patientName && p.Surname == patientSurname);
        }
        public Patient SearchPatient(int patientId)
        {
            return this.patients.Find(p => p.ID == patientId);
        }
        public Doctor SearchDoctor(string doctorName, string doctorSurname)
        {
            return this.doctors.Find(d => d.Name == doctorName && d.Surname == doctorSurname);
        }
        public Doctor SearchDoctor(int doctorId)
        {
            return this.doctors.Find(d => d.ID == doctorId);
        }
        public List<Doctor> SearchDoctorsBySpecialization(string specialization)
        {
            return this.doctors.FindAll(d => d.Specialization == specialization);
        }
        public List<Doctor> GetAllDoctors()
        {
            return this.doctors;
        }
        public void ViewAllDoctorsWithSpecializations()
        {
            if (doctors.Count == 0)
            {
                Console.WriteLine("No doctors registered.");
                return;
            }

            Console.WriteLine("=== All Doctors ===");

            foreach (var doctor in doctors)
            {
                Console.WriteLine($"[{doctor.ID}] {doctor.GetFullName()} -- {doctor.Specialization}");
            }
        }
        public void ViewDoctorVisitPlan(int doctorId)
        {
            Doctor doctor = SearchDoctor(doctorId);

            if (doctor == null)
            {
                throw new InvalidOperationException($"Doctor with ID {doctorId} not found.");
            }

            ViewDoctorVisitPlan(doctor);
        }
        public void ViewDoctorVisitPlan(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor), "Doctor cannot be null.");
            }

            var visitPlan = doctor.GetVisitPlan();

            if (visitPlan.Count == 0)
            {
                Console.WriteLine($"{doctor.GetFullName()} has no scheduled visits.");
                return;
            }

            Console.WriteLine($"=== Visit plan for Dr. {doctor.GetFullName()} ===");

            foreach (var entry in visitPlan)
            {
                Console.WriteLine($"\tDate: {entry.Key}");

                foreach (var visit in entry.Value)
                {
                    Console.WriteLine($"\t\tPatient: {visit.Patient.GetFullName()}, Time: {visit.GetTimeRange()}");
                }
            }
        }
        public void ViewAllDoctorsVisitPlan()
        {
            foreach (var doctor in doctors)
            {
                ViewDoctorVisitPlan(doctor);
            }
        }
        public void ViewPatientMedicalCard(int patientId)
        {
            Patient patient = SearchPatient(patientId);

            if (patient == null)
            {
                throw new InvalidOperationException($"Patient with ID {patientId} not found.");
            }

            ViewPatientMedicalCard(patient);
        }
        public void ViewPatientMedicalCard(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient), "Patient cannot be null.");
            }

            Console.WriteLine($"=== Medical card: {patient.GetFullName()} (ID: {patient.ID}) ===");
            var card = patient.GetMedicalCard();

            if (card.Count == 0)
            {
                Console.WriteLine("\tNo records.");
                return;
            }

            foreach (var record in card)
            {
                string endDisplay = record.End.HasValue ? record.End.Value.ToString() : "N/A";
                Console.WriteLine($"\t[{record.RecordID}] {record.Diagnosis} | Duration: {record.Start} - {endDisplay} | Doctor: {record.Doctor.GetFullName()} ({record.Doctor.Specialization})");
            }
        }
        public void ViewAllPatientsMedicalCardsOfDoctor(int doctorId)
        {
            Doctor doctor = SearchDoctor(doctorId);

            if (doctor == null)
            {
                throw new InvalidOperationException($"Doctor with ID {doctorId} not found.");
            }

            ViewAllPatientsMedicalCardsOfDoctor(doctor);
        }
        public void ViewAllPatientsMedicalCardsOfDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor), "Doctor cannot be null.");
            }

            Console.WriteLine($"=== Records for Doctor: {doctor.GetFullName()} ({doctor.Specialization}) ===");

            foreach (var patient in patients)
            {
                foreach (var record in patient.GetMedicalCard())
                {
                    if (record.Doctor.ID == doctor.ID)
                    {
                        string endDisplay = record.End.HasValue ? record.End.Value.ToString() : "N/A";
                        Console.WriteLine($"\tPatient: {patient.GetFullName()} (ID: {patient.ID}) | [{record.RecordID}] {record.Diagnosis} | Duration: {record.Start} - {endDisplay}");
                    }
                }
            }
        }
        public void ViewAllPatientsMedicalCardsOfAllDoctors()
        {
            foreach (var doctor in doctors)
            {
                ViewAllPatientsMedicalCardsOfDoctor(doctor);
            }
        }
    }
}