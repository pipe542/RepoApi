using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI1.Custom;
using WebAPI1.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using WebApi1.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace WebAPI1.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly ApiCrudContext _apiCrudContext;
        private readonly Utilidades _utilidades;
        public AccesoController(ApiCrudContext apiCrudContext, Utilidades utilidades)
        {
            _apiCrudContext = apiCrudContext;
            _utilidades = utilidades;
        }

        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(UsuarioDTO objeto)
        {

            var modeloUsuario = new Usuario
            {
                Nombre = objeto.Nombre,
                Correo = objeto.Correo,
                Clave = _utilidades.encriptarSHA256(objeto.Clave)
            };

            await _apiCrudContext.Usuarios.AddAsync(modeloUsuario);
            await _apiCrudContext.SaveChangesAsync();

            if (modeloUsuario.IdUsuario != 0)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            else
                return StatusCode(StatusCodes.Status404NotFound, new { isSuccess = false });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO objeto)
        {
            var usuarioEncontrado = await _apiCrudContext.Usuarios
                                                    .Where(u =>
                                                        u.Correo == objeto.Correo &&
                                                        u.Clave == _utilidades.encriptarSHA256(objeto.Clave)
                                                      ).FirstOrDefaultAsync();

            if (usuarioEncontrado == null)
                return StatusCode(StatusCodes.Status404NotFound, new { isSuccess = false, token = "" , info ="usuario no existe"});
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidades.generarJWT(usuarioEncontrado) });
        }

        [HttpGet]
        [Route("Users")]
        public async Task<IActionResult> Lista()
        {
            var lista = await _apiCrudContext.Usuarios.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }


        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var usuario = await _apiCrudContext.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(new { isSuccess = false, message = "Usuario no encontrado" });
            }

            _apiCrudContext.Usuarios.Remove(usuario);
            await _apiCrudContext.SaveChangesAsync();

            return Ok(new { isSuccess = true, message = "Usuario eliminado correctamente" });
        }

        
        [HttpPut]
        [Route("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UsuarioDTO model)
        {
            var usuario = await _apiCrudContext.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(new { isSuccess = false, message = "Usuario no encontrado" });
            }

            // Update fields if they are not null
            if (!string.IsNullOrEmpty(model.Nombre))
            {
                usuario.Nombre = model.Nombre;
            }
            if (!string.IsNullOrEmpty(model.Correo))
            {
                usuario.Correo = model.Correo;
            }
            if (!string.IsNullOrEmpty(model.Clave))
            {
                // Assuming Clave is stored encrypted
                usuario.Clave = EncryptPassword(model.Clave);
            }

            _apiCrudContext.Usuarios.Update(usuario);
            await _apiCrudContext.SaveChangesAsync();

            return Ok(new { isSuccess = true, message = "Usuario actualizado correctamente" });
        }

        private string EncryptPassword(string password)
        {
            // Implement your password encryption logic here
            // Example: using a simple hash algorithm (not recommended for production)
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return System.BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }


        



    }
}