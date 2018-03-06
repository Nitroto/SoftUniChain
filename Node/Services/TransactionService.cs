using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Node.Interfaces;
using Node.Models;
using Org.BouncyCastle.Asn1.BC;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using ECPoint = Org.BouncyCastle.Math.EC.ECPoint;

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

        public string CalculateTransactionHash(Transaction transaction)
        {
            string transactionPayLoadAsString = this.GetTransactionPayload(transaction);
            string transactionHash = this.BytesToHex(this.CalcSha256(transactionPayLoadAsString));

            return transactionHash;
        }

        public bool Validate(Transaction transaction)
        {
            X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");
            ECDomainParameters ecSpec = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            IDsaKCalculator kCalculator = new HMacDsaKCalculator(new Sha256Digest());
            var point = DecodeECPointFromPublicKey(transaction.SenderPublicKey);
            ECPublicKeyParameters keyParameters = new ECPublicKeyParameters(point, ecSpec);
            ECDsaSigner signer = new ECDsaSigner(kCalculator);
            signer.Init(false, keyParameters);
            var pubKey1 = new BigInteger(transaction.SenderSignature[0], 16);
            var pubKey2 = new BigInteger(transaction.SenderSignature[1], 16);
            byte[] transactionHash = this.CalcSha256(this.GetTransactionPayload(transaction));

            return signer.VerifySignature(transactionHash, pubKey1, pubKey2);
        }

        private string GetTransactionPayload(Transaction transaction)
        {
            var transactionData = new
            {
                from = transaction.From.AddressId,
                to = transaction.To.AddressId,
                senderPublicKey = transaction.SenderPublicKey,
                value = transaction.Value,
                fee = transaction.Fee,
                dateCreated = transaction.DateCreated,
            };

            return JsonConvert.SerializeObject(transactionData).Replace(" ", "");
        }

        private ECPoint DecodeECPointFromPublicKey(string input)
        {
            BigInteger bigInt = new BigInteger(input, 16);
            byte[] compressedKey = bigInt.ToByteArray();
            X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");
            var point = curve.Curve.DecodePoint(compressedKey);
            return point;
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