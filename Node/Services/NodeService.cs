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
        private ConcurrentDictionary<string, IList<string>> _transactionHashByAddressId;

        private IList<Block> _blockchain;

        public NodeService(NodeInformation nodeInformation)
        {
            this._nodeInformation = nodeInformation;
            this._peersByAddress = new ConcurrentDictionary<string, Peer>();
            this._confirmedTransactionsByHash = new ConcurrentDictionary<string, Transaction>();
            this._pendingTransactionsByHash = new ConcurrentDictionary<string, Transaction>();
            this._addresses = new ConcurrentDictionary<string, Address>();
            this._transactionHashByAddressId = new ConcurrentDictionary<string, IList<string>>();
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
                if (!this._transactionHashByAddressId.ContainsKey(transaction.From.AddressId))
                {
                    this._transactionHashByAddressId.TryAdd(transaction.From.AddressId, new List<string>());
                }
                
                this._transactionHashByAddressId[transaction.From.AddressId].Add(transaction.TransactionHash);
                
                if (!this._transactionHashByAddressId.ContainsKey(transaction.To.AddressId))
                {
                    this._transactionHashByAddressId.TryAdd(transaction.To.AddressId, new List<string>());
                }
                
                this._transactionHashByAddressId[transaction.To.AddressId].Add(transaction.TransactionHash);

                if (!this._addresses.ContainsKey(transaction.From.AddressId))
                {
                    this._addresses.TryAdd(transaction.From.AddressId, transaction.From);
                }
                
                if (!this._addresses.ContainsKey(transaction.To.AddressId))
                {
                    this._addresses.TryAdd(transaction.To.AddressId, transaction.To);
                }
            }
        }


        public Address GetAddress(string id)
        {
            if (!this._addresses.ContainsKey(id))
            {
                return null;
            }

            Address address = this._addresses[id];

            return address;
        }

        public IEnumerable<Address> GetAllAddresses()
        {
            return this._addresses.Values.ToArray();
        }

        public IEnumerable<string> GetTransactionsByAddressId(string addressId)
        {
            return this._transactionHashByAddressId.ContainsKey(addressId) ? this._transactionHashByAddressId[addressId].ToArray() : null;
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
            Transaction t = new Transaction(new Address("0"), new Address("1"), 1);
            t.SenderPublicKey = "hardocoded SenderPublicKey";
            t.SenderSignature = new List<string> {"hardcoded SenderSignatoure", "some"};

            IList<Transaction> transactions = new List<Transaction> {t};

            _genesisBlock = new Block(0, 1, new Address("00"), string.Empty, transactions);

            this.ProcessNewBlock(_genesisBlock);
        }

        private void AddressGenerator()
        {
        }
    }
}