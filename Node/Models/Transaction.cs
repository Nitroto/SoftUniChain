using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using Node.Utilities;

namespace Node.Models
{
    public class Transaction
    {
        private string _transactionHash;
        private string _transactionData;

        public Address From { get; set; }
        public Address To { get; set; }
        public long Amount { get; set; }
        public string SenderPublicKey { get; set; }
        public IList<string> SenderSignature { get; set; }
        public long Nonce { get; set; }
        public int MinedInBlockIndex { get; set; }
        public bool Paid { get; set; }

        public string TransactionData
        {
            get
            {
                if (this._transactionData == null)
                {
                    this._transactionData = $"{{'from': '{From.AddressId}','nonce':{Nonce},'value':'{Amount}','to':'{To.AddressId}'}}";
                }

                return this._transactionData;
            }
        }
        
        public string TransactionHash
        {
            get
            {
                if (this._transactionHash == null)
                {
                    this._transactionHash = Crypto.Sha256(TransactionData);
                }

                return this._transactionHash;
            }
        }
    }
}