using System.Collections.Generic;

namespace Node.Resources
{
    public class BlockResource
    {
        public int Index { get; set; }
        public string CreatedOn { get; set; }
        public long Difficulty { get; set; }
        public string MineBy { get; set; }
        public string PreviousBlockHash { get; set; }
        public string BlockHash { get; set; }
        public string BlockDataHash { get; set; }
        public long Nonce { get; set; }
        public IList<TransactionResource> Transactions { get; set; }
    }
}