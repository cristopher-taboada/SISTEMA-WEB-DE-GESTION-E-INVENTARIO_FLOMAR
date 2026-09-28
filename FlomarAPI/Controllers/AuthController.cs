using FlomarAPI.Data;
using FlomarAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlomarAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly FlomarContext _context;

        public AuthController(FlomarContext context)
        {
            _context = context;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.Nombre_Usuario == request.usuario &&
                    u.Contraseña_hash == request.password);

            if (usuario == null)
            {
                return Unauthorized(new RespuestaApi { mensaje = "Datos incorrectos" });
            }

            // NUEVA VALIDACIÓN: Verificar si el usuario está inactivo (distinto de 1)
            if (usuario.id_estado_usuario != 1)
            {
                return Unauthorized(new RespuestaApi { mensaje = "Tu cuenta se encuentra inactiva. Contacta al administrador." });
            }

            return Ok(new LoginResponse
            {
                id_usuario = usuario.id_usuario,
                nombre_completo = usuario.nombre_completo,
                id_rol = usuario.id_rol
            });
        }
    }
}