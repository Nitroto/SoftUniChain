import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material';
import { WalletService } from '../../services/wallet.service';
import { FormControl } from '@angular/forms';

import * as elliptic from 'elliptic';
import * as hashes from 'jshashes';


@Component({
  selector: 'wallet',
  templateUrl: './wallet.component.html',
  styleUrls: ['./wallet.component.css']
})
export class WalletComponent implements OnInit {

  customSelection = new FormControl(false);
  showGenerateLoadSection = true;
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

  node = 'http://127.0.0.1:5000';
  transactionDialog: MatDialogRef<TransactionContentDialog>;

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

  generateNewWallet() {
    const ec = new elliptic.ec('secp256k1');
    const keyPair = ec.genKeyPair();
    this.saveKey(keyPair);
    this.loadUserDataFromChain();
  }


  loadWallet() {
    const userPrivateKey = this.wallet.privateKey;
    const ec = new elliptic.ec('secp256k1');
    const keyPair = ec.keyFromPrivate(userPrivateKey);
    this.saveKey(keyPair);
    this.loadUserDataFromChain();
  }

  // private dialogRef: MatDialogRef<DialogContentDialog>

  loadUserDataFromChain() {
    const clientUrl = `${this.node}/api/address/${this.wallet.address}`;
    this.user = this._walletServices.getClientData(clientUrl);
  }



  signTransaction(): void {
    const data = {};
    this.openDialog();
    console.log('Sign...');
  }

  private sendSignedTransaction() {
    this.transactionDialog.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

  private openDialog() {
    this.transactionDialog = this.dialog.open(TransactionContentDialog, {
      data: {'from': this.transaction.from, 'to': this.transaction.to, 'amount': this.transaction.amount},

    });
  }

  private saveKey(keyPair) {
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
