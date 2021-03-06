import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef, MatTableModule } from '@angular/material';
import { WalletService } from '../../services/wallet.service';
import { FormControl, NgForm } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { ToastrService } from 'ngx-toastr';

import * as elliptic from 'elliptic';
import * as hashes from 'jshashes';
import * as utf8 from 'utf8';
import {Transaction} from './transaction';


@Component({
  selector: 'wallet',
  templateUrl: './wallet.component.html',
  styleUrls: ['./wallet.component.css']
})
export class WalletComponent implements OnInit {

  customSelection = new FormControl(false);
  privateKeyPattern = '^[a-f0-9]{64}$';
  addressPattern = '^[a-f0-9]{40}$';

  wallet = {
    privateKey: '',
    publicKey: '',
    address: ''
  };

  transaction = {
    from: '',
    to: '',
    value: 0,
    fee: 20,
  };

  node = 'http://localhost:5000';
  info$: Observable<any[]>;
  transactions$: Observable<Transaction[]>;
  user$: Observable<any[]>;

  constructor(public dialog: MatDialog, private _walletServices: WalletService, private _toastyService: ToastrService) {
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
      this.user$ = this.loadUserDataFromChain();
      this.getUserTransactions();
    }
  }

  getNodeInfo(): void {
    const address = this.node + '/api/info';
    this.info$ = this._walletServices.getData(address);
  }

  generateNewWallet(): void {
    const ec = new elliptic.ec('secp256k1');
    const keyPair = ec.genKeyPair();
    this.saveKey(keyPair);
    this.user$ = this.loadUserDataFromChain();
    this._toastyService.success('Wallet was generated successfully.', 'New wallet');
  }

  loadWallet(): void {
    const userPrivateKey = this.wallet.privateKey;
    const ec = new elliptic.ec('secp256k1');
    const keyPair = ec.keyFromPrivate(userPrivateKey);
    this.saveKey(keyPair);
    this.user$ = this.loadUserDataFromChain();
    this._toastyService.success('Wallet was loaded successfully.', 'Wallet recovered');
  }

  loadUserDataFromChain(): Observable<any> {
    const address = this.node + '/api/address/' + this.wallet.address;
    return this._walletServices.getData(address);
  }

  signTransaction(form: NgForm): void {
    const transactionPayLoad = {
      'from': this.transaction.from,
      'to': this.transaction.to,
      'senderPublicKey': this.wallet.publicKey,
      'value': this.transaction.value * 1000000,
      'fee': this.transaction.fee,
      'dateCreated': new Date().toISOString()
    };
    const transactionPayLoadAsString = JSON.stringify(transactionPayLoad).toString();
    const transactionPayloadHash = new hashes.SHA256().hex(utf8.encode(transactionPayLoadAsString));
    const privateKey = new elliptic.ec('secp256k1').keyFromPrivate(this.wallet.privateKey);
    const transactionSignature = privateKey.sign(transactionPayloadHash);
    const senderSignature = [transactionSignature.r.toString(16), transactionSignature.s.toString(16)];
    const data = transactionPayLoad;
    data['senderSignature'] = senderSignature;
    this.sendTransaction(data, form);
  }

  private sendTransaction(data, form: NgForm): void {
    this.openSigningDialog(data).subscribe(result => {
      if (result) {
        const address = this.node + '/api/transactions';
        this._walletServices.sendSignedTransaction(address, data).subscribe(() => {
          form.controls['recipient'].reset();
          form.controls['recipient'].clearValidators();
          form.controls['value'].reset();
          form.controls['value'].clearValidators();
          this._toastyService.success('Transaction was successfully added to blockchain.', 'Success');
          this.getUserTransactions();
        }, err => {
          console.log(err);
          this._toastyService.error('Transaction was unsuccessful.', 'Major Error');
        });
      }
    });
  }

  private getUserTransactions() {
    const url = this.node + '/api/address/' + this.wallet.address + '/transactions';
    this.transactions$ = this._walletServices.getData(url);
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

export interface Element {
  name: string;
  position: number;
  weight: number;
  symbol: string;
}
