using System.Collections.Generic;
using MediatR;
using Hamburgueria.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Hamburgueria.Application.Queries.CalcularFrete
{
    public class CalcularFreteHandler : IRequestHandler<CalcularFreteQuery, double>
    {
        private readonly InMemoryDatabase _db;
        public CalcularFreteHandler(InMemoryDatabase db) => _db = db;

        public Task<double> Handle(CalcularFreteQuery request, CancellationToken cancellationToken)
        {
            var valor = _db.Fretes.TryGetValue(request.Bairro, out var v) ? v : throw new KeyNotFoundException("Bairro não encontrado");
            return Task.FromResult(valor);
        }
    }
}
