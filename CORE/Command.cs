using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public abstract class Command : ICommand
    {
        public Command(BizflyIdentity identity)
        {
            this.CommandId = Guid.NewGuid().ToString();
            this.Identity = identity;
            this.DateTime = DateTime.UtcNow;
        }

        public string CommandId { get; }
        public BizflyIdentity Identity { get; }
        public DateTime DateTime { get; }

        

        public CommandInformation GetCommandInformation()
        {
            return new CommandInformation(this.GetType().Name, this.CommandId, this.Identity, this.DateTime);
        }
    }

}
