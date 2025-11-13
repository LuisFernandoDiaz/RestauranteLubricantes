using System;
using System.Collections.Generic;

namespace RestauranteLubricantes.Models;

public partial class Pedido
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public DateTime? FechaPedido { get; set; }

    public decimal? Total { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual Usuario Usuario { get; set; } = null!;
}
