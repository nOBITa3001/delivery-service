using DS.Handlers.Requests;
using FluentValidation;

namespace DS.Handlers.Validators
{
    public class GetTheCheapestDeliveryCostHandlerRequestValidator : AbstractValidator<GetTheCheapestDeliveryCostHandlerRequest>
    {
        public GetTheCheapestDeliveryCostHandlerRequestValidator()
        {
            RuleFor(request => request.Route).NotEmpty();
            RuleFor(request => request.Route).Matches(@"^(\w-\w)$");
        }
    }
}
