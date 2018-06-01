using CORE2;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace Accounts.Service.Validators
{
    public class ValidationDecorator<T> : ICommandHandler<T> where T:Command
    {
        private readonly ICommandHandler<T> _handler;
        private readonly IRatifier<T> _validators;

        public ValidationDecorator(ICommandHandler<T> handler, IRatifier<T> validator)
        {
            _validators = validator;
            _handler = handler;
        }

        public async Task Handle(T command)
        {
            if(_validators == null)
            {
                throw new NotImplementedException("No validator specified");
            }           
            _validators.ValidateAndThrow(command);
            await  _handler.Handle(command);
        }
    }

}
