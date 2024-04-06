namespace Healthcare.Management.Database.Demo.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        public DateOnly AppointmentDate { get; set; }

        public TimeOnly AppointmentTime { get; set; }

        public string? Paid { get; set; } = "UNPAID";

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public int PatientId { get; set; }

        public Patient Patient { get; set; }
    }
}
