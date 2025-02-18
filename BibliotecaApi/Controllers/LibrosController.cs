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
            if(libroCreacionDTO is null || libroCreacionDTO.AutoresIds.Count==0 )
            {
                ModelState.AddModelError(nameof(libroCreacionDTO.AutoresIds), "No se puede crear un libro sin autores");
                return ValidationProblem();
            }

            var autoresIdsExisten = await context.Autores.Where(x=> libroCreacionDTO.AutoresIds.Contains(x.Id))
                .Select(x=>x.Id).ToListAsync();

            if(autoresIdsExisten.Count != libroCreacionDTO?.AutoresIds.Count)
            {
                var autoresNoExisten = libroCreacionDTO.AutoresIds.Except(autoresIdsExisten);
                var autoresNoExistenString= string.Join(",", autoresNoExisten);
                var mensajeErrror = $"Los siguientes autores no existen : {autoresNoExistenString}";
                ModelState.AddModelError(nameof(libroCreacionDTO.AutoresIds), mensajeErrror);
                return ValidationProblem(); 
            }


            var libro = mapper.Map<Libro>(libroCreacionDTO);
            AsignarOrdenAutores(libro);
            context.Add(libro);
                await context.SaveChangesAsync();
                var libroDto = mapper.Map<LibrosDTO>(libro);
            return CreatedAtRoute("obtenerLibro", new { id = libro.Id }, libroDto);
        }

        private void AsignarOrdenAutores(Libro libro)
        {
            if(libro.Autores is not null)
            {
                for(int i=0; i <libro.Autores.Count; i++)
                {
                    libro.Autores[i].Orden = i;
                }
            }

        }
        

            [HttpGet("{id:int}",Name ="obtenerLibro")]
            public async Task<ActionResult<LibroConAutoresDTO>> Get(int id)
            {
                var libro = await context.Libros
                .Include(x => x.Autores)
                    .ThenInclude(x=> x.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);
                if (libro is null)
                {
                    return NotFound();
                }

                var libroDto = mapper.Map<LibroConAutoresDTO>(libro);
            return libroDto;
            }

        
            [HttpPut("{id:int}")]
            public async Task<ActionResult> Put(int id, LibroCreacionDTO libroCreacionDTO)
            {

            if (libroCreacionDTO is null || libroCreacionDTO.AutoresIds.Count == 0)
            {
                ModelState.AddModelError(nameof(libroCreacionDTO.AutoresIds), "No se puede crear un libro sin autores");
                return ValidationProblem();
            }

            var autoresIdsExisten = await context.Autores.Where(x => libroCreacionDTO.AutoresIds.Contains(x.Id))
                .Select(x => x.Id).ToListAsync();

            if (autoresIdsExisten.Count != libroCreacionDTO?.AutoresIds.Count)
            {
                var autoresNoExisten = libroCreacionDTO.AutoresIds.Except(autoresIdsExisten);
                var autoresNoExistenString = string.Join(",", autoresNoExisten);
                var mensajeErrror = $"Los siguientes autores no existen : {autoresNoExistenString}";
                ModelState.AddModelError(nameof(libroCreacionDTO.AutoresIds), mensajeErrror);
                return ValidationProblem();
            }


            var libroDB = await context.Libros.Include(x=>x.Autores).FirstOrDefaultAsync(x=>x.Id==id);
            if (libroDB is null)
            {
                return Ok();  
            }

            libroDB = mapper.Map(libroCreacionDTO, libroDB);
            AsignarOrdenAutores(libroDB);
           
           
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
