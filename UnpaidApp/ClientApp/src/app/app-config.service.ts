import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment';


@Injectable()
export class AppConfigService {
  private appConfig;
  assetsDirectory: string = environment.assetsDirectory;

  constructor(private http: HttpClient) { }

  loadAppConfig() {
    return this.http.get(`/${this.assetsDirectory}`)
      .toPromise()
      .then(data => {
        this.appConfig = data;
      });
  }

  getConfig() {
    return this.appConfig;
  }
}
