namespace BibliotecaApi.DTOs
{
    public class AutorConLibroDTO: AutorDTO
    {
        public List<LibrosDTO> Libros { get; set; } = new List<LibrosDTO>();
    }
}
