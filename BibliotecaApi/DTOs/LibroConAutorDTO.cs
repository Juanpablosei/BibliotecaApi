namespace BibliotecaApi.DTOs
{
    public class LibroConAutorDTO: LibrosDTO
    {
        public int AutorId { get; set; }    
        public required string AutorNombre { get; set; }    
    }
}
