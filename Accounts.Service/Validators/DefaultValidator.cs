using FluentValidation;

namespace Accounts.Service.Validators
{
    public abstract class DefaultValidator<T> : AbstractValidator<T>, IRatifier<T>
    {

    }
}
