using System.Collections.Generic;
using System.Threading.Tasks;
using Node.Models;

namespace Node.Interfaces
{
    public interface INodeService
    {
        NodeInformation GetNodeInfo();
        IEnumerable<Block> GetAllBlocks();
        Block GetBlock(int index);

        Transaction GetTransactionInfo(string hash);
        void AddTransaction(Transaction transaction);
        // TransactionCreatedVM AddTransaction(TransactionVM transaction)
    }
}