using System.Collections.Generic;

namespace Node.Resources
{
    public class TransactionResource
    {
        public string From { get; set; }
        public string To { get; set; }
        public long Amount { get; set; }
        public string SenderPublicKey { get; set; }
        public IList<string> SenderSignature { get; set; }
        public long Nonce { get; set; }
        public int MinedInBlockIndex { get; set; }
        public bool Paid { get; set; }
        public string TransactionHash { get; set; }
    }
}