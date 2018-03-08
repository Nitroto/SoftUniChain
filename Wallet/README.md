# SoftUniChain - Wallet

## SoftUni Chain Wallet (port 4200)

### Angular 5 with Material Theme

The wallet keeps a **private key** and its associated **public key** and **blockchain address**.

**ECDSA-based cryptography** is used, based on the [secp256k1 curve](https://en.bitcoin.it/wiki/Secp256k1).
- The **private key** is 256-bit integer, encoded as **64 hex digits**
- The **public key** is coordinate (x, y) on the secp256k1 elliptic curve, encoded in compressed form as **65 hex digits**:
  + 64 hex digits for the x coordinate.
  + 1 hex digit for the odd / even of y (0 or 1) at the end.
  
The **blockchain address** is 160-bit RIPEMD-160 hash of the compressed public key, encoded as **40 hex digits**
