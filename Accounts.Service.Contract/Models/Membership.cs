using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using CORE2;

namespace Models.Accounts
{
    public class Membership : Node
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }
        [JsonProperty(PropertyName = "permissions")]
        public string[] Permissions { get; set; }
    }
}
