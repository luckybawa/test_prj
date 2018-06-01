using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface ICommand
    {
        string CommandId { get; }
        BizflyIdentity Identity { get; }
        DateTime DateTime { get; }
    }
}
