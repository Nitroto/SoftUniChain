using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Node.Interfaces;
using Node.Models;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security;

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

//            if (transactionHash != transaction.TransactionHash)
//            {
//                return false;
//            }

            // validate signature???
            return VerifySignature(transaction.SenderPublicKey, transactionPayLoadAsString, transaction.SenderSignature);
        }

        private bool VerifySignature(string publicKey, string transaction, string[] signature)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] inputData = encoder.GetBytes(transaction);
            AsymmetricKeyParameter key = PublicKeyFactory.CreateKey(encoder.GetBytes(publicKey)) ;
            string signatureAsString = signature[0] + signature[1];
            byte[] sign = encoder.GetBytes(signatureAsString);
            ISigner signer = SignerUtilities.GetSigner("ECDSA");
            signer.Init(false, key);
            signer.BlockUpdate(inputData, 0, inputData.Length);
            bool result = signer.VerifySignature(sign);
            return result;
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