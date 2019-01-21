import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HelperService {

  constructor() { }

  public formatDate(dateToFormat: Date): string {
    var dateString = dateToFormat.getFullYear() +
      "-" +
      (`0${dateToFormat.getMonth() + 1}`).slice(-2) +
      "-" +
      (`0${dateToFormat.getDate()}`).slice(-2);

    return dateString;
  }
}
