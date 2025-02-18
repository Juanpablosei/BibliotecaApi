using BibliotecaApi.Entidades;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.DTOs
{
    public class ComentariosCreacionDTO
    {
        [Required]
        public required string Cuerpo { get; set; }
       
    }
}
