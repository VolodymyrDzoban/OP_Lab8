using Hospital;
using Hospital.MedicalCard;
using Hospital.Persons;
using Hospital.VPlan;

namespace ProgramApp
{
    internal class Program
    {
        #region Pre-populated registry

        private static HospitalRegistry BuildDemoRegistry()
        {
            HospitalRegistry registry = new HospitalRegistry();

            Doctor drPetrenko = new Doctor(1, "Oleksiy", "Petrenko", "Cardiologist");
            Doctor drKovalenko = new Doctor(2, "Iryna", "Kovalenko", "Neurologist");
            Doctor drMelnyk = new Doctor(3, "Vasyl", "Melnyk", "Surgeon");

            registry.AddDoctor(drPetrenko);
            registry.AddDoctor(drKovalenko);
            registry.AddDoctor(drMelnyk);

            Patient patShevchenko = new Patient(101, "Taras", "Shevchenko");
            Patient patFranko = new Patient(102, "Ivan", "Franko");
            Patient patLesya = new Patient(103, "Lesya", "Ukrainka");

            registry.AddPatient(patShevchenko);
            registry.AddPatient(patFranko);
            registry.AddPatient(patLesya);

            DateOnly june10 = new DateOnly(2025, 6, 10);
            drPetrenko.VisitPlan.AddVisitRecord(june10,
                new VisitRecord(patShevchenko,
                    new DateTime(2025, 6, 10, 9, 0, 0),
                    new DateTime(2025, 6, 10, 9, 30, 0)));
            drPetrenko.VisitPlan.AddVisitRecord(june10,
                new VisitRecord(patFranko,
                    new DateTime(2025, 6, 10, 10, 0, 0),
                    new DateTime(2025, 6, 10, 10, 30, 0)));
            drKovalenko.VisitPlan.AddVisitRecord(june10,
                new VisitRecord(patLesya,
                    new DateTime(2025, 6, 10, 14, 0, 0),
                    new DateTime(2025, 6, 10, 14, 45, 0)));

            patShevchenko.AddMedicalRecord("Hypertension stage II", new DateTime(2025, 1, 15), drPetrenko);
            patShevchenko.AddMedicalRecord("Arrhythmia", new DateTime(2025, 3, 20), drPetrenko);
            patShevchenko.CloseMedicalRecord(2, new DateTime(2025, 4, 10));

            patFranko.AddMedicalRecord("Migraine", new DateTime(2025, 2, 5), drKovalenko);

            patLesya.AddMedicalRecord("Lumbar herniation", new DateTime(2025, 5, 1), drMelnyk);

            return registry;
        }

        #endregion

        #region Helpers

        private static void PrintHeader(string title)
        {
            Console.WriteLine();
            Console.WriteLine("--- " + title + " ---");
        }

        private static void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("=== HOSPITAL REGISTRY MENU ===");
            Console.WriteLine();
            Console.WriteLine("  DOCTORS");
            Console.WriteLine("   1  - View all doctors and specializations");
            Console.WriteLine("   2  - Add doctor");
            Console.WriteLine("   3  - Remove doctor");
            Console.WriteLine("   4  - Edit doctor data");
            Console.WriteLine("   5  - Search doctor");
            Console.WriteLine();
            Console.WriteLine("  PATIENTS");
            Console.WriteLine("   6  - View all patients");
            Console.WriteLine("   7  - Add patient");
            Console.WriteLine("   8  - Remove patient");
            Console.WriteLine("   9  - Search patient");
            Console.WriteLine("   10 - Add medical record to patient");
            Console.WriteLine("   11 - Close medical record");
            Console.WriteLine();
            Console.WriteLine("  VISIT SCHEDULE");
            Console.WriteLine("   12 - View doctor visit plan");
            Console.WriteLine("   13 - Add visit record to doctor");
            Console.WriteLine("   14 - Remove visit record from doctor");
            Console.WriteLine("   15 - View all doctors visit plans");
            Console.WriteLine();
            Console.WriteLine("  MEDICAL CARDS");
            Console.WriteLine("   16 - View all medical cards by patient");
            Console.WriteLine("   17 - View medical card of one patient");
            Console.WriteLine("   18 - View all medical cards by doctor");
            Console.WriteLine("   19 - View all records by doctor");
            Console.WriteLine();
            Console.WriteLine("   0  - Exit");
            Console.WriteLine();
            Console.Write("  Enter option number: ");
        }

