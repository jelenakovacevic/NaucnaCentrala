import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';
import { AccountsService } from '../services/account.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(public accountsService: AccountsService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    
    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${this.accountsService.getToken()}`
      }
    });

    return next.handle(request);
  }
}