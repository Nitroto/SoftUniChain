using System.Collections.Generic;

namespace Node.Resources
{
    public class BlockResource
    {
        public int Index { get; set; }
        public string CreatedOn { get; set; }
        public long Difficulty { get; set; }
        public string MinedBy { get; set; }
        public string PreviousBlockHash { get; set; }
        public string BlockHash { get; set; }
        public string BlockDataHash { get; set; }
        public long Nonce { get; set; }
        public IList<TransactionResource> Transactions { get; set; }

        public override string ToString()
        {
            return (
                $"index: {Index},\n" +
                $"created on: {CreatedOn},\n" +
                $"difficulty: {Difficulty},\n" +
                $"mined by: {MinedBy},\n" +
                $"previous block hash: {PreviousBlockHash},\n" +
                $"block hash: {BlockHash},\n" +
                $"block data hash: {BlockDataHash},\n" +
                $"nonce: {Nonce}\n" +
                string.Join(',', Transactions)
            );
        }
    }
}