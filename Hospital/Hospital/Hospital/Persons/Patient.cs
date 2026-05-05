using Hospital.MedicalCard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Persons
{
    public class Patient : Person
    {
        private readonly List<EPatientCardRecord> card = new List<EPatientCardRecord>();

        public Patient(int id, string name, string surname)
        {
            this.ID = id;
            this.Name = name;
            this.Surname = surname;
        }

        public void AddMedicalRecord(EPatientCardRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record), "Medical record cannot be null.");
            }

            card.Add(record);
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
