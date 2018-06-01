using Accounts.Service.Contract.Commands;
using FluentValidation;
using System;

namespace Accounts.Service.Validators.Default
{

    public class SuspendGroupCommandValidator : DefaultValidator<SuspendGroupCommand>
    {
        public SuspendGroupCommandValidator()
        {
            RuleFor(x => x.CompanyId).NotEmpty().WithMessage("Id is empty")
                .Must(id => Guid.TryParse(id, out Guid result))
                .WithMessage("Id must be in GUID format");

        }
    }
}
