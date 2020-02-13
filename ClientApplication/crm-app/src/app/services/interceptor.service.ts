import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';


@Injectable()
export class TokenInterceptor implements HttpInterceptor {

    constructor(private router: Router) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        if (sessionStorage.getItem('jwt')) {
            req = req.clone({
                setHeaders: {
                    Authorization: sessionStorage.getItem('jwt')
                }
            });
        }

        return next.handle(req).pipe(catchError((error: HttpErrorResponse) => {
            if (error && error.status === 401 || error.status === 403) {
                this.router.navigate(['/authentification']);
            }
            return throwError(error);

        }));

    }
}