        private static int ReadId(string prompt)
        {
            Console.Write(prompt);
            int.TryParse(Console.ReadLine(), out int id);

            return id;
        }

        private static string ReadLine(string prompt)
        {
            Console.Write(prompt);

            return Console.ReadLine() ?? string.Empty;
        }

        private static DateOnly ReadDate(string prompt)
        {
            Console.Write(prompt + " (yyyy-MM-dd): ");
            DateOnly.TryParse(Console.ReadLine(), out DateOnly d);

            return d;
        }

        private static DateTime ReadTime(string prompt, DateOnly date)
        {
            Console.Write(prompt + " (HH:mm): ");
            TimeOnly.TryParse(Console.ReadLine(), out TimeOnly t);

            return date.ToDateTime(t);
        }

        #endregion

        #region Handlers

        /// <summary>
        /// 1 - View all doctors and specializations. Displays a list of all doctors in the registry with their full names and specializations.
        /// </summary>
        /// <param name="registry">The hospital registry containing the doctors. Cannot be null.</param>
        static void HandleViewAllDoctors(HospitalRegistry registry)
        {
            PrintHeader("All doctors and specializations");
            registry.ViewAllDoctorsWithSpecializations();
        }

        /// <summary>
        /// 2 - Add a new doctor. Prompts the user for doctor details and adds a new doctor to the specified hospital registry.
        /// </summary>
        /// <param name="registry">The hospital registry to which the new doctor will be added. Cannot be null.</param>
        static void HandleAddDoctor(HospitalRegistry registry)
        {
            PrintHeader("Add doctor");
            int id = ReadId("  Doctor ID: ");
            string name = ReadLine("  First name: ");
            string sur = ReadLine("  Surname: ");
            string spec = ReadLine("  Specialization: ");

            try
            {
                registry.AddDoctor(new Doctor(id, name, sur, spec));
                Console.WriteLine("  Doctor added.");
            }
            catch (Exception ex) { Console.WriteLine("  Error: " + ex.Message); }
        }

        /// <summary>
        /// 3 - Remove a doctor. Removes a doctor from the specified hospital registry based on user input.
        /// </summary>
        /// <remarks>Prompts the user to enter the ID of the doctor to remove. Displays a confirmation
        /// message if the removal is successful, or an error message if the operation fails.</remarks>
        /// <param name="registry">The hospital registry from which the doctor will be removed.</param>
        static void HandleRemoveDoctor(HospitalRegistry registry)
        {
            PrintHeader("Remove doctor");
            int id = ReadId("  Doctor ID: ");

            try
            {
                registry.RemoveDoctor(id);
                Console.WriteLine("  Doctor removed.");
            }
            catch (Exception ex) { Console.WriteLine("  Error: " + ex.Message); }
        }

        /// <summary>
        /// 4 - Edit doctor data. Edits the data of an existing doctor in the specified hospital registry.
        /// </summary>
        /// <remarks>Prompts the user to enter the doctor ID and, if found, allows updating the doctor's
        /// first name, surname, and specialization. If the doctor is not found, no changes are made.</remarks>
        /// <param name="registry">The hospital registry containing the doctor records to be edited.</param>
        static void HandleEditDoctor(HospitalRegistry registry)
        {
            PrintHeader("Edit doctor data");
            int id = ReadId("  Doctor ID: ");
            Doctor? doctor = registry.SearchDoctor(id);

            if (doctor == null)
            {
                Console.WriteLine("  Doctor not found.");
                return;
            }

            Console.WriteLine("  Current: " + doctor.GetFullName() + ", " + doctor.Specialization);
            string name = ReadLine("  New first name: ");
            string sur = ReadLine("  New surname: ");
            string spec = ReadLine("  New specialization: ");
            doctor.EditData(name, sur, spec);
            Console.WriteLine("  Doctor updated.");
        }

