using Hospital.Persons;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.VPlan
{
    public class VisitRecord
    {
        public Patient Patient { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public VisitRecord(Patient patient, DateTime start, DateTime end)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient), "Patient cannot be null.");
            }
            if (end <= start)
            {
                throw new ArgumentException("End time must be after start time.");
            }
            if (DateOnly.FromDateTime(start) != DateOnly.FromDateTime(end))
            {
                throw new ArgumentException($"Start and end must be on the same calendar day. Got: {DateOnly.FromDateTime(start)} — {DateOnly.FromDateTime(end)}.");
            }

            this.Patient = patient;
            this.Start = start;
            this.End = end;
        }

        public string GetTimeRange()
        {
            return $"{Start:HH:mm} - {End:HH:mm}";
        }
    }
}
