using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteLubricantes.Custom;
using RestauranteLubricantes.Models;
using RestauranteLubricantes.Models.Dtos;



namespace RestauranteLubricantes.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous] //cualquiera puede ingresar (registrar y login)
    [ApiController]
    public class AccesoController : ControllerBase
    {

        private readonly PolleriaLubricantesContext _dbPruebaContext;
        private readonly Utilidades _utilidades;


        public AccesoController(PolleriaLubricantesContext dbPruebaContext, Utilidades utilidades)
        {
            _dbPruebaContext = dbPruebaContext;
            _utilidades = utilidades;
        }

        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(UsuarioDto objeto)
        {

            var existeCorreo = await _dbPruebaContext.Usuarios
                        .AnyAsync(u => u.Correo == objeto.Correo);

            if (existeCorreo)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = "El correo ya está registrado"
                });
            }


            var modeloUsuario = new Usuario
            {
                Nombres = objeto.Nombres,
                Correo = objeto.Correo,
                Clave = _utilidades.encriptarSHA256(objeto.Clave) //recordar que la clave fue encriptada
            };

            await _dbPruebaContext.Usuarios.AddAsync(modeloUsuario);//aqui se esta guardando
            await _dbPruebaContext.SaveChangesAsync();//aqui se esta guardando

            {
                return Ok(new
                {
                    isSuccess = true,
                    message = "Usuario registrado correctamente",
                    fecha = modeloUsuario.FechaRegistro
                });
            }
            
        }




        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto objeto)
        {
            //aqui estamos validando que lo insertado  sea igual clave y correo que previamente fueron registrados en la BD
            var usuarioEncontrado = await _dbPruebaContext.Usuarios
                                       .Where(u =>
                                       u.Correo == objeto.Correo &&
                                       u.Clave == _utilidades.encriptarSHA256(objeto.Clave)
                                       ).FirstOrDefaultAsync(); //esto devuelve un true si lo encuentra, y si no un false

            if (usuarioEncontrado == null)//si es null retorna nada
            {
                return Unauthorized(new
                {
                    isSuccess = false,
                    token = "",
                    message = "Usuario no encontrado"
                });
            }
            else//si encuentra devuelve el toque
            {
                return  Ok(new
                {
                    isSuccess = true,
                    token = _utilidades.generarJWT(usuarioEncontrado),
                    message = "Usuario autenticado correctamente",
                    registro = usuarioEncontrado.FechaRegistro
                });
            }
        }



        //esto es para validar el token
        [HttpGet]
        [Route("ValidarToken")]
        public IActionResult ValidarToken([FromQuery] string token)
        {
            bool resultado = _utilidades.validarToken(token);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = resultado });
        }



    }
}
