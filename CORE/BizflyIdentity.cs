using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class BizflyIdentity
    {
        public string UserId { get; set; }
        public string GroupId { get; set; }
        public string GroupType { get; set; }
        public string[] Permissions { get; set; }

    }
}
