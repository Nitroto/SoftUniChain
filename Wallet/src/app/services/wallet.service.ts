import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class WalletService {
  private _headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Access-Control-Allow-Origin': '*' });

  constructor(private _http: HttpClient) { }

  getData(address): Observable<any> {
    const options = { headers: this._headers };
    return this._http.get(address, options);
  }

  sendSigntTransaction(address, transaction): Observable<any> {
    const data = JSON.stringify(transaction);
    const options = { headers: this._headers };

    return this._http.post(address, data, options);
  }
}