        /// <summary>
        /// 5 - Search for a doctor. Handles user interaction for searching doctors in the specified hospital registry by ID, name, or specialization.
        /// </summary>
        /// <remarks>Prompts the user to select a search method and displays the results based on the
        /// chosen criteria. The method interacts with the console for input and output.</remarks>
        /// <param name="registry">The hospital registry to search for doctor records. Cannot be null.</param>
        static void HandleSearchDoctor(HospitalRegistry registry)
        {
            PrintHeader("Search doctor");
            Console.WriteLine("  1) By ID   2) By name   3) By specialization");
            Console.Write("  Choice: ");
            string sub = Console.ReadLine() ?? "1";

            if (sub == "1")
            {
                Doctor? d = registry.SearchDoctor(ReadId("  ID: "));
                Console.WriteLine(d != null
                    ? "  Found: [" + d.ID + "] " + d.GetFullName() + " - " + d.Specialization
                    : "  Not found.");
            }
            else if (sub == "2")
            {
                string n = ReadLine("  First name: ");
                string s = ReadLine("  Surname: ");
                List<Doctor> matches = registry.SearchDoctorsByName(n, s);

                if (matches.Count == 0)
                {
                    Console.WriteLine("  Not found.");
                }
                else
                {
                    foreach (Doctor d in matches)
                    {
                        Console.WriteLine("  Found: [" + d.ID + "] " + d.GetFullName() + " - " + d.Specialization);
                    }
                }
            }
            else
            {
                string spec = ReadLine("  Specialization: ");
                List<Doctor> list = registry.SearchDoctorsBySpecialization(spec);

                if (list.Count == 0)
                {
                    Console.WriteLine("  No doctors found.");
                    return;
                }

                foreach (Doctor d in list)
                {
                    Console.WriteLine("  [" + d.ID + "] " + d.GetFullName() + " - " + d.Specialization);
                }
            }
        }

        /// <summary>
        /// 6 - View all patients. Displays a list of all patients registered in the specified hospital registry.
        /// </summary>
        /// <param name="registry">The hospital registry containing the patient records to display. Cannot be null.</param>
        static void HandleViewAllPatients(HospitalRegistry registry)
        {
            PrintHeader("All patients");
            registry.ViewAllPatients();
        }

        /// <summary>
        /// 7 - Add a new patient. Prompts the user to enter patient details and adds a new patient to the specified hospital registry.
        /// </summary>
        /// <param name="registry">The hospital registry to which the new patient will be added. Cannot be null.</param>
        static void HandleAddPatient(HospitalRegistry registry)
        {
            PrintHeader("Add patient");
            int id = ReadId("  Patient ID: ");
            string name = ReadLine("  First name: ");
            string sur = ReadLine("  Surname: ");
            try
            {
                registry.AddPatient(new Patient(id, name, sur));
                Console.WriteLine("  Patient added.");
            }
            catch (Exception ex) { Console.WriteLine("  Error: " + ex.Message); }
        }

        /// <summary>
        /// 8 - Remove a patient. Removes a patient from the specified hospital registry by prompting the user for a patient ID.
        /// </summary>
        /// <remarks>Displays a confirmation message if the patient is successfully removed, or an error
        /// message if the operation fails.</remarks>
        /// <param name="registry">The hospital registry from which the patient will be removed. Cannot be null.</param>
        static void HandleRemovePatient(HospitalRegistry registry)
        {
            PrintHeader("Remove patient");
            int id = ReadId("  Patient ID: ");

            try
            {
                string fullName = registry.RemovePatientWithName(id);
                Console.WriteLine("  Patient " + fullName + " removed.");
            }
            catch (Exception ex) { Console.WriteLine("  Error: " + ex.Message); }
        }

        /// <summary>
        /// 9 - Search for a patient. Handles user interaction for searching patients in the specified hospital registry by ID or name.
        /// </summary>
        /// <param name="registry">The hospital registry in which to search for patients. Cannot be null.</param>
        static void HandleSearchPatient(HospitalRegistry registry)
        {
            PrintHeader("Search patient");
            Console.WriteLine("  1) By ID   2) By name");
            Console.Write("  Choice: ");
            string sub = Console.ReadLine() ?? "1";

            if (sub == "1")
            {
                Patient? p = registry.SearchPatient(ReadId("  ID: "));
                Console.WriteLine(p != null
                    ? "  Found: [" + p.ID + "] " + p.GetFullName()
                    : "  Not found.");
            }
            else
            {
                string n = ReadLine("  First name: ");
                string s = ReadLine("  Surname: ");
                List<Patient> matches = registry.SearchPatientsByName(n, s);
                if (matches.Count == 0)
                {
                    Console.WriteLine("  Not found.");
                }
                else
                {
                    foreach (Patient p in matches)
                    {
                        Console.WriteLine("  Found: [" + p.ID + "] " + p.GetFullName());
                    }
                }
            }
        }

