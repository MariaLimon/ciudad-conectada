namespace Backend.Dtos
{
    public class NotaInternaCreateDto
    {
        public int UserId { get; set; }         // Usuario que crea la nota
        public string Description { get; set; } = string.Empty;
    }
}
