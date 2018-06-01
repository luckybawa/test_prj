using Accounts.Service.Contract.Commands;
using FluentValidation;
using System;

namespace Accounts.Service.Validators.Default
{

    public class DefaultDeleteGroupCommandValidator : DefaultValidator<DeleteGroupCommand>
    {
        public DefaultDeleteGroupCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is empty")
                .Must(id => Guid.TryParse(id, out Guid result))
                .WithMessage("Id must be in GUID format");
        }
    }
}
