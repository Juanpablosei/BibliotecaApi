using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.DTOs
{
    public class LibroCreacionDTO
    {
        [Required]
        public required string Titulo { get; set; }
        public List<int> AutoresIds { get; set; } = [];
    }
}
