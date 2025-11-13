using System;
using System.Collections.Generic;

namespace RestauranteLubricantes.Models;

public partial class DetallePedido
{
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public int PlatoId { get; set; }

    public int Cantidad { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;

    public virtual Plato Plato { get; set; } = null!;
}
