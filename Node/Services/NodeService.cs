using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Node.Interfaces;
using Node.Models;
using Node.Utilities;

namespace Node.Services
{
    public class NodeService : INodeService
    {
        private Address FirstAddress = new Address("0000000000000000000000000000000000000000");
        private const int Order = 1000000;
        private const int BlockReward = 5000000;
        private static Block _genesisBlock;
        private static Block _candidate;
        private NodeInformation _nodeInformation;
        private ConcurrentDictionary<string, Peer> _peersByAddress;
        private ConcurrentDictionary<string, Transaction> _confirmedTransactionsByHash;
        private ConcurrentDictionary<string, Transaction> _pendingTransactionsByHash;
        private ConcurrentDictionary<string, Address> _addresses;
        private ConcurrentDictionary<string, Block> _miningJobs;
        private ConcurrentBag<Block> _blockchain;

        public NodeService(NodeInformation nodeInformation)
        {
            this._nodeInformation = nodeInformation;
            this._peersByAddress = new ConcurrentDictionary<string, Peer>();
            this._confirmedTransactionsByHash = new ConcurrentDictionary<string, Transaction>();
            this._pendingTransactionsByHash = new ConcurrentDictionary<string, Transaction>();
            this._addresses = new ConcurrentDictionary<string, Address>();
            this._miningJobs = new ConcurrentDictionary<string, Block>();
            this._blockchain = new ConcurrentBag<Block>();

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

            var block = this._blockchain.FirstOrDefault(b => b.Index == index);

            return block;
        }

        public IEnumerable<Block> GetAllBlocks()
        {
            var blocks = this._blockchain;

            return blocks.Reverse();
        }

        public Block GetBlockCandidate()
        {
            if (this._pendingTransactionsByHash.Count > 0)
            {
                IEnumerable<Transaction> transactions = this.ProcessTransactions(GetTransactions(false));
                _candidate.Transactions = _candidate.Transactions.Concat(transactions);
            }

            return _candidate;
        }

        public IEnumerable<Transaction> GetTransactions(bool confirmed = true, bool pending = true)
        {
            IList<Transaction> confirmedTransactions = confirmed
                ? confirmedTransactions = this._confirmedTransactionsByHash.Values.ToList()
                : new List<Transaction>();

            IList<Transaction>
                pendingTransactions =
                    pending ? this._pendingTransactionsByHash.Values.ToList() : new List<Transaction>();

            IEnumerable<Transaction> transactions = confirmedTransactions.Concat(pendingTransactions).ToArray();
            return transactions;
        }

        public void PayForBlock(string address)
        {
            Address addr = this.GetAddress(address);

            Transaction reward = new Transaction(FirstAddress, addr, BlockReward, 0, string.Empty, new string[2]);
            this._pendingTransactionsByHash.TryAdd(reward.TransactionHash, reward);
            
            this.AddTransactionToAddress(reward.From, reward);
            this.AddTransactionToAddress(reward.To, reward);
        }

