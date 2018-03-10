using System.Collections.Generic;

namespace Node.Resources
{
    public class AddressResource
    {
        public AddressResource(string addr = null)
        {
            this.AddressId = addr;
        }

        public string AddressId { get; set; }
        public long Amount { get; set; }
        public IEnumerable<TransactionResource> Transactions { get; set; }
    }
}