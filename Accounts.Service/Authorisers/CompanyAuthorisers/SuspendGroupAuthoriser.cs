using Accounts.Service.Contract.Commands;
using FluentValidation;

namespace Accounts.Service.Authorisers.CompanyAuthorisers
{

    public class SuspendGroupAuthoriser : AbstractValidator<SuspendGroupCommand>, ICompanyAuthoriser<SuspendGroupCommand>
    {

        public SuspendGroupAuthoriser()
        {
            RuleFor(x => x.Identity.UserId).Must((instance, excecutingUserId) => instance.OwnerId == excecutingUserId);


        }
    }
}
