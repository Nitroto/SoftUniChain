using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Node.Interfaces;
using Node.Models;
using Node.Resources;

namespace Node.Services
{
    public class NodeService: INodeService
    {
        private NodeInformation _nodeInformation;
        private ConcurrentDictionary<string, Peer> _peersByAddress;
        private ConcurrentDictionary<string, Transaction> _confirmedTransactionsByHash;
        private ConcurrentDictionary<string, Transaction> _pendingTransactionsByHash;
        private ConcurrentDictionary<string, Address> _addresses;

        private IList<Block> _blockchain;

        public NodeService(NodeInformation nodeInformation)
        {
            this._nodeInformation = nodeInformation;
            this._peersByAddress = new ConcurrentDictionary<string, Peer>();
            this._confirmedTransactionsByHash = new ConcurrentDictionary<string, Transaction>();
            this._pendingTransactionsByHash = new ConcurrentDictionary<string, Transaction>();
            this._addresses = new ConcurrentDictionary<string, Address>();
            // TODO: decide on the collection type, if list introduce locking
            this._blockchain = new List<Block>();

            ProcessNewBlock(Block.Genesis);
        }

        public NodeInformation GetNodeInfo()
        {
            var info = new NodeInformation()
            {
                Name = this._nodeInformation.Name,
                About = this._nodeInformation.About,
                Peers = this._peersByAddress.Count,
                Blocks = this._blockchain.Count,
                ConfirmedTransactions = this._confirmedTransactionsByHash.Count,
                PendingTransactions = this._pendingTransactionsByHash.Count,
                Addresses = this._addresses.Count,
                Coins = this._addresses.Sum(a => a.Value.Amount)
            };

            return info;
        }

        public Block GetBlock(int index)
        {
            if (index < 0 || index > this._blockchain.Count)
            {
                throw new Exception($"Block not found[index='{index}']");
            }

            var block = this._blockchain[index];

            return block;
        }
        

        public IEnumerable<Block> GetAllBlocks()
        {
            
            var blocks = this._blockchain;

            return blocks.Reverse();
        }

        public Transaction GetTransactionInfo(string hash)
        {
            throw new System.NotImplementedException();
        }

        public void AddTransaction(Transaction transaction)
        {
            throw new System.NotImplementedException();
        }
        
        private void ProcessNewBlock(Block block)
        {
            if (IsBlockValid(block))
            {
                UpdateCollections(block);
                this._blockchain.Add(block);
            }
        }

        private void UpdateCollections(Block block)
        {
            foreach (Transaction t in block.Transactions)
            {
                t.From = t.From;
                t.To = t.To;

                t.From.Amount -= t.Amount;
                t.To.Amount += t.Amount;

                t.MinedInBlockIndex = block.Index;
                
                // TODO: What exactly Paid means
                t.Paid = true;

                this._confirmedTransactionsByHash.TryAdd(t.TransactionHash, t);
                this._pendingTransactionsByHash.TryRemove(t.TransactionHash, out var Ignore);
            }
        }

//        private Address GetOrAddAddress(string address)
//        {
//            return GetOrAddAddress(new Address(address));
//        }

        private bool IsBlockValid(Block block)
        {
            // TODO: Validate block index, previous blockhash, BlockHash(based on Nonce, date, BlockDataHash)
            // TODO: Validate all transactions - if the amounts are available, signature, TransactionHash
            // Is it possible to have single address 2 times in a block? If yes - will need some kind of temp addresses collection to keep track of the transactions in the block
            // TODO: Validate BlockDataHash

            return true;
        }
    }
}