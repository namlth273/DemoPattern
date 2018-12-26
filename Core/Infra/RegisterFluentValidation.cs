using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;

namespace Core.Infra
{
    public class RegisterFluentValidation
    {
        private readonly IList<Type> _typesToScan;

        public RegisterFluentValidation(IList<Type> typesToScan)
        {
            _typesToScan = typesToScan;
        }

        public void ConfigurationExpression(FluentValidationMvcConfiguration obj)
        {
            foreach (var type in _typesToScan)
            {
                obj.RegisterValidatorsFromAssemblyContaining(type);
            }

            obj.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
        }
    }
}