        /// <summary>
        /// 10 - Add medical record. Prompts the user to add a new medical record for a specified patient and doctor in the provided hospital
        /// registry.
        /// </summary>
        /// <remarks>If the specified patient or doctor does not exist in the registry, the operation is
        /// canceled and a message is displayed. The method interacts with the user via the console to collect required
        /// information.</remarks>
        /// <param name="registry">The hospital registry used to look up patients and doctors and to add the new medical record.</param>
        static void HandleAddMedicalRecord(HospitalRegistry registry)
        {
            PrintHeader("Add medical record");
            int patId = ReadId("  Patient ID: ");
            int docId = ReadId("  Doctor ID: ");
            Patient? patient = registry.SearchPatient(patId);
            Doctor? doctor = registry.SearchDoctor(docId);

            if (patient == null)
            {
                Console.WriteLine("  Patient not found.");
                return;
            }
            if (doctor == null)
            {
                Console.WriteLine("  Doctor not found.");
                return;
            }

            string diag = ReadLine("  Diagnosis: ");
            Console.Write("  Start date (yyyy-MM-dd HH:mm): ");
            DateTime.TryParse(Console.ReadLine(), out DateTime start);

            try
            {
                patient.AddMedicalRecord(diag, start, doctor);
                Console.WriteLine("  Medical record added.");
            }
            catch (Exception ex) { Console.WriteLine("  Error: " + ex.Message); }
        }

        /// <summary>
        /// 11 - Close medical record. Closes a medical record for a specified patient in the provided hospital registry. Prompts the user for
        /// patient and record identifiers, as well as the end date, and attempts to close the corresponding medical
        /// record.
        /// </summary>
        /// <remarks>If the specified patient or record is not found, or if an error occurs while closing
        /// the record, an appropriate message is displayed to the user. This method interacts with the user via the
        /// console to collect required information.</remarks>
        /// <param name="registry">The hospital registry containing patient and medical record information.</param>
        static void HandleCloseMedicalRecord(HospitalRegistry registry)
        {
            PrintHeader("Close medical record");
            int patId = ReadId("  Patient ID: ");
            Patient? patient = registry.SearchPatient(patId);

            if (patient == null)
            {
                Console.WriteLine("  Patient not found.");
                return;
            }

            int recId = ReadId("  Record ID: ");
            Console.Write("  End date (yyyy-MM-dd HH:mm): ");
            DateTime.TryParse(Console.ReadLine(), out DateTime end);

            try
            {
                patient.CloseMedicalRecord(recId, end);
                Console.WriteLine("  Record closed.");
            }
            catch (Exception ex) { Console.WriteLine("  Error: " + ex.Message); }
        }

        /// <summary>
        /// 12 - View doctor visit plan. Displays the visit plan for a specified doctor by retrieving and presenting the doctor's schedule from the
        /// provided hospital registry.
        /// </summary>
        /// <remarks>If the specified doctor ID does not exist or an error occurs while retrieving the
        /// visit plan, an error message is displayed to the user.</remarks>
        /// <param name="registry">The hospital registry instance used to access doctor visit plans.</param>
        static void HandleViewDoctorPlan(HospitalRegistry registry)
        {
            PrintHeader("Doctor visit plan");
            int id = ReadId("  Doctor ID: ");

            try
            {
                registry.ViewDoctorVisitPlan(id);
            }
            catch (Exception ex) { Console.WriteLine("  Error: " + ex.Message); }
        }

