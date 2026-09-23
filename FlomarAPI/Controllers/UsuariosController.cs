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
    }
}
