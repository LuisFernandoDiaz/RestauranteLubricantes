using System;
using System.Collections.Generic;

namespace RestauranteLubricantes.Models;

public partial class Categorium
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Plato> Platos { get; set; } = new List<Plato>();
}