        /// <summary>
        /// 13 - Add visit record. Prompts the user to add a new visit record for a patient and doctor in the specified hospital registry.
        /// </summary>
        /// <remarks>The method interacts with the user via the console to collect doctor and patient IDs,
        /// visit date, and time information. It validates the existence of the specified doctor and patient before
        /// adding the visit record. If either is not found, an appropriate message is displayed and the operation is
        /// aborted.</remarks>
        /// <param name="registry">The hospital registry in which to add the visit record. Cannot be null.</param>
        static void HandleAddVisitRecord(HospitalRegistry registry)
        {
            PrintHeader("Add visit record");
            int docId = ReadId("  Doctor ID: ");
            int patId = ReadId("  Patient ID: ");
            Doctor? doctor = registry.SearchDoctor(docId);
            Patient? patient = registry.SearchPatient(patId);

            if (doctor == null)
            {
                Console.WriteLine("  Doctor not found.");
                return;
            }
            if (patient == null)
            {
                Console.WriteLine("  Patient not found.");
                return;
            }

            DateOnly date = ReadDate("  Visit date");
            DateTime start = ReadTime("  Start time", date);
            DateTime end = ReadTime("  End time", date);

            try
            {
                doctor.VisitPlan.AddVisitRecord(date, new VisitRecord(patient, start, end));
                Console.WriteLine("  Visit record added.");
            }
            catch (Exception ex) { Console.WriteLine("  Error: " + ex.Message); }
        }

        /// <summary>
        /// 14 - Remove visit record. Removes a visit record for a specified doctor and date from the hospital registry.
        /// </summary>
        /// <remarks>Prompts the user to select a doctor and a visit date, then displays all visit records
        /// for that day. The user selects a record to remove. If the doctor or visit record is not found, or if an
        /// invalid index is provided, the operation is canceled.</remarks>
        /// <param name="registry">The hospital registry containing doctor and visit record information. Cannot be null.</param>
        static void HandleRemoveVisitRecord(HospitalRegistry registry)
        {
            PrintHeader("Remove visit record");
            int docId = ReadId("  Doctor ID: ");
            Doctor? doctor = registry.SearchDoctor(docId);

            if (doctor == null)
            {
                Console.WriteLine("  Doctor not found.");
                return;
            
            }
            DateOnly date = ReadDate("  Visit date");

            try
            {
                List<VisitRecord> records = doctor.VisitPlan.GetPlanForDay(date);
                for (int i = 0; i < records.Count; i++)
                {
                    Console.WriteLine("  [" + i + "] " + records[i].Patient.GetFullName() + " - " + records[i].GetTimeRange());
                }

                int idx = ReadId("  Record index to remove: ");

                if (idx < 0 || idx >= records.Count)
                {
                    Console.WriteLine("  Invalid index.");
                    return;
                }

                doctor.VisitPlan.DeleteVisitRecord(date, records[idx]);
                Console.WriteLine("  Visit record removed.");
            }
            catch (Exception ex) { Console.WriteLine("  Error: " + ex.Message); }
        }

        /// <summary>
        /// 15 - View all doctors' visit plans. Displays all doctors' visit plans registered in the specified hospital registry.
        /// </summary>
        /// <param name="registry">The hospital registry containing the doctors' visit plans to display. Cannot be null.</param>
        static void HandleViewAllPlans(HospitalRegistry registry)
        {
            PrintHeader("All doctors visit plans");
            registry.ViewAllDoctorsVisitPlan();
        }

        /// <summary>
        /// 16 - View all medical cards grouped by patient. Displays all medical cards grouped by patient using the specified hospital registry.
        /// </summary>
        /// <param name="registry">The hospital registry instance used to retrieve and display medical cards by patient. Cannot be null.</param>
        static void HandleViewAllCardsByPatient(HospitalRegistry registry)
        {
            PrintHeader("All medical cards by patient");
            registry.ViewAllPatientsMedicalCardsByPatients();
        }

        /// <summary>
        /// 17 - View medical card of one specific patient. Displays the medical card information for a single patient by prompting for the patient ID and retrieving
        /// the corresponding record from the registry.
        /// </summary>
        /// <remarks>If the specified patient ID does not exist or an error occurs during retrieval, an
        /// error message is displayed to the user.</remarks>
        /// <param name="registry">The hospital registry instance used to access patient medical card data. Cannot be null.</param>
        static void HandleViewOnePatientCard(HospitalRegistry registry)
        {
            PrintHeader("Medical card of patient");
            int id = ReadId("  Patient ID: ");

            try
            {
                registry.ViewPatientMedicalCard(id);
            }
            catch (Exception ex) { Console.WriteLine("  Error: " + ex.Message); }
        }

