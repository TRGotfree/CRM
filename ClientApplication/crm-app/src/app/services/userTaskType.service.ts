import { Injectable } from '@angular/core';
import { UserTaskType } from '../models/userTaskType';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class UserTaskTypeService {
    constructor(private http: HttpClient) { }

    getPriorities(): Observable<{ data: UserTaskType[] }> {

        let url = '/usertasktype';

        if (!environment.production) {
            url = environment.devApiUrl + url;
        }

        return this.http.get(url) as Observable<{ data: UserTaskType[] }>;
    }
}
