using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface ICommandHandler<in TCommand> where TCommand:Command
    {
        Task Handle(TCommand command);
    }

    public interface ICompanyCommandHandler<T> : ICommandHandler<T> where T:Command
    {}

    public interface IProviderCommandHandler<T> : ICommandHandler<T> where T : Command
    { }

    public interface IManagementCommandHandler<T> : ICommandHandler<T> where T : Command
    { }
}
