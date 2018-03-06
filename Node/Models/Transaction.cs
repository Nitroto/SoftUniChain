using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using System.Security.Cryptography;
using Node.Utilities;

namespace Node.Models
{
    public class Transaction
    {
        private const int FEE = 20;
        private string _transactionData;

        public Transaction(Address from, Address to, ulong value, uint fee, string senderPublicKey, string[] senderSignature)
        {
            this.From = from;
            this.To = to;
            this.Value = value;
            this.Fee = fee;
            this.DateCreated = DateTime.Now;
            this.SenderPublicKey = senderPublicKey;
            this.SenderSignature = senderSignature;
            this.TransactionHash = CalculateHash();
        }

        public Address From { get; set; }
        public Address To { get; set; }
        public ulong Value { get; set; }
        public uint Fee { get; set; }
        public DateTime DateCreated { get; set; }
        public string SenderPublicKey { get; set; }
        public string[] SenderSignature { get; set; }
        public string TransactionHash { get; set; }
        public int? MinedInBlockIndex { get; set; }
        public bool TransferSuccessfull { get; set; }

        private string CalculateHash()
        {
            var payload = new
            {
                from = this.From.AddressId,
                to = this.To.AddressId,
                dateCreated = this.DateCreated,
                value = this.Value,
                fee = this.Fee,
            };
            
            return Crypto.BytesToHex(Crypto.CalcSha256(Crypto.JsonToString(payload)));
        }
    }
}