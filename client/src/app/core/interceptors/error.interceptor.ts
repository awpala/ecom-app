import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
    ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error) {
          const { status, error: { message, errors } } = error;

          if (status === 400) {
            if (errors) {
              throw error.error;
            } else {
              this.toastr.error(message, status.toString());
            }
          }
          if (status === 401) {
            this.toastr.error(message, status.toString());
          }
          if (status === 404) {
            this.router.navigateByUrl('/not-found');
          }
          if (status === 500) {
            const navigationExtras: NavigationExtras = {
              state: {
                error: error.error,
              }
            };
            this.router.navigateByUrl('/server-error', navigationExtras);
          }
        }
        return throwError(() => new Error(error.message)); 
      })
    );
  }
}
