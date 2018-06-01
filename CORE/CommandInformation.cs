using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class CommandInformation
    {
        public CommandInformation(string commandName, string commandId, BizflyIdentity identity, DateTime dateTime)
        {
            CommandName = commandName;
            CommandId = commandId;
            Identity = identity;
            DateTime = dateTime;
        }

        public string CommandName { get; }
        public string CommandId { get; }
        BizflyIdentity Identity { get; }
        public DateTime DateTime { get; }
    }
}
