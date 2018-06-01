using CORE2;
using Models.Accounts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Accounts.Service.Contract.DTOs
{
    public class ListGroupSmallDetailDTO : DTO
    {
 
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        [JsonProperty(PropertyName = "data")]
        public IEnumerable<GroupSmallDetailDTO> Data { get; set; }

        public ListGroupSmallDetailDTO(IEnumerable<Group> listOfGroup)
        {
            Data = listOfGroup.Select(group => new GroupSmallDetailDTO(group));
            Count = Data.Count();
        }
        public ListGroupSmallDetailDTO()
        {
            Data = new List<GroupSmallDetailDTO>();
            Count = 0;
        }
    }
}
