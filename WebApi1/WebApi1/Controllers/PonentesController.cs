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
    public class PonentesController : ControllerBase
    {
        private readonly ApiCrudContext _apiCrudContext;
        public PonentesController(ApiCrudContext apiCrudContext)
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
            var lista = await _apiCrudContext.Ponentes.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }




        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var ponentes = await _apiCrudContext.Ponentes.FindAsync(id);
                if (ponentes == null)
                {
                    return NotFound(new { isSuccess = false, message = "Conferencia no encontrada" });
                }

                _apiCrudContext.Ponentes.Remove(ponentes);
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
        public async Task<IActionResult> ActualizarConferencia(int id, [FromBody] PonentesDTO objeto)
        {
            var ponente = await _apiCrudContext.Ponentes.FindAsync(id);
            if (ponente == null)
            {
                return NotFound(new { isSuccess = false, message = "Conferencia no encontrada" });
            }

            if (!string.IsNullOrEmpty(objeto.Apellidos))
            {
                ponente.Apellidos = objeto.Apellidos;
            }
            if (!string.IsNullOrEmpty(objeto.Documento))
            {
                ponente.Documento = objeto.Documento;
            }
            if (!string.IsNullOrEmpty(objeto.TipoDocumento))
            {
                ponente.TipoDocumento = objeto.TipoDocumento;
            }
            if (!string.IsNullOrEmpty(objeto.Correo))
            {
                ponente.Correo = objeto.Correo;
            }
            if (!string.IsNullOrEmpty(objeto.Celular))
            {
                ponente.Celular = objeto.Celular;
            }
            if (!string.IsNullOrEmpty(objeto.Ciudad))
            {
                ponente.Ciudad = objeto.Ciudad;
            }
            if (!string.IsNullOrEmpty(objeto.Departamento))
            {
                ponente.Departamento = objeto.Departamento;
            }
            if (!string.IsNullOrEmpty(objeto.Empresa))
            {
                ponente.Empresa = objeto.Empresa;
            }

            _apiCrudContext.Ponentes.Update(ponente);
            await _apiCrudContext.SaveChangesAsync();

            return Ok(new { isSuccess = true, message = "Conferencia actualizada correctamente" });
        }






    }
}
