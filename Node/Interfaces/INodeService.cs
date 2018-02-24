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
        IEnumerable<string> GetTransactionsByAddressId(string addressId);
        // TransactionCreatedVM AddTransaction(TransactionVM transaction)
        
        Address GetAddress(string id);
        IEnumerable<Address> GetAllAddresses();
        bool CheckForCollison(string transactionHash);
        bool CheckSenderBalance(string senderId, ulong amount);
    }
}