using FluentValidation;

namespace Accounts.Service.Validators
{
    public interface IRatifier<T> : IValidator<T> 
    {
    }
}