using System;
using System.Linq;
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
                from = transaction.From.AddressId,
                to = transaction.To.AddressId,
                senderPublicKey = transaction.SenderPublicKey,
                value = transaction.Value,
                fee = transaction.Fee,
                dateCreated = transaction.DateCreated
            };

            string transactionPayLoadAsString = JsonConvert.SerializeObject(transactionData).Replace(" ", "");
            string transactionHash = BytesToHex(CalcSha256(transactionPayLoadAsString));

            Console.WriteLine(transactionPayLoadAsString);
            Console.WriteLine(transactionHash);
            if (transactionHash != transaction.TransactionHash)
            {
                return false;
            }

            // validate signature???

            return true;
        }

        private byte[] CalcSha256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            Sha256Digest digest = new Sha256Digest();
            digest.BlockUpdate(bytes, 0, bytes.Length);
            byte[] result = new byte[digest.GetDigestSize()];
            digest.DoFinal(result, 0);

            return result;
        }

        private string BytesToHex(byte[] bytes)
        {
            return string.Concat(bytes.Select(b => b.ToString("x2")));
        }
    }
}