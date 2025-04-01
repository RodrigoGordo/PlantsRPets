import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, tap } from "rxjs";
import { AuthorizeService } from "./authorize.service";

// this will intercept all http requests and redirect to signin if the user is not authenticated and
// is trying to access a protected route

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthorizeService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('authToken');
    let authReq = req;

    if (token) {
      authReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }

    return next.handle(authReq).pipe(
      tap({
        error: (event) => {
          if (event instanceof HttpErrorResponse && event.status === 401) {
            localStorage.removeItem('authToken');
            this.authService.signOut();
            this.authService.requestLoginPopup();
          }
        },
      })
    );
  }
}


