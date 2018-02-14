using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Node.Models
{
    public class Block
    {
        private static Block _genesisBlock;

        public Block(int index, long difficulty, Address mineBy, string previousBlockHash,
            IList<Transaction> transactions)
        {
            this.Index = index;
            this.Difficulty = difficulty;
            this.MineBy = mineBy;
            this.PreviousBlockHash = previousBlockHash;
            this.Transactions = transactions;
        }

        public int Index { get; set; }
        public DateTime CreatedOn { get; set; }
        public long Difficulty { get; set; }
        public Address MineBy { get; set; }
        public string PreviousBlockHash { get; set; }
        public string BlockHash { get; set; }
        public string BlockDataHash { get; set; }
        public long Nonce { get; set; }
        public IList<Transaction> Transactions { get; set; }

        public void GenerateHash()
        {
            this.BlockDataHash = "hardcoded data hash, TODO merkel tree and hashing";
            this.CreatedOn = DateTime.UtcNow;
            this.Nonce = 1234567890;
            this.BlockHash = "hardcoded block hash - TODO use mining process";
        }

        public static Block Genesis
        {
            get
            {
                if (_genesisBlock == null)
                {
                    IList<Transaction> transactions = new List<Transaction>()
                    {
                        new Transaction{
                            From = Address.GeneratorAddress,
                            Amount = 100,
                            To = new Address("1"),
                            SenderPublicKey = "hardocoded SenderPublicKey",
                            SenderSignature = new List<string>{"hardcoded SenderSignatoure", "somee"}
                        }
                    };
                    _genesisBlock = new Block(0, 1, new Address("00"), string.Empty, transactions);
                }

                return _genesisBlock;
            }
        }
    }
}