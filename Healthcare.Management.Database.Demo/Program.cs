using Healthcare.Management.Database.Demo.Data;

namespace Healthcare.Management.Database.Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new ApplicationDbContext())
            {
                RunAppointmentHistoryView(context);

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

            foreach (var paHistory in patientAppointmentHistory)
                Console.WriteLine(
                    $"\nPatientName: {paHistory?.PatientName}" +
                    $"\nTotalAppointments: {paHistory?.TotalAppointments}" +
                    $"\nFirstAppointment: {paHistory?.FirstAppointment}" +
                    $"\nLastAppointment: {paHistory?.LastAppointment}");


        }
    }
}
