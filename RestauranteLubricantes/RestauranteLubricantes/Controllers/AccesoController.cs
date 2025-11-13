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
            var modeloUsuario = new Usuario
            {
                Nombres = objeto.Nombres,
                Correo = objeto.Correo,
                Clave = _utilidades.encriptarSHA256(objeto.Clave) //recordemos que la clave fue encriptada
            };

            await _dbPruebaContext.Usuarios.AddAsync(modeloUsuario);//aqui se esta guardando
            await _dbPruebaContext.SaveChangesAsync();//aqui se esta guardando

            if (modeloUsuario.Id != 0)
            {
                return Ok(new
                {
                    isSuccess = true,
                    message = "Usuario registrado correctamente"
                });
            }
            else
            {
                return Unauthorized(new
                {
                    isSuccess = false,
                    message = "Registro denegado"
                });
            }
        }




        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto objeto)
        {
            //aqui estamos validando que lo inseertado sea igual clave y correo que previamente fueron registrados en la BD
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
                    message = "Usuario no autenticado"
                });
            }
            else//si encuentra devuelve el toque
            {
                return  Ok(new
                {
                    isSuccess = false,
                    token = _utilidades.generarJWT(usuarioEncontrado),
                    message = "Usuario autenticado correctamente"
                });
            }
        }
    }
}
