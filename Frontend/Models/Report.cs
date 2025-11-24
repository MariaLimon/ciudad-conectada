namespace Frontend.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserDto? User { get; set; }
        public ServiceDto? Service { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class ServiceDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
    }
}