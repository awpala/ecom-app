import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { delay, finalize, Observable } from 'rxjs';
import { BusyService } from '../services/busy.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyService: BusyService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler,
  ): Observable<HttpEvent<unknown>> {
    // disable full-page loading if performing validation `emailExists` via component `RegisterComponent`
    if (!request.url.includes('emailExists')) {
      this.busyService.busy();
    }

    return next.handle(request)
    .pipe(
        // add delay to make page loads less "instantaneous"
        delay(500),
        finalize(() => this.busyService.idle()),
      );
  }
}
