using DS.Handlers.Requests;
using FluentValidation;

namespace DS.Handlers.Validators
{
    public class GetDeliveryCostHandlerRequestValidator : AbstractValidator<GetDeliveryCostHandlerRequest>
    {
        public GetDeliveryCostHandlerRequestValidator()
        {
            RuleFor(request => request.Route).NotEmpty();
        }
    }
}
