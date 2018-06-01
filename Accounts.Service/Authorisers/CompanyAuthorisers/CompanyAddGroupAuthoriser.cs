using Accounts.Service.Contract.Commands;
using FluentValidation;

namespace Accounts.Service.Authorisers.CompanyAuthorisers
{
    public class CompanyAddGroupAuthoriser : AbstractValidator<AddGroupCommand>, ICompanyAuthoriser<AddGroupCommand>
    {

        public CompanyAddGroupAuthoriser()
        {
            //Group must be a company
            RuleFor(x => x.GroupType == "COMPANY");

            //Creating user be owner
            RuleFor(x => x.Identity.UserId)
                .NotEmpty().WithMessage("Unauthorized Users")
                .OverridePropertyName("Header.userId")
                .Must((instance, excecutingUserId) => instance.OwnerUserId == excecutingUserId).WithMessage("Unauthorized Users")
                .OverridePropertyName("Header.userId");

        }
    }
}
