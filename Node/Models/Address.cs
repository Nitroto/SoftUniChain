using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Node.Utilities;

namespace Node.Models
{
    public class Address
    {
        private string _addressId;

        public Address(string addressId = null)
        {
            this.AddressId = addressId;
            this.Transactions = new List<Transaction>();
        }

        public string AddressId
        {
            get => this._addressId;
            private set
            {
                if (!Crypto.ValidateAddress(value))
                {
                    throw new ArgumentException("Incorect address");
                }

                this._addressId = value;
            }
        }

        public long Amount { get; set; }

        public IList<Transaction> Transactions { get; set; }

        public override string ToString()
        {
            return this.AddressId;
        }
    }
}