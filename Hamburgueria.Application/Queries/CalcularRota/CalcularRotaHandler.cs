using MediatR;
using Hamburgueria.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hamburgueria.Application.Queries.CalcularRota
{
    public class CalcularRotaHandler : IRequestHandler<CalcularRotaQuery, List<int>>
    {
        private readonly InMemoryDatabase _db;
        public CalcularRotaHandler(InMemoryDatabase db) => _db = db;

        public Task<List<int>> Handle(CalcularRotaQuery request, CancellationToken cancellationToken)
        {
            // Ordena por DataHora e distância para otimizar rota
            var map = _db.Distancias;
            var ordered = request.Itens
                .OrderBy(i => i.DataHora)
                .ThenBy(i => map.TryGetValue(i.Bairro, out var d) ? d : double.MaxValue)
                .Select(i => i.PedidoId)
                .ToList();
            return Task.FromResult(ordered);
        }
    }
}
