using MediatR;

namespace Hamburgueria.Application.Queries.CalcularFrete
{
    public record CalcularFreteQuery(string Bairro) : IRequest<double>;
}
