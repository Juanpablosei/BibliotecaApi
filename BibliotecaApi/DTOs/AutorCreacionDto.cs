using BibliotecaApi.Entidades;
using BibliotecaApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.DTOs
{
    public class AutorCreacionDto
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(150, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        [PrimeraLetraMayuscula]
        public required string Nombres { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(150, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        [PrimeraLetraMayuscula]
        public required string Apellidos { get; set; }
        public string? Identificacion { get; set; }
        public List<Libro> Libros { get; set; } = new List<Libro>();
    }
}
