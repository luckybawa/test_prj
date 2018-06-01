using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    interface IResourceBasedHandler<TCommand, in TValidated> where TCommand: Command where TValidated:class
    {
        Task Handle(TCommand command, TValidated validated);
    }
}
