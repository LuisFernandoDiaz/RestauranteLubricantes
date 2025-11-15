using RestauranteLubricantes.Models.Dtos;

namespace RestauranteLubricantes.Service
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDto>> ListarCategoriaAsync();
        Task<bool> CrearCategoriaAsync(CrearCategoriaDto dto);
        Task<List<CategoriaDto>> BuscarPorNombreAsync(BuscarCategoriaDto dto);
    }
}
