import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UnpaidInput } from '../models/unpaid-input';
import { UnpaidNotificationsResponse } from '../models/unpaid-notifications-response';

@Injectable({
  providedIn: 'root'
})
export class UnpaidService {
  unpaidApiBaseUrl: string = 'http://tts.devtest.com/api/v1';

  constructor(private httpClient: HttpClient) { }

  public addUnpaid(unpaidInputs: UnpaidInput[]): any {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'        
      })
    };

    return this.httpClient.post(`${this.unpaidApiBaseUrl}/unpaids/add`, unpaidInputs, httpOptions);
  }

  public getUnpaidNotifications(): Observable<UnpaidNotificationsResponse[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    return this.httpClient.get<UnpaidNotificationsResponse[]>(`${this.unpaidApiBaseUrl}/unpaids`, httpOptions);
  }
}
