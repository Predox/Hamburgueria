using MediatR;
using Hamburgueria.Domain.Entities;
using Hamburgueria.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Hamburgueria.Application.Commands.CreatePedido
{
    public class CreatePedidoHandler : IRequestHandler<CreatePedidoCommand>
    {
        private readonly InMemoryDatabase _db;
        public CreatePedidoHandler(InMemoryDatabase db) => _db = db;

        public Task<Unit> Handle(CreatePedidoCommand request, CancellationToken cancellationToken)
        {
            var pedido = new Pedido
            {
                Id = request.Id,
                DataHora = request.DataHora,
                Bairro = request.Bairro,
                Ingredientes = request.Ingredientes
            };
            _db.Pedidos.Add(pedido);
            return Task.FromResult(Unit.Value);
        }
    }
}
