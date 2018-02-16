using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using Node.Utilities;

namespace Node.Models
{
    public class Transaction
    {
        private string _transactionData;

        public Transaction(Address from, Address to, long amount)
        {
            this.From = from;
            this.To = to;
            this.Amount = amount;

            this.TransactionHash = Crypto.Sha256(TransactionData);
        }

        public Address From { get; private set; }
        public Address To { get; private set; }
        public long Amount { get; set; }
        public string SenderPublicKey { get; set; }
        public IList<string> SenderSignature { get; set; }
        public long Nonce { get; set; }
        public int MinedInBlockIndex { get; set; }
        public bool Paid { get; set; }
        public string TransactionHash { get; private set; }

        private string TransactionData => this._transactionData ?? (this._transactionData =
                                              $"{{'from': '{From.AddressId}','nonce':{Nonce},'value':'{Amount}','to':'{To.AddressId}'}}"
                                          );
    }
}