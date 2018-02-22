import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material';
import { WalletService } from '../../services/wallet.service';
import {FormControl, FormGroup, NgForm} from '@angular/forms';

import * as elliptic from 'elliptic';
import * as hashes from 'jshashes';
import * as utf8 from 'utf8';
import {Observable} from 'rxjs/Observable';


@Component({
  selector: 'wallet',
  templateUrl: './wallet.component.html',
  styleUrls: ['./wallet.component.css']
})
export class WalletComponent implements OnInit {

  customSelection = new FormControl(false);
  privateKeyPattern = '^[A-Za-z0-9]{64}$';
  addressPattern = '^[A-Za-z0-9]{40}$';

  wallet = {
    privateKey: '',
    publicKey: '',
    address: ''
  };

  transaction = {
    from: '',
    to: '',
    amount: 0.00000
  };

  user: {};

  node = 'http://localhost:5000';
  info$: any;
  transactions$: any;

  constructor(public dialog: MatDialog, private _walletServices: WalletService) {
  }

  ngOnInit(): void {
    if (sessionStorage['privateKey']) {
      this.wallet.privateKey = sessionStorage['privateKey'];

    }

    if (sessionStorage['privateKey']) {
      this.wallet.publicKey = sessionStorage['publicKey'];
    }

    if (sessionStorage['address']) {
      this.wallet.address = sessionStorage['address'];
      this.transaction.from = sessionStorage['address'];
    }
  }

  getNodeInfo(): void {
    let address = this.node + '/api/info';
    this.info$ = this._walletServices.getData(address);
  }

  generateNewWallet(): void {
    const ec = new elliptic.ec('secp256k1');
    const keyPair = ec.genKeyPair();
    this.saveKey(keyPair);
    this.loadUserDataFromChain();
  }

  loadWallet(): void {
    const userPrivateKey = this.wallet.privateKey;
    const ec = new elliptic.ec('secp256k1');
    const keyPair = ec.keyFromPrivate(userPrivateKey);
    this.saveKey(keyPair);
    this.loadUserDataFromChain();
  }

  loadUserDataFromChain(): void {
    const clientUrl = this.node + '/api/address/' + this.wallet.address;
    this.user = this._walletServices.getData(clientUrl);
  }

  signTransaction(form: NgForm): void {
    const transactionPayLoad = {
      'from': this.transaction.from,
      'to': this.transaction.to,
      'senderPublicKey': this.wallet.publicKey,
      'amount': this.transaction.amount,
      'dateCreated': new Date().toISOString()
    };
    const transactionPayLoadAsString = JSON.stringify(transactionPayLoad).toString();
    const transactionPayloadHash = new hashes.SHA256().hex_hmac(utf8.encode(transactionPayLoadAsString));
    const privateKey = new elliptic.ec('secp256k1').keyFromPrivate(this.wallet.privateKey);
    const transactionSignature = privateKey.sign(transactionPayloadHash);
    const senderSignature = [transactionSignature.r.toString(16), transactionSignature.s.toString(16)];
    const data = transactionPayLoad;
    data['senderSignature'] = senderSignature;
    this.openSigningDialog(data).subscribe(result => {
      if (result) {
        const address = this.node + '/api/transactions';
        data['transactionHash'] = new hashes.SHA256().hex_hmac(utf8.encode(JSON.stringify(data).toString()));
        this._walletServices.sendSigntTransaction(address, data);
        form.resetForm();
      }
    });
  }

  private getUserTransactions() {
    
  }

  private openSigningDialog(data): Observable<any> {
    const transactionDialog = this.dialog.open(TransactionContentDialog, {data: data});
    return transactionDialog.afterClosed();
  }

  private saveKey(keyPair): void {
    const privateKey = keyPair.getPrivate().toString(16);
    sessionStorage['privateKey'] = privateKey;
    const publicKey = keyPair.getPublic().getX().toString(16) + (keyPair.getPublic().getY().isOdd() ? '1' : '0');
    sessionStorage['publicKey'] = publicKey;
    const ripemd160 = new hashes.RMD160();
    const address = ripemd160.hex(publicKey);
    sessionStorage['address'] = address;

    this.wallet = {
      privateKey: privateKey,
      publicKey: publicKey,
      address: address
    };

    this.transaction.from = address;
  }
}

@Component({
  selector: 'transaction-content-dialog',
  templateUrl: './transaction-content-dialog.html',
})
export class TransactionContentDialog {
  constructor(private dialogRef: MatDialogRef<TransactionContentDialog>, @Inject(MAT_DIALOG_DATA) public data: any) {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
