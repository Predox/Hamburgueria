using MediatR;
using System;
using System.Collections.Generic;

namespace Hamburgueria.Application.Commands.CreatePedido
{
    public record CreatePedidoCommand(int Id, DateTime DataHora, string Bairro, List<string> Ingredientes) : IRequest;
}
