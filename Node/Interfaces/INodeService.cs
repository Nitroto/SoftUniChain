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
        Block GetBlockCandidate();
        bool IsBlockValid(Block block);
        void UpdateBlockchain(Block confirmedBlock);

        Transaction GetTransactionInfo(string hash);
        IEnumerable<Transaction> GetTransactions(bool confirmed = true, bool pending = true);
        void AddTransaction(Transaction transaction);
        IEnumerable<string> GetTransactionsByAddressId(string addressId);

        Address GetAddress(string id);
        IEnumerable<Address> GetAllAddresses();
        bool CheckForCollison(string transactionHash);
        bool CheckSenderBalance(string senderId, ulong amount);
        void AddAddress(Address address);
    }
}