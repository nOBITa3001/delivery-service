using DS.Dtos.Routes;
using System;

namespace DS.Dtos.Builders
{
    public class CreateRouteDtoBuilder : CreateRouteDto
    {
        public CreateRouteDtoBuilder WithAllValidFields()
        {
            Start = StringGenerator.Random(1);
            End = StringGenerator.Random(1);

            return this;
        }

        public CreateRouteDtoBuilder With(Action<CreateRouteDtoBuilder> action)
        {
            action(this);

            return this;
        }
    }
}
