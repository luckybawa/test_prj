using Accounts.Service.Contract.Commands;
using CORE2;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accounts.Service.Validators.Default
{
    public class DefaultAddGroupCommandValidator : DefaultValidator<AddGroupCommand>
    {
        public DefaultAddGroupCommandValidator()
        {
            RuleFor(x => x.GroupName)
                .NotEmpty().WithMessage("GroupName is empty");

            RuleFor(x => x.NewGroupId)
                .NotEmpty().WithMessage("NewGroupId is empty")
                .Must(id=>Guid.TryParse(id, out Guid result)).WithMessage("Id must be GUID");

            RuleFor(x => x.OwnerUserId)
                .NotEmpty().WithMessage("UserId is empty")
               .Must(id => Guid.TryParse(id, out Guid result)).WithMessage("UserId must be GUID");
        }
    }
}
