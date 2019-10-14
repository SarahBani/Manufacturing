using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI
{
    public class CustomerOrder
    {

        #region Properties

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("orderid")]
        public long OrderID { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("orderitems")]
        public IDictionary<string, short> OrderItems { get; set; }

        #endregion /Properties

    }

}
