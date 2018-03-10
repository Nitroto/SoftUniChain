export interface Transaction {
  from: string;
  to: string;
  value: number;
  fee: number;
  dateCreated: string;
  senderSignature: string[];
  senderPublicKey: string;
  transactionHash: string;
  minedInBlockIndex: number;
  transferSuccessful: boolean;
}
