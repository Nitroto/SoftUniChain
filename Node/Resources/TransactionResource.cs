using System;
using System.Collections.Generic;

namespace Node.Resources
{
    public class TransactionResource
    {
        public string From { get; set; }
        public string To { get; set; }
        public long Value { get; set; }
        public int Fee { get; set; }
        public DateTime DateCreated { get; set; }
        public string SenderPublicKey { get; set; }
        public string[] SenderSignature { get; set; }
        public string TransactionHash { get; set; }
        public int? MinedInBlockIndex { get; set; }
        public bool TransferSuccessfull { get; set; }

    }
}