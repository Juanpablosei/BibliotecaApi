using AutoMapper;
using BibliotecaApi.Datos;
using BibliotecaApi.DTOs;
using BibliotecaApi.Entidades;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosControllers : ControllerBase

    {
        public ComentariosControllers(ApplicationDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public ApplicationDbContext Context { get; }
        public IMapper Mapper { get; }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId)
        {
            var existeLibro = await Context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NoContent();
            }
            var comentarios = await Context.comentarios
                .Where(x => x.LibroId == libroId)
                .OrderByDescending(x => x.FechaPublicacion)
                .ToListAsync();

            return Mapper.Map<List<ComentarioDTO>>(comentarios);
        }

        [HttpGet("{id}", Name = "ObtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> Get (Guid id)
        {
            var comentario = await Context.comentarios.FirstOrDefaultAsync(x => x.Id == id);
            if(comentario is null)
            {
                return NotFound();  
            }
            return Mapper.Map<ComentarioDTO>(comentario);
        }
        [HttpPost]
        public async Task<ActionResult> Post(int libroId, ComentariosCreacionDTO comentarioCreacionDTO)
        {
            var existeLibro = await Context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NoContent();
            }
            var comentario = Mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            comentario.FechaPublicacion=DateTime.Now;
            Context.Add(comentario);    
            await Context.SaveChangesAsync();
            var comentarioDTO = Mapper.Map<ComentarioDTO>(comentario);
            return CreatedAtRoute("ObtenerComentario", new { id = comentario.Id, libroId },comentarioDTO);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Path(Guid id,int libroId, JsonPatchDocument<ComentarioPatchDTO> patchDOC)
        {
            if (patchDOC is null)
            {
                return BadRequest();
            }
            var existeLibro = await Context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();
            }

            var comentarioDB = await Context.comentarios.FirstOrDefaultAsync(x=>x.Id == id);
            if (comentarioDB is null)
            {
                return NotFound();
            }

            var comentarioPatchDTO = Mapper.Map<ComentarioPatchDTO>(comentarioDB);
            patchDOC.ApplyTo(comentarioPatchDTO, ModelState);
            var esValido = TryValidateModel(comentarioPatchDTO);
            if (!esValido)
            {
                return ValidationProblem();
            }
            Mapper.Map(comentarioPatchDTO, comentarioDB);
            await Context.SaveChangesAsync();
            return NoContent();


        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete (Guid id, int libroId)
        {
            var existeLibro = await Context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();
            }
            var registgroBorrados = await Context.comentarios.Where(x=>x.Id==id).ExecuteDeleteAsync();
            if(registgroBorrados==0)
            {
                return NotFound();  
            }
            return NoContent(); 
        }
    }   
}
