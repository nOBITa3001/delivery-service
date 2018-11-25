using DS.Dtos.Exceptions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DS.DomainModel
{
    public abstract class EntityBase
    {
        internal void Validate()
        {
            var validationResults = GetValidateResults().Where(result => result != ValidationResult.Success);
            if (validationResults.Any())
                throw new DomainModelException(validationResults.Select(validation => validation.ErrorMessage).ToArray());
        }

        private IEnumerable<ValidationResult> GetValidateResults()
        {
            var validationContext = new ValidationContext(this);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, validationContext, validationResults, true);

            return validationResults;
        }
    }
}
