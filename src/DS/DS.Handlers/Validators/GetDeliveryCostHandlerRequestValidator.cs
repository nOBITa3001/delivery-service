using DS.Handlers.Requests;
using FluentValidation;

namespace HBFA.Handlers.Validators
{
    public class GetDeliveryCostHandlerRequestValidator : AbstractValidator<GetDeliveryCostHandlerRequest>
    {
        public GetDeliveryCostHandlerRequestValidator()
        {
            RuleFor(request => request.Route).NotEmpty();
        }
    }
}
