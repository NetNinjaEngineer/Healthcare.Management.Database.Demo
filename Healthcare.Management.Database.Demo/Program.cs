using Healthcare.Management.Database.Demo.Data;

namespace Healthcare.Management.Database.Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new ApplicationDbContext())
            {
                //RunAppointmentHistoryView(context);

                //GetTotalAppointmentsForDepartments(context);

                //EmployeePerformanceSummary(context);

                //RunBirthdayCelebrationsView(context);

                //RunTopSalaryEmployeesView(context);

                //RunCurrentAppointmentsView(context);

                //SearchDoctorBy("JAM", context);

                RunPatientsWithCheckupCosts(context);

            }


            Console.ReadKey();
        }

        private static void RunAppointmentHistoryView(ApplicationDbContext context)
        {
            var patientAppointmentHistory = (from patient in context.Patients
                                             join appointment in context.Appointments
                                             on patient.Id equals appointment.PatientId into patientAppointments
                                             from appointment in patientAppointments.DefaultIfEmpty()
                                             group new { patient, appointment } by new { patient.FirstName, patient.LastName } into paGroup
                                             select new
                                             {
                                                 PatientName = string.Concat(paGroup.Key.FirstName, ' ', paGroup.Key.LastName),
                                                 TotalAppointments = paGroup.Count(x => x.appointment != null),
                                                 FirstAppointment = paGroup.Min(x => x.appointment.AppointmentDate),
                                                 LastAppointment = paGroup.Max(x => x.appointment.AppointmentDate)
                                             }).ToList();

            var queryFluentSyntax = context.Patients
                .Join(
                    context.Appointments,
                    patient => patient.Id,
                    appointment => appointment.PatientId,
                    (patient, appointment) => new { patient, appointment }
                )
                .GroupBy(
                    x => new { x.patient.FirstName, x.patient.LastName },
                    (key, group) => new
                    {
                        PatientName = string.Concat(key.FirstName, " ", key.LastName),
                        TotalAppointments = group.Count(x => x.appointment != null),
                        FirstAppointment = group.Min(x => x.appointment.AppointmentDate),
                        LastAppointment = group.Max(x => x.appointment.AppointmentDate)
                    }
                );


            foreach (var paHistory in patientAppointmentHistory)
                Console.WriteLine(
                    $"\nPatientName: {paHistory?.PatientName}" +
                    $"\nTotalAppointments: {paHistory?.TotalAppointments}" +
                    $"\nFirstAppointment: {paHistory?.FirstAppointment}" +
                    $"\nLastAppointment: {paHistory?.LastAppointment}");


        }

        public static void GetTotalAppointmentsForDepartments(ApplicationDbContext context)
        {
            var query = (from department in context.Departments
                         join employee in context.Employees
                         on department.Id equals employee.DepartmentId
                         join appointment in context.Appointments
                         on employee.Id equals appointment.EmployeeId
                         where appointment.AppointmentDate == DateOnly.FromDateTime(DateTime.Now)
                         group
                         new { department, employee, appointment }
                            by department.Name into grouped
                         select new
                         {
                             Department = grouped.Key,
                             TotalAppointments =
                                grouped.Where(
                                    x => x.appointment != null).Count()

                         }).ToList();


            foreach (var item in query)
                Console.WriteLine(
                    $"\nDepartment: {item.Department}" +
                    $"\nTotalAppointments: {item?.TotalAppointments}");

        }

        public static void EmployeePerformanceSummary(ApplicationDbContext context)
        {
            var employeePerformanceSummary = from appointment in context.Appointments
                                             join employee in context.Employees
                                             on appointment.EmployeeId equals employee.Id
                                             join patient in context.Patients
                                             on appointment.PatientId equals patient.Id
                                             where employee.JobTitle!.ToLower() == "doctor"
                                             group new { appointment, employee, patient } by
                                             new { employee.FirstName, employee.LastName, employee.JobTitle }
                                             into grouped
                                             select new
                                             {
                                                 Employee = string.Concat(
                                                     grouped.Key.FirstName, ' ', grouped.Key.LastName
                                                   ),
                                                 TotalAppointments = grouped.Count(x => x.appointment != null),
                                                 EmployeeJobTitle = grouped.Key.JobTitle,
                                                 Patients = grouped.Select(
                                                     x => string.Concat(
                                                         x.patient.FirstName, ' ', x.patient.LastName)
                                                     )
                                                 .ToList()
                                             };
            foreach (var empPerformance in employeePerformanceSummary)
                Console.WriteLine(
                    $"\n{empPerformance.Employee} ({empPerformance.EmployeeJobTitle})" +
                    $"\nTotalAppointments: {empPerformance?.TotalAppointments}" +
                    $"\nPatients: {string.Join(", ", empPerformance!.Patients)}");



        }

        public static void RunBirthdayCelebrationsView(ApplicationDbContext context)
        {
            var query = from employee in context.Employees
                        where employee.DateOfBirth.Month == DateTime.Now.Month
                        select new
                        {
                            EmployeeId = employee.Id,
                            EmployeeName = string.Concat(employee.LastName, ' ', employee.FirstName),
                            DateOfBirth = employee.DateOfBirth
                        };

            foreach (var employee in query)
                Console.WriteLine($"" +
                    $"\nEmployeeId: {employee.EmployeeId}" +
                    $"\nEmployeeName: {employee.EmployeeName}" +
                    $"\nDateOfBirth: {employee.DateOfBirth}");
        }

        public static void RunTopSalaryEmployeesView(ApplicationDbContext context)
        {
            var query = from employee in context.Employees
                        where employee.Salary == (context.Employees.Max(x => x.Salary))
                        select new
                        {
                            employee.Id,
                            employee.JobTitle,
                            employee.Salary,
                            EmployeeName = string.Concat(employee.FirstName, ' ', employee.LastName)
                        };

            foreach (var employee in query)
                Console.WriteLine($"" +
                    $"\nEmployeeId: {employee.Id}" +
                    $"\nEmployeeName: {employee.EmployeeName}" +
                    $"\nSalary: {employee.Salary:C}" +
                    $"\nJobTitle: {employee.JobTitle}");

        }

        public static void RunCurrentAppointmentsView(ApplicationDbContext context)
        {
            var query = from appointment in context.Appointments
                        join employee in context.Employees
                        on appointment.EmployeeId equals employee.Id
                        join patient in context.Patients
                        on appointment.PatientId equals patient.Id
                        where appointment.AppointmentDate == DateOnly.FromDateTime(DateTime.Now)
                        select new
                        {
                            appointment.Id,
                            appointment.AppointmentDate,
                            appointment.AppointmentTime,
                            appointment.Paid,
                            EmployeeName = string.Concat(employee.FirstName, ' ', employee.LastName),
                            Patient = string.Concat(patient.FirstName, ' ', patient.LastName)
                        };

            foreach (var appointment in query)
                Console.WriteLine($"" +
                    $"\nAppointmentId: {appointment.Id}" +
                    $"\nAppointmentDate: {appointment.AppointmentDate}" +
                    $"\nAppointmentTime: {appointment.AppointmentTime}" +
                    $"\nPaid: {appointment.Paid}" +
                    $"\nPatient: {appointment.Patient}" +
                    $"\nDoctor: {appointment.EmployeeName}");

        }

        public static void SearchDoctorBy(string doctorName, ApplicationDbContext context)
        {
            var doctors = context.Employees
                .Where(e => e.JobTitle == "Doctor" &&
                    (
                        e.FirstName!.ToLower().Contains(doctorName.ToLower()) ||
                        e.LastName!.ToLower().Contains(doctorName.ToLower())))
                .ToList();

            foreach (var employee in doctors)
            {
                Console.WriteLine(employee.FirstName);
            }

        }

        public static void RunPatientsWithCheckupCosts(ApplicationDbContext context)
        {
            var query = from employee in context.Employees
                        join department in context.Departments
                        on employee.DepartmentId equals department.Id
                        join appointment in context.Appointments
                        on employee.Id equals appointment.EmployeeId
                        join patient in context.Patients
                        on appointment.PatientId equals patient.Id
                        select new
                        {
                            Patient = string.Concat(patient.FirstName, " ", patient.LastName),
                            Doctor = string.Concat(employee.FirstName, " ", employee.LastName),
                            appointment.AppointmentDate,
                            appointment.AppointmentTime,
                            appointment.Paid,
                            patient.Gender,
                            CostofDiagnosis = department.CheckCost
                        };

            foreach (var item in query)
                Console.WriteLine($"" +
                    $"\nPatient: {item.Patient}" +
                       $"\nDoctor:  {item.Patient}" +
                    $"\nAppointmentTime: {item.AppointmentTime}" +
                    $"\nAppointmentDate: {item.AppointmentDate}" +
                    $"\nGender: {item.Gender}" +
                    $"\nPaid: {item.Paid}" +
                       $"\nCostofDiagnosis: {item.CostofDiagnosis}");
        }

    }
}
