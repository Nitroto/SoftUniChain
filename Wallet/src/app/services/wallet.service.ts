import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class WalletService {

  constructor(private _http: HttpClient) { }


  getClientData(clientAddress) {
    return this._http.get(clientAddress);
  }
}
