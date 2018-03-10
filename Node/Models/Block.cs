using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Node.Utilities;

namespace Node.Models
{
    public class Block
    {
        public Block(int index, long difficulty, string previousBlockHash, IEnumerable<Transaction> transactions,
            string minesBy = null, long? nonce = null)
        {
            this.Index = index;
            this.CreatedOn = DateTime.Now;
            this.Difficulty = difficulty;
            this.Nonce = nonce;
            this.MinedBy = minesBy;
            this.PreviousBlockHash = previousBlockHash;
            this.Transactions = transactions;
            this.BlockDataHash = CalculateBlockDataHash();
            this.BlockHash = this.GenerateHash();
        }

        public int Index { get; private set; }
        public DateTime CreatedOn { get; set; }
        public long Difficulty { get; private set; }
        public string MinedBy { get; set; }
        public string PreviousBlockHash { get; private set; }
        public string BlockHash { get; set; }
        public string BlockDataHash { get; private set; }
        public long? Nonce { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }

        public string GenerateHash()
        {
            var block = new
            {
                blockDataHash = this.BlockDataHash,
                nonce = this.Nonce,
                createdOn = this.CreatedOn,
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
                minedBy = this.MinedBy,
            };

            return Crypto.JsonToString(blockData);
        }
    }
}