using System.Collections.Generic;

namespace Node.Models
{
    public class Address
    {
        public Address(string addressId)
        {
            // TODO: Validate address?
            this.AddressId = addressId;
        }
        
        public string AddressId { get;}
        public long Amount { get; set; }
        
        //TODO: possible concurency issues?

        public override string ToString()
        {
            return this.AddressId;
        }
    }
}