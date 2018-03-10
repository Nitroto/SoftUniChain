using Node.Interfaces;
using Node.Models;
using Node.Utilities;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace Node.Services
{
    public class TransactionService : ITransactionService
    {
        public void Create(Transaction transaction)
        {
            throw new System.NotImplementedException();
        }

        public string CalculateTransactionHash(Transaction transaction)
        {
            string transactionPayLoadAsString = this.GetTransactionPayload(transaction);
            string transactionHash = Crypto.BytesToHex(Crypto.CalcSha256(transactionPayLoadAsString));

            return transactionHash;
        }

        public bool Validate(Transaction transaction)
        {
            ECDomainParameters ecSpec =
                new ECDomainParameters(Crypto.Curve.Curve, Crypto.Curve.G, Crypto.Curve.N, Crypto.Curve.H);
            IDsaKCalculator kCalculator = new HMacDsaKCalculator(new Sha256Digest());
//            var point = Crypto.DecodeECPointFromPublicKey(transaction.SenderPublicKey);
//            ECPublicKeyParameters keyParameters = new ECPublicKeyParameters(point, ecSpec);
//            ECDsaSigner signer = new ECDsaSigner(kCalculator);
//            signer.Init(false, keyParameters);
//            var pubKey1 = new BigInteger(transaction.SenderSignature[0], 16);
//            var pubKey2 = new BigInteger(transaction.SenderSignature[1], 16);
//            byte[] transactionHash = Crypto.CalcSha256(this.GetTransactionPayload(transaction));
//            bool isValid = signer.VerifySignature(transactionHash, pubKey1, pubKey2);
            return true;
        }

        private string GetTransactionPayload(Transaction transaction)
        {
            var transactionData = new
            {
                from = transaction.From,
                to = transaction.To,
                senderPublicKey = transaction.SenderPublicKey,
                value = transaction.Value,
                fee = transaction.Fee,
                dateCreated = transaction.DateCreated,
            };

            return Crypto.JsonToString(transactionData);
        }
    }
}