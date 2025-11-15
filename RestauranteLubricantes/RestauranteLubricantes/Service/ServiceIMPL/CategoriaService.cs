
using Microsoft.EntityFrameworkCore;
using RestauranteLubricantes.Models;
using RestauranteLubricantes.Models.Dtos;

namespace RestauranteLubricantes.Service.ServiceIMPL
{
    public class CategoriaService : ICategoriaService
    {

        private readonly PolleriaLubricantesContext _context;

        public CategoriaService(PolleriaLubricantesContext context)
        {
            _context = context;
        }


        //metodos

        public async Task<List<CategoriaDto>> BuscarPorNombreAsync(BuscarCategoriaDto dto)
        {
            return await _context.Categoria
          .Where(p => p.Nombre.Contains(dto.Nombre))
          .Select(p => new CategoriaDto
          {
              Id = p.Id,
              Nombre = p.Nombre,
              Descripcion = p.Descripcion
          }).ToListAsync();
        }

        public async Task<bool> CrearCategoriaAsync(CrearCategoriaDto dto)
        {
            var categoria = new Categorium
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };

            _context.Categoria.Add(categoria);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CategoriaDto>> ListarCategoriaAsync()
        {
            return await _context.Categoria
         .Select(p => new CategoriaDto
         {
             Id = p.Id,
             Nombre = p.Nombre,
             Descripcion = p.Descripcion
         }).ToListAsync();
        }
    }
}
