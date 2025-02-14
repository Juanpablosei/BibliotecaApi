using AutoMapper;
using BibliotecaApi.Datos;
using BibliotecaApi.DTOs;
using BibliotecaApi.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController: ControllerBase
    {
       
       
            private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext Context ,IMapper mapper)
        {
                context = Context;
                this.mapper = mapper;
        }
            [HttpGet]
            public async Task<IEnumerable<LibrosDTO>> Get()
            {
                var libros= await context.Libros.ToListAsync();
                var librosDto = mapper.Map<IEnumerable<LibrosDTO>>(libros);
                return librosDto;   
        }


            [HttpPost]
            public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)
            {
            var libro = mapper.Map<Libro>(libroCreacionDTO);
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);
            if (!existeAutor)
            {
                return BadRequest("No existe el autor");
            }
            context.Add(libro);
                await context.SaveChangesAsync();
                var libroDto = mapper.Map<LibrosDTO>(libro);
            return CreatedAtRoute("obtenerLibro", new { id = libro.Id }, libroDto);
        }


            [HttpGet("{id:int}",Name ="obtenerLibro")]
            public async Task<ActionResult<LibroConAutorDTO>> Get(int id)
            {
                var libro = await context.Libros
                .Include(x => x.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);
                if (libro is null)
                {
                    return NotFound();
                }

                var libroDto = mapper.Map<LibroConAutorDTO>(libro);
            return libroDto;
            }


            [HttpPut("{id:int}")]
            public async Task<ActionResult> Put(int id, LibroCreacionDTO libroCreacionDTO)
            {
                var libro = mapper.Map<Libro>(libroCreacionDTO);
                libro.Id = id;
                var existeAutor = await context.Autores.AnyAsync(x=>x.Id == libro.AutorId );
            if (!existeAutor)
            {
                return BadRequest("No existe el autor");
            }
            context.Update(libro);
            await context.SaveChangesAsync();
            return NoContent();
        }


            [HttpDelete("{id:int}")]
            public async Task<ActionResult> Delete(int id)
            {
                var registrosBorrados = await context.Libros.Where(x => x.Id == id).ExecuteDeleteAsync();
                if (registrosBorrados == 0)
                {
                    return NotFound();
                }
            return NoContent();
        }
        }
    
}
