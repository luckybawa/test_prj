using System;
using System.Collections.Generic;
using System.Text;

namespace CORE2
{
    public interface ICommand
    {
        string CommandId { get; }
        BizflyIdentity Identity { get; }
        DateTime DateTime { get; }
    }
}
