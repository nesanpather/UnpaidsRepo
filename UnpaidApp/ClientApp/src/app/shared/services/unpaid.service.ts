import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IUnpaidInput } from '../models/unpaid-input';
import { IUnpaidNotifications } from '../models/unpaid-notifications';
import { IUnpaidNotificationsResponse } from '../models/unpaid-notifications-response';
import { IUser } from '../models/user';

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

  public getUnpaidNotificationsByDateRange(dateType: number, dateFrom: string, dateTo: string): Observable<IUnpaidNotifications[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    return this.httpClient.get<IUnpaidNotifications[]>(`${this.unpaidApiBaseUrl}/unpaids/datefrom/${dateFrom}/dateto/${dateTo}/datetype/${dateType}`, httpOptions);
  }

  public getUnpaidNotificationResponses(): Observable<IUnpaidNotificationsResponse[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    return this.httpClient.get<IUnpaidNotificationsResponse[]>(`${this.unpaidApiBaseUrl}/unpaids/responses`, httpOptions);
  }

  public getUnpaidNotificationResponsesByDateRange(dateType: number, dateFrom: string, dateTo: string): Observable<IUnpaidNotificationsResponse[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    return this.httpClient.get<IUnpaidNotificationsResponse[]>(`${this.unpaidApiBaseUrl}/unpaids/responses/datefrom/${dateFrom}/dateto/${dateTo}/datetype/${dateType}`, httpOptions);
  }

  public authenticateUser(): Observable<IUser> {
    const httpOptions = {
      withCredentials: true
    };

    return this.httpClient.get<IUser>(`${this.unpaidApiBaseUrl}/unpaids/AuthenticateUser`, httpOptions);
  }
}
