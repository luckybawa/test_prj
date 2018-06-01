using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public abstract class DTO
    {
        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; set; }
    }
}
