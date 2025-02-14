using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.DTOs
{
    public class LibroCreacionDTO
    {
        [Required]
        public required string Titulo { get; set; }
        public int AutorId { get; set; }
    }
}
