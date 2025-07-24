using Hamburgueria.Domain.Entities;
using System.Collections.Generic;

namespace Hamburgueria.Infrastructure.Data
{
    public class InMemoryDatabase
    {
        public List<Pedido> Pedidos { get; set; } = new();
        public Dictionary<string, double> Fretes { get; set; } = new()
        {
            { "Bairro A", 10.0 },
            { "Bairro B", 12.5 },
            { "Bairro C", 8.0 },
            { "Bairro D", 15.0 },
            { "Bairro E", 20.0 }
        };
        public Dictionary<string, double> Distancias { get; set; } = new()
        {
            { "Bairro A", 2.0 },
            { "Bairro B", 5.0 },
            { "Bairro C", 1.5 },
            { "Bairro D", 7.0 },
            { "Bairro E", 10.0 }
        };
    }
}
