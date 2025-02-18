namespace BibliotecaApi.DTOs
{
    public class LibroConAutoresDTO: LibrosDTO
    {
        public List<AutorDTO> Autores { get; set; } = [];
    }
}
