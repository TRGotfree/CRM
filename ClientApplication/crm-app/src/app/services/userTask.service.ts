import { Injectable } from '@angular/core';
import { UserTask } from '../models/userTask';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class UserTaskService {
    constructor(private http: HttpClient) { }

    getTasks(numberOfTasks: number): Observable<any> {

        let url = '/usertask';

        if (!environment.production) {
            url = environment.devApiUrl + url;
        }

        const httpParams = new HttpParams()
            .set('numberOfRows', numberOfTasks.toString());

        return this.http.get(url, { params: httpParams });
    }

    getSortedOrFilteredTasks(from: number, to: number,
                             orderBy: string, sortBy: string, filterBy: string, filterValue: string): Observable<any> {
        try {

            let url = '/usertask';

            if (!environment.production) {
                url = environment.devApiUrl + url;
            }

            const httpParams = new HttpParams()
                .set('from', from.toString())
                .set('to', to.toString())
                .set('orderBy', orderBy)
                .set('sortBy', sortBy)
                .set('filterBy', filterBy)
                .set('filterValue', filterValue);

            return this.http.get(url, { params: httpParams });

        } catch (error) {
            throw error;
        }
    }
}
