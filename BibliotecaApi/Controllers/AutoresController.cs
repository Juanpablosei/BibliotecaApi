using AutoMapper;
using Azure;
using BibliotecaApi.Datos;
using BibliotecaApi.DTOs;
using BibliotecaApi.Entidades;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public AutoresController(ApplicationDbContext  Context,IMapper mapper)
        {
            context = Context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task< IEnumerable<AutorDTO>>Get()
        {
            var autores = await context.Autores.ToListAsync();
            var autoresDto = mapper.Map<IEnumerable<AutorDTO>>(autores);

            return autoresDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post(AutorCreacionDto autorCracionDTO)
        {   
            var autor = mapper.Map<Autor>(autorCracionDTO); 
            context.Add(autor);
            await context.SaveChangesAsync();
            var autorDTO = mapper.Map<AutorDTO>(autor);

            return  CreatedAtRoute("ObtenerAutor", new { id = autor.Id }, autorDTO);

        }
        [HttpGet("{id:int}",Name ="ObtenerAutor")]
        public async Task<ActionResult<AutorConLibroDTO>> Get(int id)
        {
            var autor = await context.Autores
                .Include(x => x.Libros)
                .FirstOrDefaultAsync(x => x.Id == id);
            if(autor is null)
            {
                return NotFound();
            }
            var autorDto = mapper.Map<AutorConLibroDTO>(autor);
            return autorDto;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult>Put (int id,AutorCreacionDto autorCreacionDto)
        {
            var autor = mapper.Map<Autor>(autorCreacionDto);
            autor.Id = id;
            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent(); 
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Path (int id,JsonPatchDocument<AutorPathDTO>patchDOC)
        {
            if(patchDOC is null)
            {
                return BadRequest();
            }
            var autorDb= await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (autorDb is null)
            {
                return NotFound();
            }
            var autorPatchDTO= mapper.Map<AutorPathDTO>(autorDb);
            patchDOC.ApplyTo(autorPatchDTO, ModelState);
            var esValido = TryValidateModel(autorPatchDTO);
            if(!esValido)
            {
                return ValidationProblem();
            }
            mapper.Map(autorPatchDTO,autorDb);
            await context.SaveChangesAsync();
            return NoContent();


        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
           var registrosBorrados = await context.Autores.Where(x => x.Id == id).ExecuteDeleteAsync();

            if (registrosBorrados == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
