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
    public record GetFoodNutrientByIdQuery(int FoodNutrientId) : IRequest<FoodNutrient?>;
    public record GetFoodNutrientsQuery : IRequest<IReadOnlyList<FoodNutrient>>;
    public record GetFoodNutrientsByFoodIdQuery(int FoodId) : IRequest<IEnumerable<FoodNutrient>>;
    public record GetFoodNutrientsByNutrientIdQuery(int NutrientId) : IRequest<IEnumerable<FoodNutrient>>;
    public record GetFoodNutrientByFoodAndNutrientIdQuery(int FoodId, int NutrientId) : IRequest<FoodNutrient?>;

    // Commands
    public record CreateFoodNutrientCommand(FoodNutrient FoodNutrient) : IRequest<FoodNutrient>;
    public record UpdateFoodNutrientCommand(FoodNutrient FoodNutrient) : IRequest<FoodNutrient>;
    public record DeleteFoodNutrientCommand(int FoodNutrientId) : IRequest<FoodNutrient?>;

    // Handlers
    public class GetFoodNutrientByIdQueryHandler : IRequestHandler<GetFoodNutrientByIdQuery, FoodNutrient?>
    {
        private readonly IFoodNutrientRepository _foodNutrientRepository;
        public GetFoodNutrientByIdQueryHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
        public async Task<FoodNutrient?> Handle(GetFoodNutrientByIdQuery request, CancellationToken cancellationToken)
            => await _foodNutrientRepository.GetByIdAsync(request.FoodNutrientId);
    }

    public class GetFoodNutrientsQueryHandler : IRequestHandler<GetFoodNutrientsQuery, IReadOnlyList<FoodNutrient>>
    {
        private readonly IFoodNutrientRepository _foodNutrientRepository;
        public GetFoodNutrientsQueryHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
        public async Task<IReadOnlyList<FoodNutrient>> Handle(GetFoodNutrientsQuery request, CancellationToken cancellationToken)
            => await _foodNutrientRepository.ListAllAsync();
    }

    public class GetFoodNutrientsByFoodIdQueryHandler : IRequestHandler<GetFoodNutrientsByFoodIdQuery, IEnumerable<FoodNutrient>>
    {
        private readonly IFoodNutrientRepository _foodNutrientRepository;
        public GetFoodNutrientsByFoodIdQueryHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
        public async Task<IEnumerable<FoodNutrient>> Handle(GetFoodNutrientsByFoodIdQuery request, CancellationToken cancellationToken)
            => await _foodNutrientRepository.GetByFoodIdAsync(request.FoodId);
    }

    public class GetFoodNutrientsByNutrientIdQueryHandler : IRequestHandler<GetFoodNutrientsByNutrientIdQuery, IEnumerable<FoodNutrient>>
    {
        private readonly IFoodNutrientRepository _foodNutrientRepository;
        public GetFoodNutrientsByNutrientIdQueryHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
        public async Task<IEnumerable<FoodNutrient>> Handle(GetFoodNutrientsByNutrientIdQuery request, CancellationToken cancellationToken)
            => await _foodNutrientRepository.GetByNutrientIdAsync(request.NutrientId);
    }

    public class GetFoodNutrientByFoodAndNutrientIdQueryHandler : IRequestHandler<GetFoodNutrientByFoodAndNutrientIdQuery, FoodNutrient?>
    {
        private readonly IFoodNutrientRepository _foodNutrientRepository;
        public GetFoodNutrientByFoodAndNutrientIdQueryHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
        public async Task<FoodNutrient?> Handle(GetFoodNutrientByFoodAndNutrientIdQuery request, CancellationToken cancellationToken)
            => await _foodNutrientRepository.GetByFoodAndNutrientIdAsync(request.FoodId, request.NutrientId);
    }

    public class CreateFoodNutrientCommandHandler : IRequestHandler<CreateFoodNutrientCommand, FoodNutrient>
    {
        private readonly IFoodNutrientRepository _foodNutrientRepository;
        public CreateFoodNutrientCommandHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
        public async Task<FoodNutrient> Handle(CreateFoodNutrientCommand request, CancellationToken cancellationToken)
            => await _foodNutrientRepository.CreateAsync(request.FoodNutrient);
    }

    public class UpdateFoodNutrientCommandHandler : IRequestHandler<UpdateFoodNutrientCommand, FoodNutrient>
    {
        private readonly IFoodNutrientRepository _foodNutrientRepository;
        public UpdateFoodNutrientCommandHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
        public async Task<FoodNutrient> Handle(UpdateFoodNutrientCommand request, CancellationToken cancellationToken)
            => await _foodNutrientRepository.UpdateAsync(request.FoodNutrient);
    }

    public class DeleteFoodNutrientCommandHandler : IRequestHandler<DeleteFoodNutrientCommand, FoodNutrient?>
    {
        private readonly IFoodNutrientRepository _foodNutrientRepository;
        public DeleteFoodNutrientCommandHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
        public async Task<FoodNutrient?> Handle(DeleteFoodNutrientCommand request, CancellationToken cancellationToken)
        {
            var foodNutrient = await _foodNutrientRepository.GetByIdAsync(request.FoodNutrientId);
            if (foodNutrient == null)
                return null;

            return await _foodNutrientRepository.DeleteAsync(foodNutrient);
        }
    }
}
