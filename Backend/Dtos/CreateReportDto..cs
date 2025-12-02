namespace Backend.Dtos
{
    // Esta clase coincide exactamente con el NewReport de tu app MAUI
    public class CreateReportDto
    {
        public int UserId { get; set; }
        public int ServiceId1 { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Estado { get; set; } = "Enviado";
        public string Location { get; set; } = string.Empty;
    }
}