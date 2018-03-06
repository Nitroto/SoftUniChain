using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace Node.Utilities
{
    public static class Crypto
    {
        public static readonly X9ECParameters Curve = SecNamedCurves.GetByName("secp256k1");

        public static byte[] CalcSha256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            Sha256Digest digest = new Sha256Digest();
            digest.BlockUpdate(bytes, 0, bytes.Length);
            byte[] result = new byte[digest.GetDigestSize()];
            digest.DoFinal(result, 0);

            return result;
        }

        public static string BytesToHex(byte[] bytes)
        {
            return string.Concat(bytes.Select(b => b.ToString("x2")));
        }

        public static ECPoint DecodeECPointFromPublicKey(string input)
        {
            BigInteger privKeyInt = new BigInteger(input, 16);
            byte[] compressedKey = privKeyInt.ToByteArray();
            ECPoint point = Curve.Curve.DecodePoint(compressedKey);
            return point;
        }

        public static string JsonToString(object obj)
        {
            return JsonConvert.SerializeObject(obj).Replace(" ", "");
        }

        public static bool ValidateAddress(string address)
        {
            Regex reg = new Regex("^[0-9a-f]{40}$");
            return reg.IsMatch(address);
        }
    }
}