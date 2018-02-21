import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable()
export class WalletService {
  constructor(private _http: HttpClient) { }


  getClientData(clientAddress) {
    return this._http.get(clientAddress);
  }

  sendSigntTransaction(address, transaction) {
    return this._http.post(address, JSON.stringify(transaction));
  }

  getInfo(address) {
    return this._http.get(address);
  }
}