        public Transaction GetTransactionInfo(string hash)
        {
            if (this._pendingTransactionsByHash.ContainsKey(hash))
            {
                return this._pendingTransactionsByHash[hash];
            }

            return this._confirmedTransactionsByHash.ContainsKey(hash) ? this._confirmedTransactionsByHash[hash] : null;
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

        public IEnumerable<Transaction> GetTransactionsByAddressId(string addressId)
        {
            if (this._addresses.ContainsKey(addressId))
            {
                return this._addresses[addressId].Transactions.ToArray();
            }

            return null;
        }

        public void UpdateBlockchain(Block newBlock)
        {
            this._blockchain.Add(newBlock);
            this.PrepareCandidate();
        }

        public bool CheckForCollison(string transactionHash)
        {
            return (this._pendingTransactionsByHash.ContainsKey(transactionHash) ||
                    this._confirmedTransactionsByHash.ContainsKey(transactionHash));
        }

        public bool CheckSenderBalance(string senderId, ulong amount)
        {
            if (senderId == FirstAddress.AddressId)
            {
                return true;
            }

            if (!this._addresses.ContainsKey(senderId))
            {
                return false;
            }

            return (ulong) (this._addresses[senderId].Amount * Order) >= amount;
        }

        public void AddAddress(Address address)
        {
            this._addresses.TryAdd(address.AddressId, address);
        }

        public void AddMiningJob(string address, Block block)
        {
            if (!this._miningJobs.ContainsKey(address))
            {
                this._miningJobs.TryAdd(address, block);
            }
            else
            {
                this._miningJobs[address] = block;
            }
        }

        public Block GetMiningJob(string address)
        {
            if (!this._miningJobs.ContainsKey(address))
            {
                return null;
            }

            return this._miningJobs[address];
        }

        public void AddTransaction(Transaction transaction)
        {
            this._pendingTransactionsByHash.TryAdd(transaction.TransactionHash, transaction);

            this.AddAddressNewAddress(transaction.From);
            this.AddAddressNewAddress(transaction.To);

            this.AddTransactionToAddress(transaction.From, transaction);
            this.AddTransactionToAddress(transaction.To, transaction);
        }


        private void AddTransactionToAddress(string address, Transaction transaction)
        {
            if (!this._addresses.ContainsKey(address))
            {
                this.AddAddress(new Address(address));
            }

            this._addresses[address].Transactions.Add(transaction);
        }

        private void AddAddressNewAddress(string address)
        {
            if (!this._addresses.ContainsKey(address))
            {
                this._addresses.TryAdd(address, new Address(address));
            }
        }

        private IEnumerable<Transaction> ProcessTransactions(IEnumerable<Transaction> transactions)
        {
            Transaction[] processTransactions = transactions as Transaction[] ?? transactions.ToArray();
            foreach (Transaction t in processTransactions)
            {
                Address from = this.GetAddress(t.From);
                from.Amount -= (long) t.Value;
                Address to = this.GetAddress(t.To);
                to.Amount += (long) t.Value;

                t.MinedInBlockIndex = _candidate.Index;
                t.TransferSuccessful = true;

                this._confirmedTransactionsByHash.TryAdd(t.TransactionHash, t);
                this._pendingTransactionsByHash.TryRemove(t.TransactionHash, out var ignore);
            }

            return processTransactions;
        }

        public bool IsBlockValid(Block block)
        {
            // TODO: Validate block index, previous blockhash, BlockHash(based on Nonce, date, BlockDataHash)
            // TODO: Validate all transactions - if the amounts are available, signature, TransactionHash
            // TODO: Validate BlockDataHash

            return true;
        }

        private void ProcessGenesisBlock()
        {
            if (_genesisBlock != null) return;
            Transaction faucet = new Transaction(this.FirstAddress,
                new Address("bee3f694bf0fbf9556273e85d43f2e521d24835e"), 2000000000, 0, "0", new string[2]);
            faucet.MinedInBlockIndex = 0;
            faucet.TransferSuccessful = true;

            this._confirmedTransactionsByHash.TryAdd(faucet.TransactionHash, faucet);
            this.AddAddressNewAddress(faucet.To);
            this.AddTransactionToAddress(faucet.From, faucet);
            this.AddTransactionToAddress(faucet.To, faucet);
            this._addresses[faucet.To].Amount += (long) faucet.Value;

            List<Transaction> transactions = new List<Transaction>() {faucet};
            _genesisBlock = new Block(0, 5, string.Empty,
                transactions, this.FirstAddress.AddressId);

            this._blockchain.Add(_genesisBlock);
            this.PrepareCandidate();
        }

        private void PrepareCandidate()
        {
            IEnumerable<Transaction> transactions = this.ProcessTransactions(GetTransactions(false));
            _candidate = new Block(this._blockchain.Count, 5, this._blockchain.Last().BlockHash, transactions);
        }
    }
}