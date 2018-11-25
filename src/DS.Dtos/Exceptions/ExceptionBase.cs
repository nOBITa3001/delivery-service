using System;
using System.Collections.Generic;

namespace DS.Dtos.Exceptions
{
    public abstract class ExceptionBase : Exception
    {
        public IEnumerable<string> ErrorMessages { get; }

        protected ExceptionBase(params string[] errorMessages)
        {
            ErrorMessages = errorMessages;
        }
    }
}
