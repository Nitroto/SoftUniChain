import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {Observable} from 'rxjs/Observable';
import {RequestOptions} from '@angular/http';

@Injectable()
export class WalletService {
  constructor(private _http: HttpClient) { }


  getClientData(clientAddress): Observable<any> {
    return this._http.get(clientAddress);
  }

  sendSigntTransaction(address, transaction): Observable<any> {
    const data = JSON.stringify(transaction);
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const options = { headers: headers };

    return this._http.post(address, data, options);
  }

  getInfo(address): Observable<any> {
    return this._http.get(address);
  }
}
