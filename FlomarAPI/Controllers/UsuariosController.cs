using FlomarAPI.Data;
using FlomarAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlomarAPI.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly FlomarContext _context;

        public UsuariosController(FlomarContext context)
        {
            _context = context;
        }

        // GET: api/usuarios  (para la pantalla de usuarios del administrador)
        [HttpGet]
        public async Task<ActionResult<List<UsuarioListaDto>>> Listar()
        {
            return await _context.Usuarios
                .OrderBy(u => u.id_usuario)
                .Select(u => new UsuarioListaDto
                {
                    id_usuario = u.id_usuario,
                    Nombre_Usuario = u.Nombre_Usuario,
                    nombre_completo = u.nombre_completo,
                    id_rol = u.id_rol,
                    id_estado_usuario = u.id_estado_usuario,
                    fecha_creacion = u.fecha_creacion,
                    ultimo_acceso = u.ultimo_acceso
                })
                .ToListAsync();
        }

        // PUT: api/usuarios/cambiar-estado/5  (Endpoint dedicado para alternar el estado y guardar en MySQL)
        [HttpPut("cambiar-estado/{id}")]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Invertimos el estado directamente: si es 1 pasa a 2, si es 2 (u otro) pasa a 1
            usuario.id_estado_usuario = (usuario.id_estado_usuario == 1) ? 2 : 1;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}