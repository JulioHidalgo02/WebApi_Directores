using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApi_Directores.Context;
using WebApi_Directores.Entitites;

namespace WebApi_Directores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectoresController : ControllerBase
    {
        private readonly DirectorContext _context;

        //Constructor de la clase con la inyección del contexto de la Base de datos
        public DirectoresController(DirectorContext directorContext)
        {
            _context = directorContext;
        }


        // GET: api/ObtenerDirectores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Director>>> ObtenerDirectores()
        {
            //Se comunica con la base de datos y trae todos los directores
            return await _context.Directores.ToListAsync();
        }

        // GET: api/ObtenerDirector/
        [HttpGet("{id}")]
        public async Task<ActionResult<Director>> ObtenerDirector(int id)
        {
            //Se comunica con la base de datos y trae el director que hace match con el id
            var director = await _context.Directores.FindAsync(id);

            if (director == null)
            {
                //Si no encuentra un director devuelve un NotFound
                return NotFound();
            }

            return director;
        }

        // POST: api/AgregarDirector
        [HttpPost]
        public async Task<ActionResult<Director>> AgregarDirector(Director director)
        {
            //Se comunica con la base de datos para agregar un director
            _context.Directores.Add(director);
            await _context.SaveChangesAsync();

            //retorna la consulta a la acción ObterDirector donde muestra el director creado recientemente.
            return CreatedAtAction("ObtenerDirector", new { id = director.Id }, director);
        }

        // PUT: api/EditarDirector/
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarDirector(int id, Director director)
        {
            if (id != director.Id)
            {
                // Si el id del parametro es dirente al id del objeto director se devuelve un badRequest
                return BadRequest();
            }
            //Se comiunica con la base de datos y trata de hacer el cambio con la nueva información
            _context.Entry(director).State = EntityState.Modified;

            try
            {
                //Guarda los cambios en la base de datos
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //Si el director existe se devuelve una excepcion sino se devuelve un not Found
                if (!DirectorExiste(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/EliminarDirector/
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarDirector(int id)
        {
            //se comunica con la base de datos para verificar que el id pertenece a una dupla de la tabla
            var director = await _context.Directores.FindAsync(id);
            if (director == null)
            {
                //Si no pertenece se devuelve un Not found
                return NotFound();
            }
            //Si existe se procede a borrar el director
            _context.Directores.Remove(director);
            await _context.SaveChangesAsync();

            //Se retorna un noContent
            return NoContent();
        }

        //Funcion para ver si el directore existe, devuelve un true si si existe y un false si no existe.
        private bool DirectorExiste(int id)
        {
            return _context.Directores.Any(e => e.Id == id);
        }
    }
}
