import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class WalletService {
  constructor(private _http: HttpClient) { }


  getData(address): Observable<any> {
    return this._http.get(address);
  }

  sendSigntTransaction(address, transaction): Observable<any> {
    const data = JSON.stringify(transaction);
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const options = { headers: headers };

    return this._http.post(address, data, options);
  }
}
