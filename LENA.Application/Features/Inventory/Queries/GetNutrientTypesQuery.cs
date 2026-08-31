using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace LENA.Application.Features.Inventory.Queries
{
    public record GetNutrientTypesQuery : IRequest<IReadOnlyList<NutrientType>>;

        public class GetNutrientTypesQueryHandler : IRequestHandler<GetNutrientTypesQuery, IReadOnlyList<NutrientType>>
        {
            private readonly INutrientTypeRepository _nutrientTypeRepository;
            public GetNutrientTypesQueryHandler(INutrientTypeRepository nutrientTypeRepository) => _nutrientTypeRepository = nutrientTypeRepository;
            public async Task<IReadOnlyList<NutrientType>> Handle(GetNutrientTypesQuery request, CancellationToken cancellationToken)
                => await _nutrientTypeRepository.ListAllAsync();
        }
}