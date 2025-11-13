using System;
using System.Collections.Generic;

namespace RestauranteLubricantes.Models;

public partial class Plato
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public string? Origen { get; set; }

    public string? Descripcion { get; set; }

    public int CategoriaId { get; set; }

    public virtual Categorium Categoria { get; set; } = null!;

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();
}