        /// <summary>
        /// 18 - View all medical cards grouped by doctor. Displays all medical cards for patients, grouped by doctor, using the specified hospital registry.
        /// </summary>
        /// <param name="registry">The hospital registry instance used to retrieve and display medical cards for all doctors. Cannot be null.</param>
        static void HandleViewAllCardsByDoctor(HospitalRegistry registry)
        {
            PrintHeader("All medical cards by doctor");
            registry.ViewAllPatientsMedicalCardsOfAllDoctors();
        }

        /// <summary>
        /// 19 - View all medical records associated with a specific doctor. Displays all medical records for patients treated by a specific doctor by prompting for the doctor's ID and
        /// retrieving the relevant records from the provided hospital registry.
        /// </summary>
        /// <remarks>If the specified doctor ID does not exist or an error occurs during retrieval, an
        /// error message is displayed to the user.</remarks>
        /// <param name="registry">The hospital registry instance used to access and display medical records. Cannot be null.</param>
        static void HandleViewRecordsByDoctor(HospitalRegistry registry)
        {
            PrintHeader("All records by doctor");
            int id = ReadId("  Doctor ID: ");
            try
            {
                registry.ViewAllPatientsMedicalCardsOfDoctor(id);
            }
            catch (Exception ex) { Console.WriteLine("  Error: " + ex.Message); }
        }

        #endregion

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== HOSPITAL REGISTRY SYSTEM ===");
            Console.WriteLine();
            Console.WriteLine("  Start with:");
            Console.WriteLine("   1 - Pre-populated demo registry");
            Console.WriteLine("   2 - Blank registry");
            Console.Write("  Choice: ");
            string startChoice = Console.ReadLine() ?? "2";

            HospitalRegistry registry = startChoice == "1"
                ? BuildDemoRegistry()
                : new HospitalRegistry();

            if (startChoice == "1")
            {
                Console.WriteLine();
                Console.WriteLine("  Demo registry loaded.");
                Console.WriteLine("  Doctors:  Petrenko (Cardiology), Kovalenko (Neurology), Melnyk (Surgery)");
                Console.WriteLine("  Patients: Shevchenko, Franko, Ukrainka");
                Console.WriteLine("  Visit plans and medical records pre-created.");
            }

            bool running = true;

            while (running)
            {
                PrintMenu();
                string input = Console.ReadLine() ?? "0";

                try
                {
                    switch (input.Trim())
                    {
                        case "1": HandleViewAllDoctors(registry); break;
                        case "2": HandleAddDoctor(registry); break;
                        case "3": HandleRemoveDoctor(registry); break;
                        case "4": HandleEditDoctor(registry); break;
                        case "5": HandleSearchDoctor(registry); break;
                        case "6": HandleViewAllPatients(registry); break;
                        case "7": HandleAddPatient(registry); break;
                        case "8": HandleRemovePatient(registry); break;
                        case "9": HandleSearchPatient(registry); break;
                        case "10": HandleAddMedicalRecord(registry); break;
                        case "11": HandleCloseMedicalRecord(registry); break;
                        case "12": HandleViewDoctorPlan(registry); break;
                        case "13": HandleAddVisitRecord(registry); break;
                        case "14": HandleRemoveVisitRecord(registry); break;
                        case "15": HandleViewAllPlans(registry); break;
                        case "16": HandleViewAllCardsByPatient(registry); break;
                        case "17": HandleViewOnePatientCard(registry); break;
                        case "18": HandleViewAllCardsByDoctor(registry); break;
                        case "19": HandleViewRecordsByDoctor(registry); break;
                        case "0":
                            Console.WriteLine("  Exiting.");
                            running = false;
                            break;
                        default:
                            Console.WriteLine("  Unknown option. Please enter a number from the menu.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("  Unexpected error: " + ex.Message);
                }

                if (running)
                {
                    Console.WriteLine();
                    Console.Write("  Press Enter to continue...");
                    Console.ReadLine();
                }
            }
        }
    }
}