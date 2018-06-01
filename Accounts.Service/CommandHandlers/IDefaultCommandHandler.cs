using System;
using System.Collections.Generic;
using System.Text;
using CORE2;

namespace Accounts.Service.CommandHandlers
{
    public interface IDefaultCommandHandler<T> : ICommandHandler<T> where T:Command
    {
    }
}
