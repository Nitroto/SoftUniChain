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
        private ConcurrentDictionary<string, Block> _miningJobs;

        private IList<Block> _blockchain;

        public NodeService(NodeInformation nodeInformation)
        {
            this._nodeInformation = nodeInformation;
            this._peersByAddress = new ConcurrentDictionary<string, Peer>();
            this._confirmedTransactionsByHash = new ConcurrentDictionary<string, Transaction>();
            this._pendingTransactionsByHash = new ConcurrentDictionary<string, Transaction>();
            this._addresses = new ConcurrentDictionary<string, Address>();
            this._transactionHashByAddressId = new ConcurrentDictionary<string, IList<string>>();
            this._miningJobs = new ConcurrentDictionary<string, Block>();
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

        public bool CheckForCollison(string transactionHash)
        {
            return (this._pendingTransactionsByHash.ContainsKey(transactionHash) ||
                    this._confirmedTransactionsByHash.ContainsKey(transactionHash));
        }

        public bool CheckSenderBalance(string senderId, ulong amount)
        {
            if (!this._addresses.ContainsKey(senderId))
            {
                return false;
            }

            return (ulong) (this._addresses[senderId].Amount * 1000000) >= amount;
        }

        public void AddTransaction(Transaction transaction)
        {
            string hash = transaction.TransactionHash;

            this._pendingTransactionsByHash.TryAdd(hash, transaction);

            this.AddTransactionToAddress(transaction.From, hash);
            this.AddTransactionToAddress(transaction.To, hash);

            this.AddAddress(transaction.From);
            this.AddAddress(transaction.To);
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
            return this._transactionHashByAddressId.ContainsKey(addressId)
                ? this._transactionHashByAddressId[addressId].ToArray()
                : null;
        }

        private void AddTransactionToAddress(Address address, string transactionHash)
        {
            if (!this._transactionHashByAddressId.ContainsKey(address.AddressId))
            {
                this._transactionHashByAddressId.TryAdd(address.AddressId, new List<string>());
            }

            this._transactionHashByAddressId[address.AddressId].Add(transactionHash);
        }

        private void AddAddress(Address address)
        {
            if (!this._addresses.ContainsKey(address.AddressId))
            {
                this._addresses.TryAdd(address.AddressId, address);
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
//            foreach (Transaction t in block.Transactions)
//            {
//                t.From.Amount -= t.Value;
//                t.To.Amount += t.Value;
//
//                t.MinedInBlockIndex = block.Index;
//
//                // TODO: What exactly Paid means
//                t.TransferSuccessfull = true;
//
//                this._confirmedTransactionsByHash.TryAdd(t.TransactionHash, t);
//                this._pendingTransactionsByHash.TryRemove(t.TransactionHash, out var Ignore);
//            }
        }

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
            Transaction faucet = new Transaction()
            {
                From = new Address("0000000000000000000000000000000000000000"),
                To = new Address("bee3f694bf0fbf9556273e85d43f2e521d24835e"),
                DateCreated = DateTime.Now,
                Value = 2000000000,
                Fee = 0,
                MinedInBlockIndex = 0,
                TransferSuccessfull = true
            };

            this.AddAddress(faucet.To);
            this._addresses[faucet.To.AddressId].Amount += (long) faucet.Value;

            List<Transaction> transactions = new List<Transaction>() {faucet};
            _genesisBlock = new Block(0, 5, null, new Address("0000000000000000000000000000000000000000"), string.Empty,
                transactions);

            this.ProcessNewBlock(_genesisBlock);
        }

        private void AddressGenerator()
        {
        }
    }
}