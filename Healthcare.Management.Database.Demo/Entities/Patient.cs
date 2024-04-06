using Healthcare.Management.Database.Demo.Entities.Common;

namespace Healthcare.Management.Database.Demo.Entities
{
    public class Patient : Person
    {
        public string? Email { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = [];
    }
}
