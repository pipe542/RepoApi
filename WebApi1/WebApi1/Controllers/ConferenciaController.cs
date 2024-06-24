using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI1.Custom;
using WebAPI1.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using WebApi1.Models;
using WebApi1.Models.DTOs;



namespace WebApi1.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ConferenciaController : ControllerBase
    {
        private readonly ApiCrudContext _apiCrudContext;
        public ConferenciaController(ApiCrudContext apiCrudContext)
        {
            _apiCrudContext = apiCrudContext;
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> Crear([FromBody] ConferenciaDTO objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { isSuccess = false, message = "Datos de conferencia no válidos" });
            }

            try
            {
                var modeloConferencia = new Conferencia
                {
                    Espacio = objeto.Espacio,
                    NumeroPersonas = objeto.NumeroPersonas,
                    Fecha = objeto.Fecha,
                    Hora = objeto.Hora,
                    NumeroPonentes = objeto.NumeroPonentes,
                    Temas = objeto.Temas
                };

                await _apiCrudContext.Conferencia.AddAsync(modeloConferencia);
                await _apiCrudContext.SaveChangesAsync();

                if (modeloConferencia.IdConferencias != 0)
                    return StatusCode(StatusCodes.Status201Created, new { isSuccess = true, message = "Conferencia creada correctamente" });
                else
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "No se pudo crear la conferencia" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var lista = await _apiCrudContext.Conferencia.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }



        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var conferencia = await _apiCrudContext.Conferencia.FindAsync(id);
                if (conferencia == null)
                {
                    return NotFound(new { isSuccess = false, message = "Conferencia no encontrada" });
                }

                _apiCrudContext.Conferencia.Remove(conferencia);
                await _apiCrudContext.SaveChangesAsync();

                return Ok(new { isSuccess = true, message = "Conferencia eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }


        [HttpPut]
        [Route("Actualizar/{id}")]
        public async Task<IActionResult> ActualizarConferencia(int id, [FromBody] ConferenciaDTO objeto)
        {
            var conferencia = await _apiCrudContext.Conferencia.FindAsync(id);
            if (conferencia == null)
            {
                return NotFound(new { isSuccess = false, message = "Conferencia no encontrada" });
            }

            // Update fields if they are not null
            if (!string.IsNullOrEmpty(objeto.Espacio))
            {
                conferencia.Espacio = objeto.Espacio;
            }
            if (objeto.NumeroPersonas.HasValue)
            {
                conferencia.NumeroPersonas = objeto.NumeroPersonas.Value;
            }
            if (objeto.Fecha != null)
            {
                conferencia.Fecha =  objeto.Fecha;
            }
            if (objeto.Hora != null)
            {
                conferencia.Hora = objeto.Hora;
            }
            if (objeto.NumeroPonentes.HasValue)
            {
                conferencia.NumeroPonentes = objeto.NumeroPonentes.Value;
            }
            if (!string.IsNullOrEmpty(objeto.Temas))
            {
                conferencia.Temas = objeto.Temas;
            }

            _apiCrudContext.Conferencia.Update(conferencia);
            await _apiCrudContext.SaveChangesAsync();

            return Ok(new { isSuccess = true, message = "Conferencia actualizada correctamente" });
        }

    



    }
}
