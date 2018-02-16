namespace Node.Models
{
    public class Address
    {
        private static Address _generatorAddress;
        
        public string AddressId { get;}
        public long Amount { get; set; }
        
        //TODO: possible concurency issues?

        public Address(string address)
        {
            // TODO: Validate address?
            this.AddressId = address;
        }

        public static Address GeneratorAddress
        {
            get
            {
                if (_generatorAddress == null)
                {
                    _generatorAddress = new Address("0");
                }

                return _generatorAddress;
            }

        }

        public override string ToString()
        {
            return this.AddressId;
        }
    }
}