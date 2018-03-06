using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Node.Utilities;

namespace Node.Models
{
    public class Block
    {
        public Block(int index, long difficulty, long? nonce, Address mineBy, string previousBlockHash,
            IEnumerable<Transaction> transactions)
        {
            this.Index = index;
            this.CreatedOn = DateTime.Now;
            this.Difficulty = difficulty;
            this.Nonce = nonce;
            this.MineBy = mineBy;
            this.PreviousBlockHash = previousBlockHash;
            this.Transactions = transactions;
            this.BlockDataHash = CalculateBlockDataHash();
            this.BlockHash = this.GenerateHash();
        }

        public int Index { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public long Difficulty { get; private set; }
        public Address MineBy { get; private set; }
        public string PreviousBlockHash { get; private set; }
        public string BlockHash { get; private set; }
        public string BlockDataHash { get; private set; }
        public long? Nonce { get; private set; }
        public IEnumerable<Transaction> Transactions { get; private set; }

        public string GenerateHash()
        {
            var block = new
            {
                blockDataHash = this.BlockDataHash,
                nonce = this.Nonce,
            };

            string blockHash = Crypto.BytesToHex(Crypto.CalcSha256(Crypto.JsonToString(block)));
            ;
            return blockHash;
        }

        private string CalculateBlockDataHash()
        {
            string blockDataAsString = this.GetBlockDataPayload();
            string blockHash = Crypto.BytesToHex(Crypto.CalcSha256(blockDataAsString));
            return blockHash;
        }

        private string GetBlockDataPayload()
        {
            var blockData = new
            {
                index = this.Index,
                transactions = this.Transactions,
                dificulty = this.Difficulty,
                prevBlockHash = this.PreviousBlockHash,
                createdOn = this.CreatedOn,
                mineBy = this.MineBy,
            };

            return Crypto.JsonToString(blockData);
        }
    }
}