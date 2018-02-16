using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Node.Interfaces;
using Node.Models;

namespace Node.Services
{
    public class NodeService : INodeService
    {
        private static Block _genesisBlock;
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

            this.ProcessGenesisBlock();
        }

        public NodeInformation GetNodeInfo()
        {
            NodeInformation info = new NodeInformation()
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
            if (index < 0 || index >= this._blockchain.Count)
            {
                return null;
            }

            Block block = this._blockchain[index];

            return block;
        }


        public IEnumerable<Block> GetAllBlocks()
        {
            var blocks = this._blockchain;

            return blocks.Reverse();
        }

        public Transaction GetTransactionInfo(string hash)
        {
            if (this._pendingTransactionsByHash.ContainsKey(hash))
            {
                return this._pendingTransactionsByHash[hash];
            }

            return this._confirmedTransactionsByHash.ContainsKey(hash) ? this._confirmedTransactionsByHash[hash] : null;
        }

        public void AddTransaction(Transaction transaction)
        {
            string transactionHash = transaction.TransactionHash;
            if (!this._pendingTransactionsByHash.ContainsKey(transactionHash) &&
                !this._confirmedTransactionsByHash.ContainsKey(transactionHash))
            {
                this._pendingTransactionsByHash.TryAdd(transactionHash, transaction);
            }
        }

        private void ProcessNewBlock(Block block)
        {
            if (!IsBlockValid(block)) return;
            this.UpdateCollections(block);
            this._blockchain.Add(block);
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

        private void ProcessGenesisBlock()
        {
            if (_genesisBlock != null) return;
            IList<Transaction> transactions = new List<Transaction>()
            {
                new Transaction
                {
                    From = Address.GeneratorAddress,
                    Amount = 100,
                    To = new Address("1"),
                    SenderPublicKey = "hardocoded SenderPublicKey",
                    SenderSignature = new List<string> {"hardcoded SenderSignatoure", "somee"}
                }
            };
            _genesisBlock = new Block(0, 1, new Address("00"), string.Empty, transactions);

            this.ProcessNewBlock(_genesisBlock);
        }
    }
}