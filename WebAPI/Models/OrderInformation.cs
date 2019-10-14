using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebAPI
{
    public class OrderInformation
    {

        #region Properties

        [JsonProperty("orderitems")]
        public IDictionary<string, short> OrderItems { get; private set; }

        [JsonProperty("requiredbinwidth")]
        public float RequiredBinWidth { get; private set; }

        #endregion /Properties

        #region Constructors

        public OrderInformation()
        {

        }

        public OrderInformation(IDictionary<string, short> orderItems,
            float requiredBinWidth)
        {
            this.OrderItems = orderItems;
            this.RequiredBinWidth = requiredBinWidth;
        }

        #endregion /Constructors

    }
}
