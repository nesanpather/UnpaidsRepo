import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IUnpaidInput } from '../models/unpaid-input';
import { IUnpaidNotifications } from '../models/unpaid-notifications';

@Injectable({
  providedIn: 'root'
})
export class UnpaidService {
  unpaidApiBaseUrl: string = 'http://tts.devtest.com/api/v1';

  constructor(private httpClient: HttpClient) { }

  public addUnpaid(unpaidInputs: IUnpaidInput[]): any {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'        
      })
    };

    return this.httpClient.post(`${this.unpaidApiBaseUrl}/unpaids/add`, unpaidInputs, httpOptions);
  }

  public getUnpaidNotifications(): Observable<IUnpaidNotifications[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    return this.httpClient.get<IUnpaidNotifications[]>(`${this.unpaidApiBaseUrl}/unpaids`, httpOptions);
  }
}
