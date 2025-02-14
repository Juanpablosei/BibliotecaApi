using BibliotecaApi.Entidades;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.DTOs
{
    public class LibrosDTO
    {
        [Required]
        public required string Titulo { get; set; }
        public int Id { get; set; }
    }
}
