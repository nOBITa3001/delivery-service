using DS.Handlers.Requests;
using FluentValidation;

namespace DS.Handlers.Validators
{
    public class GetPossibleDeliveryRoutesHandlerRequestValidator : AbstractValidator<GetPossibleDeliveryRoutesHandlerRequest>
    {
        public GetPossibleDeliveryRoutesHandlerRequestValidator()
        {
            RuleFor(request => request.Route).NotEmpty();
            RuleFor(request => request.Route).Matches(@"\w+-\w+(-\w)?");
            RuleFor(request => request.MaxRouteRepeat).GreaterThan(0);
        }
    }
}
