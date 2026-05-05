using Hospital.Persons;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.MedicalCard
{
    public class EPatientCardRecord
    {
        public int RecordID { get; private set; }
        public string Diagnosis { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime? End { get; private set; } = null;
        public Doctor Doctor { get; private set; }
        public EPatientCardRecord(int recordID, string diagnosis, DateTime start, Doctor doctor)
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                throw new ArgumentNullException(nameof(diagnosis), "Diagnosis cannot be empty.");
            }
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor), "Doctor cannot be null.");
            }

            this.RecordID = recordID;
            this.Diagnosis = diagnosis;
            this.Start = start;
            this.Doctor = doctor;
        }

        public void CloseRecord(DateTime end)
        {
            if (End.HasValue)
            {
                throw new InvalidOperationException($"Record {RecordID} is already closed.");
            }
            if (end <= Start)
            {
                throw new ArgumentException("End date must be after the start date.");
            }

            this.End = end;
        }

        public void EditRecord(string diagnosis, DateTime start, Doctor doctor)
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                throw new ArgumentNullException(nameof(diagnosis), "Diagnosis cannot be empty.");
            }
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor), "Doctor cannot be null.");
            }

            this.Diagnosis = diagnosis;
            this.Start = start;
            this.Doctor = doctor;
        }

        public void EditRecord(string diagnosis, DateTime start, DateTime? end, Doctor doctor)
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                throw new ArgumentNullException(nameof(diagnosis), "Diagnosis cannot be empty.");
            }
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor), "Doctor cannot be null.");
            }

            this.Diagnosis = diagnosis;
            this.Start = start;
            this.End = end;
            this.Doctor = doctor;
        }
    }
}
