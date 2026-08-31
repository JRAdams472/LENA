using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LENA.Application.Features.Inventory
{
    // Queries
    public record GetFoodFlavorByIdQuery(int FoodFlavorId) : IRequest<FoodFlavor?>;
    public record GetFoodFlavorsQuery : IRequest<IReadOnlyList<FoodFlavor>>;
    public record GetFoodFlavorsByFoodIdQuery(int FoodId) : IRequest<IEnumerable<FoodFlavor>>;
    public record GetFoodFlavorsByFlavorIdQuery(int FlavorId) : IRequest<IEnumerable<FoodFlavor>>;
    public record GetFoodFlavorByFoodAndFlavorIdQuery(int FoodId, int FlavorId) : IRequest<FoodFlavor?>;

    // Commands
    public record CreateFoodFlavorCommand(FoodFlavor FoodFlavor) : IRequest<FoodFlavor>;
    public record UpdateFoodFlavorCommand(FoodFlavor FoodFlavor) : IRequest<FoodFlavor>;
    public record DeleteFoodFlavorCommand(int FoodFlavorId) : IRequest<FoodFlavor?>;

    // Handlers
    public class GetFoodFlavorByIdQueryHandler : IRequestHandler<GetFoodFlavorByIdQuery, FoodFlavor?>
    {
        private readonly IFoodFlavorRepository _foodFlavorRepository;
        public GetFoodFlavorByIdQueryHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
        public async Task<FoodFlavor?> Handle(GetFoodFlavorByIdQuery request, CancellationToken cancellationToken)
            => await _foodFlavorRepository.GetByIdAsync(request.FoodFlavorId);
    }

    public class GetFoodFlavorsQueryHandler : IRequestHandler<GetFoodFlavorsQuery, IReadOnlyList<FoodFlavor>>
    {
        private readonly IFoodFlavorRepository _foodFlavorRepository;
        public GetFoodFlavorsQueryHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
        public async Task<IReadOnlyList<FoodFlavor>> Handle(GetFoodFlavorsQuery request, CancellationToken cancellationToken)
            => await _foodFlavorRepository.ListAllAsync();
    }

    public class GetFoodFlavorsByFoodIdQueryHandler : IRequestHandler<GetFoodFlavorsByFoodIdQuery, IEnumerable<FoodFlavor>>
    {
        private readonly IFoodFlavorRepository _foodFlavorRepository;
        public GetFoodFlavorsByFoodIdQueryHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
        public async Task<IEnumerable<FoodFlavor>> Handle(GetFoodFlavorsByFoodIdQuery request, CancellationToken cancellationToken)
            => await _foodFlavorRepository.GetByFoodIdAsync(request.FoodId);
    }

    public class GetFoodFlavorsByFlavorIdQueryHandler : IRequestHandler<GetFoodFlavorsByFlavorIdQuery, IEnumerable<FoodFlavor>>
    {
        private readonly IFoodFlavorRepository _foodFlavorRepository;
        public GetFoodFlavorsByFlavorIdQueryHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
        public async Task<IEnumerable<FoodFlavor>> Handle(GetFoodFlavorsByFlavorIdQuery request, CancellationToken cancellationToken)
            => await _foodFlavorRepository.GetByFlavorIdAsync(request.FlavorId);
    }

    public class GetFoodFlavorByFoodAndFlavorIdQueryHandler : IRequestHandler<GetFoodFlavorByFoodAndFlavorIdQuery, FoodFlavor?>
    {
        private readonly IFoodFlavorRepository _foodFlavorRepository;
        public GetFoodFlavorByFoodAndFlavorIdQueryHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
        public async Task<FoodFlavor?> Handle(GetFoodFlavorByFoodAndFlavorIdQuery request, CancellationToken cancellationToken)
            => await _foodFlavorRepository.GetByFoodAndFlavorIdAsync(request.FoodId, request.FlavorId);
    }

    public class CreateFoodFlavorCommandHandler : IRequestHandler<CreateFoodFlavorCommand, FoodFlavor>
    {
        private readonly IFoodFlavorRepository _foodFlavorRepository;
        public CreateFoodFlavorCommandHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
        public async Task<FoodFlavor> Handle(CreateFoodFlavorCommand request, CancellationToken cancellationToken)
            => await _foodFlavorRepository.CreateAsync(request.FoodFlavor);
    }

    public class UpdateFoodFlavorCommandHandler : IRequestHandler<UpdateFoodFlavorCommand, FoodFlavor>
    {
        private readonly IFoodFlavorRepository _foodFlavorRepository;
        public UpdateFoodFlavorCommandHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
        public async Task<FoodFlavor> Handle(UpdateFoodFlavorCommand request, CancellationToken cancellationToken)
            => await _foodFlavorRepository.UpdateAsync(request.FoodFlavor);
    }

    public class DeleteFoodFlavorCommandHandler : IRequestHandler<DeleteFoodFlavorCommand, FoodFlavor?>
    {
        private readonly IFoodFlavorRepository _foodFlavorRepository;
        public DeleteFoodFlavorCommandHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
        public async Task<FoodFlavor?> Handle(DeleteFoodFlavorCommand request, CancellationToken cancellationToken)
        {
            var foodFlavor = await _foodFlavorRepository.GetByIdAsync(request.FoodFlavorId);
            if (foodFlavor == null)
                return null;

            return await _foodFlavorRepository.DeleteAsync(foodFlavor);
        }
    }
}
