import {Component, OnInit} from '@angular/core';
// import elliptic from 'elliptic';
// import hashes from 'hashes';

@Component({
  selector: 'create',
  templateUrl: './wallet.component.html',
  styleUrls: ['./wallet.component.css']
})
export class WalletComponent implements OnInit {


  wallet = {
    privateKey: '',
    publicKey: '',
    address: ''
  };

  constructor() {
  }

  ngOnInit(): void {
    if (sessionStorage['privateKey']) {
      this.wallet.privateKey = sessionStorage['privateKey'];
    }

    if (sessionStorage['privateKey']) {
      this.wallet.publicKey = sessionStorage['publicKey'];
    }

    if (sessionStorage['address']) {
      this.wallet.privateKey = sessionStorage['address'];
    }
  }

  generateNewWallet() {
    // let ec = new elliptic.ec('secp256k1');
    // let keyPair = ec.generatePair();
    // this.saveKey(keyPair);
    this.wallet = {
      privateKey: 'generated',
      publicKey: 'generated',
      address: 'generated'
    };
  }

  send(){

  }

  private loadWallet() {
  }

  private saveKey(keyPair) {
    // sessionStorage['privateKey'] = keyPair.getPrivateKey().toString(16);
    // let publicKey = keyPair.getPublic().getX().toString(16) + (keyPair.getPublic().getY().isOdd() ? '1' : '0');
    // sessionStorage['publicKey'] = publicKey;
    // // let ripemd160 = new Hashes.RMD160();
    // // sessionStorage['address'] = ripemd160.hex(publicKey);
  }
}
