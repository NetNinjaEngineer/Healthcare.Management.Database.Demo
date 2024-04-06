namespace Healthcare.Management.Database.Demo.Entities.Common
{
    public abstract class Person
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Phone { get; set; }

        public string? Gender { get; set; }
    }
}
