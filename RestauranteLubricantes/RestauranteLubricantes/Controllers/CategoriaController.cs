using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteLubricantes.Models;
using RestauranteLubricantes.Models.Dtos;
using RestauranteLubricantes.Service;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RestauranteLubricantes.Controllers
{
    [Route("api/[controller]")]
    [Authorize] // con esto solo entraran usuarios autorizados ...... "AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme" este codigo es solo si hay muchas autenticaciones
    [ApiController]
    public class CategoriaController : ControllerBase
    {

        public readonly ICategoriaService _dbpruebaContext;

        public CategoriaController(ICategoriaService dbpruebaContext)
        {
            _dbpruebaContext = dbpruebaContext;
        }



        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            //con el try catch verificamos los errores
            try
            {
                var lista = await _dbpruebaContext.ListarCategoriaAsync();

                if (lista == null)
                    //esta da una respuesta http 404 
                    return NotFound(new
                    {
                        estado = "error",
                        fecha = DateTime.UtcNow,
                        mensaje = $"No existe Lista en Categoria"
                    });

                //esta da la respuesta 200 (ok)
                return StatusCode(StatusCodes.Status200OK, new { value = lista });


            }
            catch (Exception ex)
            {
                //esta da una respuesta http 500  (error)
                return StatusCode(500, new
                {
                    estado = "error",
                    mensaje = ex.Message
                });
            }


        }


        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> Crear([FromBody] CrearCategoriaDto dto)
        {
            try
            {
                var nuevo = await _dbpruebaContext.CrearCategoriaAsync(dto);
                //esta da una respuesta http 201 (create)
                return CreatedAtAction(nameof(Crear), new
                {
                    estado = "ok",
                    fecha = DateTime.UtcNow,
                    mensaje = "Categoria Creada Exitosamente"
                });
            }
            catch (Exception ex)
            {
                //esta da una respuesta http 500  (error)
                return StatusCode(500, new
                {
                    estado = "error",
                    fecha = DateTime.UtcNow,
                    mensaje = ex.Message
                });
            }
        }



        [HttpPost]
        [Route("Buscar")]
        public async Task<IActionResult> Buscar([FromBody] BuscarCategoriaDto dto)
        {
            try
            {
                var resultados = await _dbpruebaContext.BuscarPorNombreAsync(dto);
                if (resultados == null)
                    //esta da una respuesta http 404 
                    return NotFound(new
                    {
                        estado = "error",
                        fecha = DateTime.UtcNow,
                        mensaje = $"Producto {dto} no encontrado"
                    });

                //esta da una respuesta http 200 
                return Ok(new
                {
                    estado = "ok",
                    fecha = DateTime.UtcNow,
                    producto =  resultados
                });
            }
            catch (Exception ex)
            {
                //esta da una respuesta http 500 
                return StatusCode(500, new
                {
                    estado = "error",
                    fecha = DateTime.UtcNow,
                    mensaje = ex.Message
                });
            }
        }


    }
}
