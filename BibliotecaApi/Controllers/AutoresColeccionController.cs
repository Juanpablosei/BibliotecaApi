using System.Security.Cryptography.Xml;
using AutoMapper;
using BibliotecaApi.Datos;
using BibliotecaApi.DTOs;
using BibliotecaApi.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Controllers
{
    [ApiController]
    [Route("api/autores-coleccion")]
    public class AutoresColeccionController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresColeccionController( ApplicationDbContext context , IMapper mapper ) 
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{ids}", Name = "ObtenerAutoresPorIds")]
        public async Task<ActionResult<List<AutorConLibroDTO>>> Get(string ids)
        {
            var idsColeccion = new List<int>();
            foreach (var id in ids.Split(",")) 
            {
                if(int.TryParse(id,out int idint))
                {
                    idsColeccion.Add(idint);
                }
            }
            if(!idsColeccion.Any())
            {
                ModelState.AddModelError(nameof(ids), "Ningu id fue encontrado");
                return ValidationProblem();
            }
            var autores = await context.Autores
                .Include(x => x.Libros)
                    .ThenInclude(x => x.Libro)
                .Where(x => idsColeccion.Contains(x.Id))
                .ToListAsync();
            if(autores.Count != idsColeccion.Count)
            {
                return NotFound();
            }
            var autoresDTO = mapper.Map<List<AutorConLibroDTO>>(autores);
            return autoresDTO;

        }
        

        [HttpPost]
        public async Task<ActionResult> Post (IEnumerable<AutorCreacionDto>autoresCreacionDTO)
        {
            var autores= mapper.Map<IEnumerable<Autor>>(autoresCreacionDTO);
            context.AddRange(autores);  
            await context.SaveChangesAsync();

            var autoresDT0 = mapper.Map<IEnumerable<AutorDTO>>(autores);
            var ids = autores.Select(x => x.Id);
            var idsString = string.Join(",", ids);
            return CreatedAtRoute("ObtenerAutoresPorIds", new { ids = idsString }, autoresDT0);
        }


        
    }
}
