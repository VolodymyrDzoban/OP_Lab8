using Hospital.MedicalCard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Persons
{
    public class Patient : Person
    {
        private int nextRecordId = 1;
        private readonly List<EPatientCardRecord> card = new List<EPatientCardRecord>();

        public Patient(int id, string name, string surname)
        {
            this.ID = id;
            this.Name = name;
            this.Surname = surname;
        }

        public void AddMedicalRecord(string diagnosis, DateTime start, Doctor doctor)
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                throw new ArgumentNullException(nameof(diagnosis), "Diagnosis cannot be empty.");
            }
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor), "Doctor cannot be null.");
            }

            card.Add(new EPatientCardRecord(nextRecordId, diagnosis, start, doctor));
            nextRecordId++;
        }

        public void CloseMedicalRecord(int recordId, DateTime end)
        {
            EPatientCardRecord record = card.Find(r => r.RecordID == recordId);

            if (record == null)
            {
                throw new InvalidOperationException($"Medical record with ID {recordId} not found.");
            }

            record.CloseRecord(end);
        }

        public IReadOnlyList<EPatientCardRecord> GetMedicalCard()
        {
            return card.AsReadOnly();
        }
    }
}
