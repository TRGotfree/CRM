import { Injectable } from '@angular/core';
import { UserTaskState } from '../models/userTaskState';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class UserTaskStateService {
    constructor(private http: HttpClient) { }

    getPriorities(): Observable<{ data: UserTaskState[] }> {

        let url = '/usertaskstate';

        if (!environment.production) {
            url = environment.devApiUrl + url;
        }

        return this.http.get(url) as Observable<{ data: UserTaskState[] }>;
    }
}
