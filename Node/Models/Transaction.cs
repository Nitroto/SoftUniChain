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

        public Address From { get; set; }
        public Address To { get; set; }        
        public long Value { get; set; }
        public int Fee { get; set; }
        public DateTime DateCreated { get; set; }
        public string SenderPublicKey { get; set; }
        public IEnumerable<string> SenderSignature { get; set; }
        public string TransactionHash { get; set; }
        public int? MinedInBlockIndex { get; set; }
        public bool TransferSuccessfull { get; set; }
    }
}