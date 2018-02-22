using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Node.Interfaces;
using Node.Models;
using Org.BouncyCastle.Crypto.Digests;

namespace Node.Services
{
    public class TransactionService : ITransactionService
    {
        public void Create(Transaction transaction)
        {
            throw new System.NotImplementedException();
        }

        public Transaction Sign(Transaction transaction, string privateKey)
        {
            // get transactionData
            // hash transactionData
            // sign transaction data
            // populate signature in the transaction
            throw new System.NotImplementedException();
        }

        public bool Validate(Transaction transaction)
        {
            var transactionData = new
            {
                from = transaction.From,
                to = transaction.To,
                senderPublicKey = transaction.SenderPublicKey,
                value = transaction.Value,
                fee = transaction.Fee,
                dateCreated = transaction.DateCreated
            };

            string transactionPayLoadAsString = JsonConvert.SerializeObject(transactionData);
            string transactionHash = CalcSha256(transactionPayLoadAsString);

            Console.WriteLine(transactionHash);
//            if (transactionHash != transaction.TransactionHash)
//            {
//                return false;
//            }
            // get transactionData
            // hash transactionData
            // validate signature???

            return true;
        }

        private string CalcSha256(string text)
        {
            using (var sha256 = SHA256.Create() )
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                var hashBytes = sha256.ComputeHash(bytes);
                var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}