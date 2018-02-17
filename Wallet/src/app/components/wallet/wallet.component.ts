import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material';
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

  wallet = {
    privateKey: '',
    publicKey: '',
    address: ''
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

  signTransaction() {
    this.openDialog();
    console.log('Sign...');
  }

  loadUserDataFromChain() {
    const clientUrl = `${this.node}/api/address/${this.wallet.address}`;
    this.user = this._walletServices.getClientData(clientUrl);
  }

  openDialog() {
    this.transactionDialog = this.dialog.open(TransactionContentDialog, {
      hasBackdrop: false,
      height: '350px'
    });
  }

  private sendSignedTransaction() {
    this.transactionDialog.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
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
  }
}

@Component({
  selector: 'transaction-content-dialog',
  templateUrl: './transaction-content-dialog.html',
})
export class TransactionContentDialog {
  constructor(private dialogRef: MatDialogRef<TransactionContentDialog>) {

  }
}
