using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CORE2
{
    interface IResourceBasedHandler<TCommand, in TValidated> where TCommand: Command where TValidated:class
    {
        Task Handle(TCommand command, TValidated validated);
    }
}
