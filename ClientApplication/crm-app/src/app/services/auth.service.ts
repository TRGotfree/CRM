import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  constructor(private http: HttpClient) { }

  checkCredentials(user: User): Observable<{}> {

    if (user === null) {
      throw new Error('Input parameter is null');
    }

    if (!user.login) {
      throw new Error('Login not specified!');
    }

    if (!user.password) {
      throw new Error('Password not specified!');
    }

    let url = '/authentification';

    if (!environment.production) {
      url = environment.devApiUrl + url;
    }

    return this.http.post(url, user);
  }

}
