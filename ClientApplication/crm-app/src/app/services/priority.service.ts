import { Injectable } from '@angular/core';
import { Priority } from '../models/priority';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class PriorityService {
    constructor(private http: HttpClient) { }

    getPriorities(): Observable<{ data: Priority[] }> {

        let url = '/priority';

        if (!environment.production) {
            url = environment.devApiUrl + url;
        }

        return this.http.get(url) as Observable<{ data: Priority[] }>;
    }
}
