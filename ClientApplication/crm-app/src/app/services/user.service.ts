import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ExecutorUser } from '../models/executorUser';

@Injectable({
    providedIn: 'root'
})

export class UserService {
    constructor(private http: HttpClient) { }

    getExecutorUsers(): Observable<{ data: ExecutorUser[] }> {

        let url = '/user/executor';

        if (!environment.production) {
            url = environment.devApiUrl + url;
        }

        return this.http.get(url) as Observable<{ data: ExecutorUser[] }>;
    }
}
