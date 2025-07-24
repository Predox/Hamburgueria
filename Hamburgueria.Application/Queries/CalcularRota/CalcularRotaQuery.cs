using MediatR;
using System;
using System.Collections.Generic;

namespace Hamburgueria.Application.Queries.CalcularRota
{
    public record CalcularRotaQuery(List<(int PedidoId, string Bairro, DateTime DataHora)> Itens) : IRequest<List<int>>;
}
