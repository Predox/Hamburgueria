using MediatR;
using Microsoft.AspNetCore.Mvc;
using Hamburgueria.Application.Commands.CreatePedido;
using Hamburgueria.Application.Queries.CalcularFrete;
using Hamburgueria.Application.Queries.CalcularRota;
using Hamburgueria.Domain.Entities;
using Hamburgueria.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hamburgueria.WebApi.Controllers
{
    // DTO para receber o payload de rota
    public class CalcularRotaRequestDto
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public string Bairro { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly InMemoryDatabase _db;

        public PedidosController(IMediator mediator, InMemoryDatabase db)
        {
            _mediator = mediator;
            _db = db;
        }

        [HttpPost("criar")]
        public async Task<IActionResult> Criar([FromBody] CreatePedidoCommand cmd)
        {
            await _mediator.Send(cmd);
            return Ok();
        }

        [HttpPost("criar-em-lote")]
        public async Task<IActionResult> CriarEmLote([FromBody] List<CreatePedidoCommand> comandos)
        {
            foreach (var cmd in comandos)
            {
                await _mediator.Send(cmd);
            }
            return Ok();
        }

        [HttpGet("frete/{bairro}")]
        public async Task<IActionResult> Frete(string bairro)
        {
            var valor = await _mediator.Send(new CalcularFreteQuery(bairro));
            return Ok(valor);
        }

        [HttpPost("rota")]
        public async Task<IActionResult> Rota([FromBody] List<CalcularRotaRequestDto> itens)
        {
            var input = itens
                .Select(i => (PedidoId: i.Id, Bairro: i.Bairro, DataHora: i.DataHora))
                .ToList();

            var orderedIds = await _mediator.Send(new CalcularRotaQuery(input));

            var orderedPedidos = orderedIds
                .Select(id => _db.Pedidos.FirstOrDefault(p => p.Id == id))
                .Where(p => p != null)
                .ToList();

            var distanciasMap = _db.Distancias;
            var fretesMap = _db.Fretes;

            double distanciaTotal = 0;
            double valorTotalFrete = 0;
            string bairroAnterior = null;

            foreach (var pedido in orderedPedidos)
            {
                var bairroAtual = pedido.Bairro;

                valorTotalFrete += fretesMap.TryGetValue(bairroAtual, out var frete) ? frete : 0;

                if (bairroAnterior == null)
                {
                    distanciaTotal += distanciasMap.TryGetValue(bairroAtual, out var d0) ? d0 : 0;
                }
                else if (bairroAtual != bairroAnterior)
                {
                    distanciaTotal += distanciasMap.TryGetValue(bairroAtual, out var d) ? d : 0;
                }

                bairroAnterior = bairroAtual;
            }

            if (bairroAnterior != null)
            {
                distanciaTotal += distanciasMap.TryGetValue(bairroAnterior, out var retorno) ? retorno : 0;
            }

            return Ok(new
            {
                distanciaPercorrida = distanciaTotal,
                valorTotalFrete = valorTotalFrete,
                pedidos = orderedPedidos
            });
        }

        [HttpGet("get-pedidos")]
        public IActionResult ListarTodos()
        {
            var pedidos = _db.Pedidos;
            return Ok(pedidos);
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            var pedido = _db.Pedidos.FirstOrDefault(p => p.Id == id);
            if (pedido == null)
                return NotFound(new { Mensagem = "Pedido não encontrado" });

            _db.Pedidos.Remove(pedido);
            return NoContent();
        }
    }
}
