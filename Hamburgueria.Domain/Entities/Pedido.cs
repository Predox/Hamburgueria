using System;
using System.Collections.Generic;

namespace Hamburgueria.Domain.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public string Bairro { get; set; }
        public List<string> Ingredientes { get; set; } = new();
    }
}
