using System.Collections.Generic;

namespace Node.Resources
{
    public class AddressResource
    {
        public string AddressId { get; set; }
        public long Amount { get; set; }
        public IEnumerable<string> Transactions { get; set; }
    }
}