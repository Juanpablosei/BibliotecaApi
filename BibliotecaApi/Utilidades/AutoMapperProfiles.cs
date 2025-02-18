using AutoMapper;
using BibliotecaApi.DTOs;
using BibliotecaApi.Entidades;

namespace BibliotecaApi.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Autor, AutorDTO>()
                .ForMember(dto => dto.NombreCompleto, config => config.MapFrom(autor => MapearNombreCompleto(autor)));


            CreateMap<Autor, AutorConLibroDTO>()
                 .ForMember(dto => dto.NombreCompleto, config => config.MapFrom(autor => MapearNombreCompleto(autor)));
            CreateMap<AutorCreacionDto, Autor>();

            CreateMap< Libro,LibrosDTO>();
            CreateMap<Autor, AutorPathDTO>().ReverseMap();

            CreateMap<LibroCreacionDTO, Libro>();

            CreateMap<Libro,LibroConAutorDTO>()
                .ForMember(dto => dto.AutorNombre, config => config.MapFrom(ent => MapearNombreCompleto(ent.Autor!)));

            CreateMap<ComentariosCreacionDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
            CreateMap<ComentarioPatchDTO,Comentario>().ReverseMap();    
        }

        private string MapearNombreCompleto(Autor autor)
        {
            return $"{autor.Nombres} {autor.Apellidos}";
        }

    }
}
