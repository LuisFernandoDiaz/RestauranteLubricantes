using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteLubricantes.Models;

namespace RestauranteLubricantes.Controllers
{
    [Route("api/[controller]")]
    [Authorize] // con esto solo entraran usuarios autorizados ...... "AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme" este codigo es solo si hay muchas autenticaciones
    [ApiController]
    public class CategoriaController : ControllerBase
    {


        public readonly PolleriaLubricantesContext _dbpruebaContext;
        public CategoriaController(PolleriaLubricantesContext dbpruebaContext)
        {
            _dbpruebaContext = dbpruebaContext;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var lista = await _dbpruebaContext.Categoria.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }


    }
}
