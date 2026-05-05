using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.VPlan
{
    public class VisitPlan
    {
        private readonly Dictionary<DateOnly, List<VisitRecord>> plan = new Dictionary<DateOnly, List<VisitRecord>>();

        private static void CheckOverlap(List<VisitRecord> existingRecords, VisitRecord newRecord, VisitRecord? excludedRecord)
        {
            foreach (VisitRecord existing in existingRecords)
            {
                if (ReferenceEquals(existing, excludedRecord))
                {
                    continue;
                }

                bool overlaps = newRecord.Start < existing.End && existing.Start < newRecord.End;

                if (overlaps)
                {
                    throw new InvalidOperationException($"Time slot {newRecord.GetTimeRange()} overlaps with an existing visit for patient {existing.Patient.GetFullName()} ({existing.GetTimeRange()}).");
                }
            }
        }

        public void AddSchedule(DateOnly date, List<VisitRecord> records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records), "Records list cannot be null.");
            }
            if (plan.ContainsKey(date))
            {
                throw new ArgumentException($"A schedule for date {date} already exists. Use EditSchedule to modify it.");
            }

            for (int i = 0; i < records.Count; i++)
            {
                for (int j = i + 1; j < records.Count; j++)
                {
                    bool overlaps = records[i].Start < records[j].End && records[j].Start < records[i].End;
                    if (overlaps)
                    {
                        throw new InvalidOperationException($"Records at positions {i} and {j} overlap: {records[i].GetTimeRange()} vs {records[j].GetTimeRange()}.");
                    }
                }
            }

            plan.Add(date, records);
        }

        public void AddVisitRecord(DateOnly date, VisitRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record), "Visit record cannot be null.");
            }
            if (!plan.TryGetValue(date, out var records))
            {
                records = new List<VisitRecord>();
                plan[date] = records;
            }

            CheckOverlap(records, record, null);
            records.Add(record);
        }

        public void EditVisitRecord(DateOnly date, VisitRecord record, VisitRecord newRecord)
        {
            if (!plan.TryGetValue(date, out var records))
            {
                throw new KeyNotFoundException($"No schedule found for date {date}.");
            }

            int index = records.IndexOf(record);

            if (index == -1)
            {
                throw new InvalidOperationException("The specified visit record was not found in the plan for that date.");
            }

            CheckOverlap(records, newRecord, record);
            records[index] = newRecord;
        }

        public void EditSchedule(DateOnly date, List<VisitRecord> newRecords)
        {
            if (newRecords == null)
            {
                throw new ArgumentNullException(nameof(newRecords), "Records list cannot be null.");
            }
            if (!plan.ContainsKey(date))
            {
                throw new KeyNotFoundException($"No schedule found for date {date}.");
            }

            for (int i = 0; i < newRecords.Count; i++)
            {
                for (int j = i + 1; j < newRecords.Count; j++)
                {
                    bool overlaps = newRecords[i].Start < newRecords[j].End && newRecords[j].Start < newRecords[i].End;
                    if (overlaps)
                    {
                        throw new InvalidOperationException($"Records at positions {i} and {j} overlap: {newRecords[i].GetTimeRange()} vs {newRecords[j].GetTimeRange()}.");
                    }
                }
            }

            plan[date] = newRecords;
        }

        public List<VisitRecord> GetPlanForDay(DateOnly date)
        {
            if (!plan.TryGetValue(date, out var records))
            {
                throw new KeyNotFoundException($"No schedule found for date {date}.");
            }

            return records;
        }

        public void DeleteVisitRecord(DateOnly date, VisitRecord record)
        {
            if (!plan.TryGetValue(date, out var records))
            {
                throw new KeyNotFoundException($"No schedule found for date {date}.");
            }

            bool removed = records.Remove(record);

            if (!removed)
            {
                throw new InvalidOperationException("The specified visit record was not found in the plan for that date.");
            }
        }

        public Dictionary<DateOnly, List<VisitRecord>> GetPlan()
        {
            return plan;
        }
    }
}
