using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public abstract class Node : Document
    {
        [JsonProperty(PropertyName = "label")]
        public virtual string Label => this.GetType().Name;
        [JsonProperty(PropertyName = "groupId")]
        public virtual string GroupId { get; set; }
        [JsonProperty(PropertyName = "lastChangedCommandDetails")]
        public CommandInformation LastChangedCommandInformation { get; set; }
        [JsonProperty(PropertyName = "createdCommandDetails")]
        public CommandInformation CreatedCommandInformation { get; set; }
    }
}
